using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.HighroadEngine;
using UnityEngine.Events;

namespace MoreMountains.HighroadEngine
{
	/// <summary>
	/// Solid Controller class, that handles vehicles with suspension
	/// This allows for a more dynamic behaviour of the vehicles and allows for bumpy roads, slopes or even loops
	/// This controller also offers easier extendability thanks to the SolidBehaviourExtender.
	/// If you want to create a vehicle using this controller, you'll need to setup its suspension correctly, and pay attention to the weight repartition.
	/// For that, you can simply duplicate one of the demo vehicles, or have a look at the documentation that explains how to setup a vehicle, step by step.
	/// </summary>
	public class SolidController : BaseController 
	{
		/// 引擎的动力
		public float EngineForce = 1000;
		[Header("Vehicule Physics")]
		/// 车辆的重心位置设置在下方。这有助于Unity物理系统保持车辆的稳定性
		public Vector3 CenterOfMass = new Vector3(0, -1, 0);
		[Range(0.0f, 5f)]
		/// 认为车辆与地面接触的距离
		public float GroundDistance = 1f;
		[Range(1, 10)]
		/// 离开道路时的惩罚系数
		public float OffroadPenaltyFactor = 2f;
		/// 轮胎的抓地力。数值越高，车辆在转弯时滑动越少
		public float CarGrip = 10f;
		/// 车辆被认为处于全油门状态的速度阈值。车辆的速度可以高于该阈值
		public float FullThrottleVelocity = 30;
		[Range(0.0f, 5f)]
		/// 车辆进行转向所需的最小速度
		public float MinimalTurningSpeed = 1f;
		[Range(-5.0f, 5f)]
		/// 施加前进力的高度
		public float ForwardForceHeight = 1f;
		/// 基于速度的额外扭矩力
		public AnimationCurve TorqueCurve;
		/// 后退时的旋转力
		public AnimationCurve BackwardForceCurve;
		[Range(0.0f, 1f)]
		/// 抓地力系数的倍数。数值越高，车辆越能够在高速下保持与地面的附着力
		public float GripSpeedFactor = 0.02f;
		[Range(0, 200)]
		/// 车辆的最大抓地力值
		public int MaxGripValue = 100;

		[Header("Suspension System")]
		/// 车轮的尺寸
		public float RadiusWheel = 0.5f;
		/// 弹簧常数
		public float SpringConstant = 20000f;
		/// 阻尼常数
		public float DamperConstant = 2000f;
		/// 当悬挂处于静止状态时，弹簧的长度
		public float RestLength = 0.5f;
		/// 转向时模拟弹簧压缩的水平旋转力，用于将车辆左右倾斜
		public float SpringTorqueForce = 1000f;
		/// 当车辆与其他物体发生碰撞时触发的事件
		public UnityAction<Collision> OnCollisionEnterWithOther;
		/// 当车辆重新生成时触发的事件
		public UnityAction OnRespawn;

		protected float _springForce = 0f;
		protected float _damperForce = 0f;
		protected RaycastHit _hit;
		protected Vector3 _startPosition;
		protected Quaternion _startRotation;
		protected GameObject _groundGameObject;
		protected LayerMask _noLayerMask = ~0;

		/// 车辆的档位枚举。可以向前行驶或向后行驶（倒车）
		public enum Gears { forward, reverse }
		/// 当前档位值
		public Gears CurrentGear { get; protected set; }
		/// 轮胎使用的当前引擎力值
		public Vector3 CurrentEngineForceValue { get; protected set; }
		/// 获取一个值，该值指示此车辆是否离开了道路。
		public virtual bool IsOffRoad
		{
			get { return (_groundGameObject != null && _groundGameObject.tag == "OffRoad"); }
		}
		/// <summary>
		/// 获取规范化的速度。
		/// </summary>
		/// <value>规范化的速度。</value>
		public virtual float NormalizedSpeed
		{
			get { return Mathf.InverseLerp(0f, FullThrottleVelocity, Mathf.Abs(Speed)); }
		}
		/// <summary>
		/// 如果车辆向前行驶，则返回true。
		/// </summary>
		/// <value><c>true</c> 表示向前行驶，否则为 <c>false</c>。</value>
		public virtual bool Forward
		{
			get { return transform.InverseTransformDirection(_rigidbody.velocity).z > 0; }
		}
		/// <summary>
		/// 如果车辆正在刹车，则返回true。
		/// </summary>
		/// <value><c>true</c> 表示正在刹车，否则为 <c>false</c>。</value>
		public virtual bool Braking
		{
			get { return Forward && (CurrentGasPedalAmount < 0); }
		}
		/// <summary>
		/// 返回车辆与水平面之间的角度。
		/// 用于禁用AI的方向控制，当车辆超过一定角度时。
		/// 允许更容易处理翻转
		/// </summary>
		/// <value>水平角度。</value>
		public virtual float HorizontalAngle
		{
			get { return Vector3.Dot(Vector3.up, transform.up); }
		}

		/// <summary>
		/// 获取前进的规范化速度。
		/// 用于评估引擎的动力
		/// </summary>
		/// <value>前进的规范化速度。</value>
		public virtual float ForwardNormalizedSpeed
		{
			get
			{
				float forwardSpeed = Vector3.Dot(transform.forward, _rigidbody.velocity);
				return Mathf.InverseLerp(0f, FullThrottleVelocity, Mathf.Abs(forwardSpeed));
			}
		}

		/// 车辆的当前侧向速度值
		public virtual float SlideSpeed { get; protected set; }

		/// <summary>
		/// Physics initialization
		/// </summary>
		protected override void Awake() 
		{
			base.Awake();

			// we change the center of mass below the vehicle to help with unity physics stability
			_rigidbody.centerOfMass += CenterOfMass;

			CurrentGear = Gears.forward;
		}

        /// <summary>
        /// Unity start function
        /// </summary>
        protected override void Start()
        {
            base.Start();
            _startPosition = transform.position;
            _startRotation = transform.rotation;
        }

        /// <summary>
        /// Update main function
        /// </summary>
        protected virtual void Update() 
		{
			UpdateGroundSituation();

			/*	MMDebug.DebugOnScreen("Steering", CurrentSteeringAmount);
			MMDebug.DebugOnScreen("acceleration", CurrentGasPedalAmount);
			MMDebug.DebugOnScreen("Speed", Speed);
			MMDebug.DebugOnScreen("SlideSpeed", SlideSpeed);
			MMDebug.DebugOnScreen("IsGrounded", IsGrounded);
			MMDebug.DebugOnScreen("Forward", Forward);
			MMDebug.DebugOnScreen("Braking", Braking);
			MMDebug.DebugOnScreen("_engineForce", CurrentEngineForceValue);
			MMDebug.DebugOnScreen("_rotationForce", CurrentRotationForceValue);
			MMDebug.DebugOnScreen("ForwardNormalizedSpeed", ForwardNormalizedSpeed);
			MMDebug.DebugOnScreen("HorizontalAngle", HorizontalAngle);*/
		}

		/// <summary>
		/// Updates the ground situation for this car.
		/// </summary>
		protected virtual void UpdateGroundSituation() 
		{
			IsGrounded = Physics.Raycast(transform.position, -transform.up, out _hit, GroundDistance, _noLayerMask, QueryTriggerInteraction.Ignore) ? true : false;
			_groundGameObject = _hit.transform != null ? _hit.transform.gameObject : null;
		}

		/// <summary>
		/// Fixed update.
		/// We apply physics and input evaluation.
		/// </summary>
		protected virtual void FixedUpdate() 
		{
			UpdateEngineForceValue();

			UpdateSlideForce();

			UpdateTorqueRotation();

			UpdateAirRotation();
		}

		/// <summary>
		/// Computes the engine's power. This value can be used by a wheel to apply force if conditions are met
		/// </summary>
		protected virtual void UpdateEngineForceValue()
		{
			// we use this intermediary variable to account for backwards mode
			float gasPedalForce = CurrentGasPedalAmount;

			if (IsOffRoad) 
			{
				gasPedalForce /= OffroadPenaltyFactor;
			}

			// if the player is accelerating
			if (CurrentGasPedalAmount > 0)
			{
				CurrentGear = Gears.forward;
			}

			// if the player is braking
			if (CurrentGasPedalAmount < 0)
			{
				// if it's fast enough, the car starts braking
				if ((Speed > MinimalTurningSpeed) && (CurrentGear == Gears.forward) && Forward)
				{
					// braking
				} else
				{
					// Otherwise, car is slow enough to go reverse
					CurrentGear = Gears.reverse;

					// We apply going backward penalty
					gasPedalForce = -BackwardForceCurve.Evaluate(-gasPedalForce);
				}
			}

			CurrentEngineForceValue = (Quaternion.AngleAxis(90, transform.right) * _hit.normal * (EngineForce * gasPedalForce));
		}

		/// <summary>
		/// Applies a torque force to the vehicle when the user wants to turn
		/// </summary>
		protected virtual void UpdateTorqueRotation()
		{
			if (IsGrounded)
			{ 
				Vector3 torque = transform.up * Time.fixedDeltaTime * _rigidbody.mass * TorqueCurve.Evaluate(NormalizedSpeed) * SteeringSpeed;
				// Going backward, we invert steering
				if (CurrentGear == Gears.reverse) 
				{
					torque = -torque;
				}
				_rigidbody.AddTorque(torque * CurrentSteeringAmount);
				// Horizontal torque. Simulates spring compression.
				_rigidbody.AddTorque(transform.forward * SpringTorqueForce * Time.fixedDeltaTime * _rigidbody.mass * CurrentSteeringAmount * ForwardNormalizedSpeed);
			}
		}

		/// <summary>
		/// Applies slide force to the vehicle
		/// </summary>
		protected virtual void UpdateSlideForce()
		{
			if (IsGrounded)
			{
				// We store the horizontal velocity
				Vector3 flatVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

				// we compute our lateral speed value
				// we store it so it can be used by skidmarks
				SlideSpeed = Vector3.Dot(transform.right, flatVelocity);

				// we compute the vehicle's grip value based on speed and settings
				float grip = Mathf.Lerp(MaxGripValue, CarGrip, Speed * GripSpeedFactor);

				Vector3 slideForce = transform.right * (-SlideSpeed * grip);
				_rigidbody.AddForce(slideForce * Time.fixedDeltaTime * _rigidbody.mass);
			}
		}

		/// <summary>
		/// Handles rotation of the vehicle in the air
		/// </summary>
		protected virtual void UpdateAirRotation()
		{
			if (!IsGrounded)
			{
				// Slow turning in air
				if (Speed > MinimalTurningSpeed)
				{
					Vector3 airRotationForce = transform.up * CurrentSteeringAmount * SteeringSpeed * Time.fixedDeltaTime * _rigidbody.mass;
					_rigidbody.AddTorque(airRotationForce);
				}
			}
		}

		/// <summary>
		/// Resets the position of the vehicle.
		/// </summary>
		public override void Respawn()
		{
			Vector3 resetPosition;
			Quaternion resetRotation;

			// Getting current reset position 
			if (Score == 0)
			{
				resetPosition = _startPosition;
				resetRotation = _startRotation;
			}
			else 
			{
				Transform resetTransform = _currentWaypoint == 0 ? _checkpoints[_checkpoints.Length - 1] : _checkpoints[_currentWaypoint - 1];
				resetPosition = resetTransform.position;
				resetRotation = resetTransform.rotation;
			}

			_rigidbody.velocity = Vector3.zero;
			transform.position = resetPosition;
			transform.rotation = resetRotation;

            OnRespawn();
        }

		/// <summary>
		/// Raises the collision enter event.
		/// </summary>
		/// <param name="other">Other object.</param>
		protected virtual void OnCollisionEnter(Collision other)
		{
			if (OnCollisionEnterWithOther != null) 
			{
				OnCollisionEnterWithOther(other);
			}
		}

		/// <summary>
		/// Draws debug info
		/// </summary>
		protected virtual void OnDrawGizmos() 
		{
			// distance to ground
			Gizmos.color = Color.green;
			Gizmos.DrawLine (transform.position, transform.position - (transform.up * (GroundDistance)));
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using Missions;

public class CarInteraction : MonoBehaviour
{
    public MissionEvent FocusCameraOnCar;
    public ParticleSystem Smokes;
    public int SmokeCollisionNum;
    public float ExplosionForce = 10f;
    public float pushForce = 10000f;
    private ParticleSystem.EmissionModule _smokes;
    private SolidController controller;
    [SerializeField]
    private int collisionNum;
    private Rigidbody rb;

    private CarManager carManager;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        InitSmoke();
        controller = GetComponent<SolidController>();
        rb = GetComponent<Rigidbody>();
        controller.OnCollisionEnterWithOther += OnVehicleCollisionEnter;
        carManager = GetComponent<CarManager>();
    }
    private void Update()
    {
        OvertakingSystem();
    }

    #region 超车系统
    public float overtakingProbability = 0.2f;
    public float overtakingDistance = 15f;
    public float overtakingSpeedThreshold = 10f;
    public float overtakingForce = 20000f; // 超车时施加的推力大小
    public float overtakingDuration = 2f;

    private Transform frontCar;
    private bool isOvertaking = false;
    
    private void OvertakingSystem()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, overtakingDistance))
        {
            if (hit.collider.CompareTag(GlobalConstants.CARTAGNAME))
            {
                frontCar = hit.collider.transform;
                TryOvertake(hit);
                
            }
        }
    }

    private void TryOvertake(RaycastHit hit)
    {
        if (!isOvertaking /*&& rb.velocity.magnitude > overtakingSpeedThreshold*/)
        {
            if (Random.value < overtakingProbability)
            {
                int direction = Random.Range(0, 2); // 0代表左边，1代表右边
                //Vector3 overtakingForceVector = Quaternion.Euler(0, (direction == 0) ? -90f : 90f, 0) * transform.forward;
                Vector3 overtakingForceVector = (direction == 0) ? -transform.right : transform.right;
                

                // 在超车结束后，重置正在超车标志
                StartCoroutine(ResetOvertakingFlag(overtakingForceVector));

                Debug.Log(carManager.CarInfo.Name + "检测到前面有车 :" + hit.collider.GetComponent<CarManager>().CarInfo.Name + ",正在向" + direction + "超车");
            }
        }
    }

    private IEnumerator ResetOvertakingFlag(Vector3 overtakingForceVector)
    {
        // 设置正在超车标志
        isOvertaking = true;

        float time = 0;
        while (time < overtakingDuration)
        {
            // 施加超车推力
            rb.AddForce(overtakingForceVector * overtakingForce, ForceMode.Force);
            time += Time.deltaTime;
            // 等待1秒钟后重置正在超车标志
            yield return null;
        }
        
        isOvertaking = false;
        Debug.Log(carManager.CarInfo.Name + "超车结束");
    }
    #endregion

    #region 车辆碰撞事件处理
    private void InitSmoke()
    {
        _smokes = Smokes.emission;
        //_smokes.enabled = false;
        Smokes.Stop();
    }

    private void OnVehicleCollisionEnter(Collision other)
    {
        collisionNum++;
        UpdateSmoke();
        AddPushForce(other);
    }

    private void AddPushForce(Collision other)
    {
        if (other.gameObject.CompareTag(GlobalConstants.CARTAGNAME))
        {
            FocusCameraOnCar.Raise(GetComponent<CarManager>(), GetComponent<CarManager>().HasFinishedRace());
            // 获取碰撞对象的Rigidbody组件
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();

            // 确保双方都有Rigidbody组件
            if (rb != null && otherRb != null)
            {
                // 计算推力方向
                Vector3 pushDirection = (otherRb.transform.position - rb.transform.position).normalized;

                // 在碰撞方向上施加推力
                rb.AddForce(-pushDirection * pushForce, ForceMode.Impulse);
                otherRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
    private void UpdateSmoke()
    {
        if(collisionNum >= SmokeCollisionNum && !Smokes.isPlaying)
        {
            //Debug.Log("开启烟雾");
            //_smokes.enabled = true;
            Smokes.Play();
        }
        if (Smokes.isPlaying)
        {
            // 烟变大
            SetStartSize(Smokes, Smokes.main.startSize.curveMultiplier + 4);

            // 烟变黑
            Color color = new Color(
                Mathf.Max(50, Smokes.main.startColor.color.r - 10),
                Mathf.Max(50, Smokes.main.startColor.color.g - 10),
                Mathf.Max(50, Smokes.main.startColor.color.b - 10));
            SetStartColor(Smokes, color);
        }
    }
    // 设置粒子系统的起始大小
    private void SetStartSize(ParticleSystem particleSystem, float size)
    {
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startSize = size;
    }

    // 设置粒子系统的起始颜色
    private void SetStartColor(ParticleSystem particleSystem, Color color)
    {
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startColor = color;
    }
    #endregion


    private void OnDrawGizmos()
    {
        // 在Scene视图中绘制射线
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * overtakingDistance);
    }

}

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
    private int collisionNum;
    private Rigidbody rb;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        InitSmoke();
        controller = GetComponent<SolidController>();
        rb = GetComponent<Rigidbody>();
        controller.OnCollisionEnterWithOther += OnVehicleCollisionEnter;
        
    }

    public void InitSmoke()
    {
        _smokes = Smokes.emission;
        _smokes.enabled = false;
    }

    private void OnVehicleCollisionEnter(Collision other)
    {
        collisionNum ++;
        UpdateSmoke();
        AddPushForce(other);
    }
    private void AddPushForce(Collision other)
    {
        if (other.gameObject.CompareTag("Car"))
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
        if(collisionNum >= SmokeCollisionNum && !_smokes.enabled)
        {
            _smokes.enabled = true;
        }
        // 烟变大
        SetStartSize(Smokes, Smokes.main.startSize.curveMultiplier + 1);

        // 烟变黑
        Color color = new Color(
            Smokes.main.startColor.color.r - 10,
            Smokes.main.startColor.color.g - 10,
            Smokes.main.startColor.color.b - 10);
        SetStartColor(Smokes, color);
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
}

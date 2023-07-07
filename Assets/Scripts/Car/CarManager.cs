using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
public class CarManager : MonoBehaviour
{
    public CarInfo CarInfo;
    public CarInfoScriptableObject CarInfoScriptableObject;
    private float oneLapTime;
    private float totalTime;
    private VehicleInformation vehicleInformation;
    private SolidController solidController;

    public float FinalTime;
    private bool IsTimerRunning;
    private void Awake()
    {
        InitData();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        Timer();
    }
    public void InitData()
    {
        FinalTime = 0;
        oneLapTime = 0;
        StartTimer();

        CarInfo = CarInfoScriptableObject.GetCarInfo();
        Car car = new(MyGameManager.instance.FactorsBaseObject, CarInfo);
        // 设置车辆名称信息
        vehicleInformation = GetComponent<VehicleInformation>();
        vehicleInformation.LobbyName = CarInfo.Name;
        // 设置车辆实际数值
        solidController = GetComponent<SolidController>();
        solidController.EngineForce = car.Acceleration;
        solidController.FullThrottleVelocity = car.TopSpeed;
        solidController.CarGrip = car.Handling;
    }
    private void StartTimer() => IsTimerRunning = false;
    private void StopTimer() => IsTimerRunning = true;
    /// <summary>
    /// 计时器
    /// </summary>
    private void Timer()
    {
        if (!IsTimerRunning)
        {
            oneLapTime += Time.deltaTime;
            totalTime += Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 一圈结束
        if (other.CompareTag(GlobalConstants.CHECKPOINT))
        {
            OnOneLapEnd();
        }
        // 出界
        if (other.CompareTag(GlobalConstants.BOUNDARIES))
        {
            solidController.Respawn();
        }
    }
    /// <summary>
    /// 一圈结束事件
    /// </summary>
    private void OnOneLapEnd()
    {
        FinalTime = (Mathf.Round(oneLapTime * 1000) / 1000f);
        oneLapTime = 0;
        StopTimer();
    }
    /// <summary>
    /// 运行完成事件
    /// </summary>
    public void OnEnd()
    {

    }
    /// <summary>
    /// 运行开始事件
    /// </summary>
    public void OnStart()
    {

    }
}

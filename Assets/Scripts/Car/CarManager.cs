using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
public class CarManager : MonoBehaviour
{
    public CarInfo CarInfo
    {
        get
        {
            int index = Mathf.Min(CarInfoScriptableObject.CarInfos.Count - 1, CarInfoIndex);
            return CarInfoScriptableObject.GetCarInfo(index);
        }
    }
    public CarInfoScriptableObject CarInfoScriptableObject;
    private int CarInfoIndex => MyGameManager.instance.CurSeason;
    private float oneLapTime;
    private float totalTime;
    private VehicleInformation vehicleInformation;
    private SolidController solidController
    {
        get
        {
            return GetComponent<SolidController>();
        }
    }

    public float FinalTime;
    private bool IsTimerRunning;
    private bool twoLapFlag = false;
    private Car car
    {
        get 
        {
            return new(MyGameManager.instance.FactorsBaseObject, CarInfo);
        }
    }
    private void Awake()
    {
        //InitData();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        Timer();
    }
    #region Init
    public void InitData()
    {
        //Debug.Log("已重置车辆数据");
        FinalTime = 0;
        oneLapTime = 0;
        StartTimer();

        /*CarInfo = CarInfoScriptableObject.GetCarInfo();
        car = new(MyGameManager.instance.FactorsBaseObject, CarInfo);*/
        // 设置车辆名称信息
        vehicleInformation = GetComponent<VehicleInformation>();
        vehicleInformation.LobbyName = CarInfo.Name;
        InitCarData();
    }

    public void InitCarData()
    {
        // 设置车辆实际数值
        //solidController = GetComponent<SolidController>();
        InitEngineForce();
        InitFullThrottleVelocity();
        InitCarGrip();
    }
    public void InitEngineForce() => solidController.EngineForce = car.Acceleration;
    public void InitFullThrottleVelocity() => solidController.FullThrottleVelocity = car.TopSpeed;
    public void InitCarGrip() => solidController.CarGrip = car.Handling;
    #endregion

    #region Get / Set
    public void SetEngineForce(float engineForce) => solidController.EngineForce = engineForce;
    public void SetFullThrottleVelocity(float fullThrottleVelocity) => solidController.FullThrottleVelocity = fullThrottleVelocity;
    public void SetCarGrip(float carGrip) => solidController.CarGrip = carGrip;
    public Car GetInitCar() => car;
    #endregion
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
            if (!twoLapFlag)
            {
                twoLapFlag = true;
            }
            else
            {
                OnOneLapEnd();
            }
        }
        // 出界
        if (other.CompareTag(GlobalConstants.BOUNDARIES))
        {
            //solidController.Respawn();
            Respawn();
        }
    }
    private void Respawn()
    {
        MapPieceInfo[] mapPieceInfos = FindObjectsOfType<MapPieceInfo>();
        Vector3 respawnPos = new Vector3(0, 0, 0);
        float minDis = float.MaxValue;
        foreach(var i in mapPieceInfos)
        {
            foreach(var j in i.RespawnPoints)
            {
                var tempDis = Vector3.Distance(transform.position, j.position);
                if(tempDis < minDis)
                {
                    minDis = tempDis;
                    respawnPos = j.position;
                }
            }
        }
        transform.position = respawnPos;
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

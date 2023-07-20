using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
public class CarManager : MonoBehaviour
{
    // store renderers
    MeshRenderer[] renderers;
    [SerializeField] private GameObject explosionPrefab;

    internal double airDuration = 0;
    internal bool isRespawning = false;

    public CarInfo CarInfo
    {
        get
        {
            // int index = Mathf.Min(CarInfoScriptableObject.CarInfos.Count - 1, CarInfoIndex);
            int index = (MyGameManager.instance.GameRound % MyGameManager.instance.Season);
            return CarInfoScriptableObject.GetCarInfo(index);
        }
    }
    public CarInfoScriptableObject CarInfoScriptableObject;
    private int CarInfoIndex => MyGameManager.instance.GameRound % MyGameManager.instance.Season;
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
        // store renderers
        renderers = GetComponentsInChildren<MeshRenderer>();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        Timer();
        CheckAirTime();
    }

    private void CheckAirTime()
    {
        // calculate how long car is in air for
        if (solidController.IsGrounded == false)
            airDuration += Time.deltaTime;
        else
            airDuration = 0;

        // focus on this car when it is in the air
       // if (airBorneDuration > minAirTime)
           // MyGameManager.instance.raceCamera.SwitchTarget(CarInfo.Name, false, true);
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
            Respawn(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ExplosionTag"))
        {
            SpawnExplosion();
        }
    }
    internal void Respawn(bool respawnWithExplosion)
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
        if(respawnWithExplosion)
            StartCoroutine(RespawnWithDelay(respawnPos));


    }

    private IEnumerator RespawnWithDelay(Vector3 respawnPos)
    {
        // move camera to another driver after this one explodes
        MyGameManager.instance.raceCamera.StartCoroutine(MyGameManager.instance.raceCamera.SwitchToAnotherDriverAfterDelay(2.25f));

        // spawn explosion particle
        SpawnExplosion();

        // make invisible
        foreach (var ren in renderers)
            ren.enabled = false;

        // wait a bit to set respawn flag (for smoother camera
        yield return new WaitForSeconds(0.5f);
        isRespawning = true;
        airDuration = 0;
        yield return new WaitForSeconds(2f);

        // respawn to position
        transform.position = respawnPos;

        // make visible
        foreach (var ren in renderers)
            ren.enabled = true;

        isRespawning = false;
    }

    internal void SpawnExplosion()
    {
       MyGameManager.instance.raceCamera.TryZoomOnDriver(CarInfo.Name);
        GameObject newExplosion = Instantiate(explosionPrefab);
        newExplosion.transform.position = transform.position;
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

using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using Missions;

public class CarManager : MonoBehaviour
{
    public GameObject cameraAction;
    public MissionEvent FocusCameraOnCar;
    public MissionEvent OnCarExplode;

    // store renderers
    private MeshRenderer[] renderers;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject splashPrefab;
    [SerializeField] private float BoundaryCheckRadio = 5f;
    private GameObject currentExplosion;

    private double minAirDuration = 1;
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

    internal bool HasFinishedRace()
    {
        return solidController.FinalRank > 0 ? true : false;
    }

    public bool IsFinished;
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
    private VehicleAI vehicleAI;
    private void Awake()
    {
        //InitData();
        // store renderers
        renderers = GetComponentsInChildren<MeshRenderer>();
    }
    private void Start()
    {
        RaceCameraManager.SetTarget(cameraAction, transform);
    }
    private void Update()
    {
        Timer();
        CheckAirTime();
        BoundariesHandle();
        MapOutOfBoundsProcessing();
    }

    private void OnEnable()
    {
        vehicleAI = GetComponent<VehicleAI>();
        vehicleAI.StuckEvent.AddListener(OnStuck);
    }
    private void OnDisable()
    {
        if (vehicleAI != null) vehicleAI.StuckEvent.RemoveListener(OnStuck);
    }
    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, BoundaryCheckRadio);*/
    }
    private void CheckAirTime()
    {
        // calculate how long car is in air for
        if (solidController.IsGrounded == false)
            airDuration += Time.deltaTime;
        else
            airDuration = 0;

        if (airDuration > minAirDuration)
            FocusCameraOnCar.Raise(this);

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

    #region Timer
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
    #endregion

    #region CollisionHandle
    private void OnTriggerEnter(Collider other)
    {
        CollisionEnterHandle(other.gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnterHandle(collision.gameObject);
    }
    private void CollisionEnterHandle(GameObject gameObject)
    {
        // 一圈结束
        if (gameObject.CompareTag(GlobalConstants.CHECKPOINT))
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
        if (gameObject.transform.CompareTag("ExplosionTag"))
        {
            SpawnExplosion();
        }
        if (gameObject.transform.CompareTag("ExplosionDeathTag"))
        {
            SpawnExplosion();
            Respawn(false);
        }
        // 出界
        if (gameObject.transform.CompareTag(GlobalConstants.BOUNDARIES))
        {
            //solidController.Respawn();
            Respawn(true);
        }
    }
    /// <summary>
    /// 出界处理
    /// </summary>
    private void BoundariesHandle()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, BoundaryCheckRadio);
        foreach (var i in colliders)
        {
            if (i.CompareTag(GlobalConstants.BOUNDARIES))
            {
                TerrainObject terrainObject = i.GetComponentInParent<TerrainObject>();
                if (terrainObject != null)
                {
                    switch (terrainObject.terrainType)
                    {
                        case TerrainObject.Type.Sea:
                            Instantiate(splashPrefab);
                            break;
                        default:
                            SpawnExplosion();
                            break;
                    }
                    Respawn(false);
                }
            }
        }
    }
    private void MapOutOfBoundsProcessing()
    {
        if (transform.position.y < 24) Respawn(false);
    }
    /// <summary>
    /// 复活在距离目targetPoint最近的复活点
    /// </summary>
    /// <param name="respawnWithExplosion"></param>
    internal void Respawn(bool respawnWithExplosion)
    {
        MapPieceInfo[] mapPieceInfos = FindObjectsOfType<MapPieceInfo>();
        Vector3 respawnPos = new Vector3(0, 0, 0);
        float minDis = float.MaxValue;
        Vector3 targetAIWayPoint = GetComponent<VehicleAI>()._targetWaypoint;
        foreach (var i in mapPieceInfos)
        {
            foreach (var j in i.RespawnPoints)
            {
                var tempDis = Vector3.Distance(targetAIWayPoint, j.position);
                if (tempDis < minDis)
                {
                    minDis = tempDis;
                    respawnPos = j.position;
                }
            }
        }
        if (respawnWithExplosion)
            StartCoroutine(RespawnWithDelay(respawnPos));
        else
            // respawn to position
            //transform.position = respawnPos;
            RespawnDelay(respawnPos);
    }
    private void RespawnDelay(Vector3 respawnPosition, float delayTime = 0f)
    {
        StartCoroutine(RespawnWithDelay(respawnPosition, delayTime));
        IEnumerator RespawnWithDelay(Vector3 respawnPosition, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            transform.position = respawnPosition;
        }
    }

    private IEnumerator RespawnWithDelay(Vector3 respawnPos)
    {
        // move camera to another driver after this one explodes
        //MyGameManager.instance.raceCamera.StartCoroutine(MyGameManager.instance.raceCamera.SwitchToAnotherDriverAfterDelay(2.25f));

        // spawn explosion particle
        SpawnExplosion();

        // make invisible
        foreach (var ren in renderers)
            ren.enabled = false;

        // wait a bit to set respawn flag (for smoother camera
        yield return new WaitForSeconds(0.5f);
        isRespawning = true;
        airDuration = 0;
        OnCarExplode.Raise(this);
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
        if (currentExplosion == null)
        {
            GameObject newExplosion = Instantiate(explosionPrefab);
            newExplosion.transform.position = transform.position;
            currentExplosion = newExplosion;

            SoundManager soundManager = FindObjectOfType<SoundManager>();
            if(soundManager != null)
            {
                soundManager.PlayExplosionSound(transform);
            }
        }
    }

    #endregion

    #region Events
    internal void OnStuck()
    {
        Respawn(true);
    }
    /// <summary>
    /// 一圈结束事件
    /// </summary>
    private void OnOneLapEnd()
    {
        FinalTime = (Mathf.Round(oneLapTime * 1000) / 1000f);
        oneLapTime = 0;
        StopTimer();
        IsFinished = true;
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
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using UnityEngine.SceneManagement;
using System.Linq;
/// <summary>
/// 游戏主流程
/// </summary>
public class MyGameManager : Singleton<MyGameManager>
{
    public Material litMaterial;
    public FactorsBaseObject FactorsBaseObject;
    private RaceManager raceManager;
    public GameObject Map;
    public override void Awake()
    {
        base.Awake();
        raceManager = FindObjectOfType<RaceManager>();
        InitData();
        InitMap();
    }
    // Start is called before the first frame update
    void Start()
    {
        //InitCars();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // 使用索引重新加载当前场景
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
    #region 数据初始化
    /// <summary>
    /// 数据初始化
    /// </summary>
    public void InitData()
    {
        foreach (GameObject i in raceManager.TestBotPlayers)
        {
            i.GetComponent<CarManager>().InitData();
        }
    }
    /// <summary>
    /// 地图数据初始化
    /// </summary>
    public void InitMap()
    {
        // 加载地图
        Map = LoadMap("Map");
        // 设置RaceManager

        // 设置车辆出生点
        LoadStartPoints();

        // 设置车辆初始朝向
        SetStartAngle();

        // 设置CheckPoints
        LoadCheckPoints();

        // 设置AI WayPoint
        LoadAIWayPoints();
    }

    /// <summary>
    /// 设置车辆出生点
    /// </summary>
    private void LoadStartPoints()
    {
        StartPieceInfo startPieceInfo = FindObjectOfType<StartPieceInfo>();
        if (startPieceInfo.StartPoints.Count == 0)
        {
            Debug.LogError("Failed to load map : Non-existent StartPoints");
            return;
        }
        Vector3[] startPositions = startPieceInfo.StartPoints.Select(transform => transform.position).ToArray();
        raceManager.StartPositions = startPositions;
    }

    /// <summary>
    /// 设置CheckPoint
    /// </summary>
    private void LoadCheckPoints()
    {
        StartPieceInfo startPieceInfo = FindObjectOfType<StartPieceInfo>();
        if (startPieceInfo.CheckPoint == null)
        {
            Debug.LogError("Failed to load map : Non-existent CheckPoint");
            return;
        }
        GameObject[] checkPoints = new GameObject[] { startPieceInfo.CheckPoint };
        raceManager.Checkpoints = checkPoints;
    }
    /// <summary>
    /// 加载AIWayPoints
    /// <summary>
    /// <returns></returns>
    private void LoadAIWayPoints()
    {
        StartPieceInfo startPieceInfo = FindObjectOfType<StartPieceInfo>();
        if (startPieceInfo.FirstWayPoint == null)
        {
            Debug.LogError("Failed to load map : Non-existent FirstWayPoint");
            return;
        }

        Vector3 firstPoint = startPieceInfo.FirstWayPoint.position;
        GameObject AIWayPoint = new GameObject("AIWayPoint");
        Waypoints waypoints = AIWayPoint.AddComponent<Waypoints>();

        // 获取全部导航点
        MapPieceWayPoints[] mapPieceWayPoints = FindObjectsOfType<MapPieceWayPoints>();

        List<Vector3> wayPoints = new();
        foreach (var mapPieceWayPoint in mapPieceWayPoints)
        {
            List<Vector3> temp = mapPieceWayPoint.Points.Select(p => p.transform.position).ToList();
            wayPoints.AddRange(temp);
        }
        // 转化成AIWayPoint的相对坐标
        //wayPoints = wayPoints.Select(p => AIWayPoint.transform.InverseTransformPoint(p)).ToList();

        List<Vector3> AIPoints = new List<Vector3>();

        // 添加首个点
        AIPoints.Add(firstPoint);

        while (wayPoints.Count > 0)
        {
            Vector3 lastPoint = AIPoints[AIPoints.Count - 1];
            float minDistance = float.MaxValue;
            Vector3 closestPoint = Vector3.zero;

            // 在wayPoints中找出距离最后一个点最近的点
            foreach (Vector3 point in wayPoints)
            {
                float distance = Vector3.Distance(lastPoint, point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = point;
                }
            }

            // 将最近的点加入AIPoints列表，并从wayPoints中移除
            AIPoints.Add(closestPoint);
            wayPoints.Remove(closestPoint);
        }
        waypoints.items = AIPoints;

        raceManager.AIWaypoints = AIWayPoint;
    }
    /// <summary>
    /// 加载地图
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private GameObject LoadMap(string name)
    {
        GameObject loadedPrefab = Resources.Load<GameObject>("SaveMap/" + name);

        if (loadedPrefab != null)
        {
            // 修复材质缺失问题
            MeshRenderer[] ms = loadedPrefab.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in ms)
            {
                if (m.sharedMaterial == null)
                {
                    m.sharedMaterial = litMaterial;
                }
            }
            Instantiate(loadedPrefab, transform.position, transform.rotation);
            Debug.Log("Prefab loaded: " + name);
            Debug.Log("Map loaded successfully");
        }
        else
        {
            Debug.LogError("Failed to load prefab: " + name);
        }
        return loadedPrefab;
    }
    /// <summary>
    /// 设置复活朝向角度
    /// </summary>
    private void SetStartAngle()
    {
        StartPieceInfo startPieceInfo = FindObjectOfType<StartPieceInfo>();
        if (startPieceInfo.FirstWayPoint == null)
        {
            Debug.LogError("Failed to load map : Non-existent InitialOrientation");
            return;
        }
        Vector3 direction = startPieceInfo.InitialOrientation;
        float angle = Vector3.Angle(Vector3.forward, direction);

        float dot = Vector3.Dot(transform.right, direction);
        float signedAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (signedAngle > 180)
        {
            angle = 360 - angle;
        }

        angle = Mathf.Round(angle / 90f) * 90f;
        raceManager.StartAngleDegree = (int)angle;

        // 输出角度
        Debug.Log("Angle: " + angle);
    }
    #endregion
}
using MoreMountains.HighroadEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
public class State_Race : IGameState
{
    private GameObject raceObjects = null;
    RaceManager raceManager;
    Material litMaterial;
    private GameObject raceCamera;
    public State_Race()
    {
    }

    public void StartState()
    {
        // enable current state objects
        raceObjects = MyGameManager.instance.GetSceneManager().raceObjects;
        if (raceObjects != null)
            raceObjects.SetActive(true);

        // assign refs
        raceManager = MyGameManager.instance.GetRaceManager();
        litMaterial = MyGameManager.instance.litMaterial;

        // init stuff
        
        InitCarData();
        InitMap();
        InitRaceManager();
        InitCamera();

        //raceManager.StartRace();

        // init carInfoSearch
        CarInfoSearch carInfoSerach = MyGameManager.instance.GetCarInfoSerach();
        MyGameManager.instance.SetShadowQuality(1);
        MyGameManager.instance.raceCamera.StoreCarManagers();




        //carInfoSerach.SetupCarManagers();
    }
    public void EndState()
    {
        if (raceObjects != null)
            raceObjects.SetActive(false);
    }
    public void OnAction() { }
    public void UpdateState() { }

    public void InitCamera()
    {
//        if(raceCamera == null)
//        {
//            //raceCamera = GameObject.Instantiate(MyGameManager.instance.RaceCamera);
//           // raceCamera = MyGameManager.instance.RaceCameraObject;
//            // 初始化Follow和Target
//            //Transform car = GameObject.FindObjectOfType<CarManager>().transform;
/////RaceCameraManager.SetTarget(raceCamera, car);
//        }
    }
    public void InitRaceManager()
    {
        raceManager.ResetCurrentFinisherRank();
        raceManager.StartRace();
    }

    public void InitCarData()
    {
        foreach (GameObject i in raceManager.TestBotPlayers)
        {
            i.GetComponent<CarManager>().InitData();
            // 卡住时间为2s
            i.GetComponent<VehicleAI>().TimeBeforeStuck = 2f;
        }
        // 启用所有车之间的碰撞
        InitAllCarsCollision();
    }
    public void InitMap()
    {
        // 设置车辆出生点
        LoadStartPoints();

        // 随机打乱车辆出生位置
        ShuffleArray(raceManager.StartPositions);

        // 设置车辆初始朝向
        SetStartAngle();

        // 设置CheckPoints
        LoadCheckPoints();

        // 设置AI WayPoint
        LoadAIWayPoints();

    }

    public static void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
        //Debug.Log("出生点已被打乱");
    }
    /// <summary>
    /// 启用所有车之间的碰撞
    /// </summary>
    private void InitAllCarsCollision()
    {
        foreach(var i in raceManager.TestBotPlayers)
        {
            foreach (var j in raceManager.TestBotPlayers)
            {
                CollisionController.EnableCollisionBetweenGameObjects(i, j);
            }
        }
    }
    private void LoadStartPoints()
    {
        StartPieceInfo startPieceInfo = MyGameManager.instance.GetStartPieceInfoObject();
        if (startPieceInfo == null)
        {
            Debug.LogError("Failed to load map : Non-existent StartPoints");
            return;
        }
        Vector3[] startPositions = startPieceInfo.StartPoints.Select(transform => transform.position).ToArray();
        raceManager.StartPositions = startPositions;
    }
    private void SetStartAngle()
    {
        StartPieceInfo startPieceInfo = MyGameManager.instance.GetStartPieceInfoObject();
        if (startPieceInfo.FirstWayPoint == null)
        {
            Debug.LogError("Failed to load map : Non-existent InitialOrientation");
            return;
        }
        Vector3 direction = startPieceInfo.InitialOrientation;
        float angle = Vector3.Angle(Vector3.forward, direction);
        if (Vector3.Cross(Vector3.forward, direction).y < 0)
        {
            angle = 360 - angle;
        }
        /*float dot = Vector3.Dot(Vector3.right, direction);
        float signedAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (signedAngle > 180)
        {
            angle = 360 - angle;
        }*/

        angle = Mathf.Round(angle / 90f) * 90f;
        raceManager.StartAngleDegree = (int)angle;

        // 输出角度
        //Debug.Log("Angle: " + angle);
    }

    private void LoadCheckPoints()
    {
        StartPieceInfo startPieceInfo = MyGameManager.instance.GetStartPieceInfoObject();
        if (startPieceInfo.CheckPoint == null)
        {
            Debug.LogError("Failed to load map : Non-existent CheckPoint");
            return;
        }
        GameObject[] checkPoints = new GameObject[] { startPieceInfo.CheckPoint };
        raceManager.Checkpoints = checkPoints;
    }
    private void LoadAIWayPoints()
    {
        StartPieceInfo startPieceInfo = MyGameManager.instance.GetStartPieceInfoObject();
        if (startPieceInfo.FirstWayPoint == null)
        {
            Debug.LogError("Failed to load map : Non-existent FirstWayPoint");
            return;
        }

        Vector3 firstPoint = startPieceInfo.FirstWayPoint.position;
        GameObject AIWayPoint = new GameObject("AIWayPoint");
        Waypoints waypoints = AIWayPoint.AddComponent<Waypoints>();

        // 获取全部导航点
        MapPieceInfo[] mapPieceWayPoints = MyGameManager.instance.GetMapPieceWayPointsObjects();

        Debug.Log("Map Piece Waypoints: " + mapPieceWayPoints.Length);

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
}

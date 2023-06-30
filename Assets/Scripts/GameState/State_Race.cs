using MoreMountains.HighroadEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class State_Race : IGameState
{
    RaceManager raceManager;
    Material litMaterial;

    public State_Race()
    {
        litMaterial = MyGameManager.instance.litMaterial;
    }

    public void StartState()
    {
        raceManager = MyGameManager.instance.GetRaceManager();

        InitData();
        InitMap();
    }
    public void EndState() { }
    public void OnAction() { }
    public void UpdateState() { }



    public void InitData()
    {
        foreach (GameObject i in raceManager.TestBotPlayers)
        {
            i.GetComponent<CarManager>().InitData();
        }
    }
    public void InitMap()
    {
        // 加载地图
        MyGameManager.instance.Map = LoadMap("Map");
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

            MyGameManager.instance.InstantiateMapObject(loadedPrefab);
            Debug.Log("Prefab loaded: " + name);
            Debug.Log("Map loaded successfully");
        }
        else
        {
            Debug.LogError("Failed to load prefab: " + name);
        }
        return loadedPrefab;
    }

    private void LoadStartPoints()
    {
        StartPieceInfo startPieceInfo = MyGameManager.instance.GetStartPieceInfoObject();
        if (startPieceInfo == null  )
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

        float dot = Vector3.Dot(Vector3.right, direction);
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
        MapPieceWayPoints[] mapPieceWayPoints = MyGameManager.instance.GetMapPieceWayPointsObjects();

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

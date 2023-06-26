using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;

/// <summary>
/// 游戏主流程
/// </summary>
public class MyGameManager : Singleton<MyGameManager>
{
    public GameObject CarPrefab;
    public List<CarInfoScriptableObject> CarConfigList;
    private List<Transform> startPoints;
    // 当前车辆名次
    public int Rank = 1;
    // 车辆总数
    public int CarTotalNum;
    private RaceManager raceManager;
    public override void Awake()
    {
        base.Awake();
        raceManager = FindObjectOfType<RaceManager>();
        InitData();
    }
    // Start is called before the first frame update
    void Start()
    {
        //InitCars();
    }
    
    // Update is called once per frame
    void Update()
    {

    }
    public void InitData()
    {
        foreach (GameObject i in raceManager.TestBotPlayers)
        {
            i.GetComponent<CarManager>().InitData();
        }
    }
    /// <summary>
    /// 获取排名
    /// </summary>
    /// <returns></returns>
    public int GetRank(bool update = false)
    {
        int res = Rank;
        if (update)
        {
            Rank++;
            if (Rank > CarTotalNum) Rank = 1;
        }

        return res;
    }
}
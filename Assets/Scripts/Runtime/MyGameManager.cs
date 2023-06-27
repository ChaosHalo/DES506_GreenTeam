using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;

/// <summary>
/// 游戏主流程
/// </summary>
public class MyGameManager : Singleton<MyGameManager>
{
    public FactorsBaseObject FactorsBaseObject;
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
}
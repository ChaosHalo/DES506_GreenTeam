using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using UnityEngine.SceneManagement;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // 使用索引重新加载当前场景
            SceneManager.LoadScene(currentSceneIndex);
        }
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
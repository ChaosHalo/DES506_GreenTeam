using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Lovatto.MiniMap;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
/// <summary>
/// 游戏主流程
/// </summary>
public class MyGameManager : MonoBehaviour
{
    public FactorsBaseObject FactorsBaseObject;
    public GameObject Map;
    public IGameState gameState;
    public Material litMaterial;
    /// <summary>
    /// 表示每多少GameRound轮后更新一次赛季信息
    /// </summary>
    public int Season = 3;
    [HideInInspector]
    /// <summary>
    /// 游戏已运行轮次
    /// </summary>
    public int GameRound;
    /// <summary>
    /// 所有游戏轮次保存的比赛信息
    /// </summary>
    public List<OneRoundRaceResultData> OneRoundRaceResultDatas = new();
    /// <summary>
    /// 当前赛季值
    /// </summary>
    public int CurSeason => GameRound / Season;
    // build / race scene objects
    [SerializeField] internal GameObject buildObjects;
    [SerializeField] internal GameObject raceObjects;

    public Light light;

    // has new scene been loaded for game state?
    bool gamestateNewScene = false;

    public MissionManager missionManager;
    public GameObject RaceCamera;
    // singleton instance
    internal static MyGameManager instance;

    void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
       // Application.targetFrameRate = 60;

        SetupInstance();
        InitManager();
        if (instance != this)
            return;

    }
    void SetupInstance()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        //SetNewState(SceneManager.GetActiveScene().buildIndex, false);
        SetNewState(0, false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState != null)
            gameState.UpdateState();

        if (missionManager != null)
            foreach (var mission in missionManager.missionUI)
                mission.CustomUpdate();

        // commented this out because was preventing build
        //ReLoadScene();
    }

#if UNITY_EDITOR
    void ReLoadScene()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // 使用索引重新加载当前场景
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
#endif
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(gameState!=null && gamestateNewScene == true)
        {
            gamestateNewScene=false;
            gameState.StartState();
        }
    }

    internal void SetNewState(int index, bool loadNewScene)
    {
        gamestateNewScene = true;

        // clear old state
        if(gameState !=null)
        {
            gameState.EndState();
            gameState = null;
        }

        // set new state
        switch (index)
        {
            case 0:
                gameState = new State_CheckRacers();
                break;
            case 1:
                gameState = new State_Build();
                break;
            case 2:
                gameState = new State_Race();
                break;
            case 3:
                gameState = new State_SeasonEnd();
                break;
        }

        // manually start new state if new level has not been loaded (otherwise automatically starts on level load)
        if (loadNewScene == false)
            if (gameState != null)
                gameState.StartState();

        // debug print current state
        Debug.Log("Changing state to: "+gameState);
    }

    public void OnContinuePress()
    {
        gameState.OnAction();
    }

    private void InitManager()
    {
        
    }
    public void SaveOneRoundRaceResultData()
    {
        List<OneCarRaceResultData> tempCarDatas = new();
        BaseController[] baseControllers = FindObjectsOfType<BaseController>();
        foreach(var i in baseControllers)
        {
            tempCarDatas.Add(new OneCarRaceResultData(
                i.GetComponent<CarManager>().CarInfo.Name,
                i.FinalRank,
                i.GetComponent<CarManager>().FinalTime));
        }
        AddRaceResult(new OneRoundRaceResultData(tempCarDatas));
    }
    public void AddRaceResult(OneRoundRaceResultData oneRoundRaceResultData)
    {
        Debug.Log($"添加第{GameRound}轮的比赛结算数据");
        OneRoundRaceResultDatas.Add(oneRoundRaceResultData);
    }
    /// <summary>
    /// 清除所有车辆和车辆图标
    /// </summary>
    public void ClearAllCars()
    {
        CarManager[] CarManager = FindObjectsOfType<CarManager>();
        // 清除小地图UI
        bl_MiniMapIcon[] bl_MiniMapIcons = FindObjectsOfType<bl_MiniMapIcon>();
        foreach (var i in bl_MiniMapIcons)
        {
            //Debug.Log("已清除图标" + i.gameObject.name);
            Destroy(i.gameObject);
        }
        foreach (var Car in CarManager)
        {
            //Debug.Log("已清除车辆" + Car.gameObject.name);
            Destroy(Car.gameObject);
            //Car.gameObject.SetActive(false);
        }
    }

    public void SetShadowQuality(int shadowQuality)
    {
        //light.shadowResolution = (LightShadowResolution)1;

        //float shadowDistance = 500;
        //if (shadowQuality == 1)
        //    shadowDistance = 250;
        //QualitySettings.shadowDistance = shadowDistance;
        //Debug.Log(QualitySettings.shadowDistance);
    }


    public void OnEndSeason(GameObject endSeasonObject)
    {
        endSeasonObject.SetActive(true);
    }
    #region GET / SET
    internal StartPieceInfo GetStartPieceInfoObject()
    {
        try
        {
            return FindObjectOfType<StartPieceInfo>();
        }
        catch
        {
            Debug.LogError("non-startPieceInfo find");
            return null;
        }
    }
    internal CarInfoSearch GetCarInfoSerach() { return FindObjectOfType<CarInfoSearch>(); }
    internal GameObject GetObjectWithTag(string tag) { return GameObject.FindGameObjectWithTag(tag); }
    internal GameStateManager GetSceneManager() { return FindObjectOfType<GameStateManager>(); }
    internal RaceManager GetRaceManager() { return FindObjectOfType<RaceManager>(); }
    internal SaveMap GetSaveMap() { return FindObjectOfType<SaveMap>(); }
    internal ObjectPlacer GetObjectPlacer() { return FindObjectOfType<ObjectPlacer>(); }
    internal CurrencyManager GetCurrencyManager() { return FindObjectOfType<CurrencyManager>(); }
    internal MapPieceInfo[] GetMapPieceWayPointsObjects() { return FindObjectsOfType<MapPieceInfo>(); }
    internal CameraManager GetCameraManager() { return FindObjectOfType<CameraManager>(); }
    internal PlacementSystem GetPlacementSystem() { return FindObjectOfType<PlacementSystem>(); }
    #endregion
}
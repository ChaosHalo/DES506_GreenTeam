using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Lovatto.MiniMap;
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
    /// 赛季轮次 
    /// </summary>
    public int Season = 3;
    /// <summary>
    /// 游戏已运行轮次
    /// </summary>
    public int GameRound;
    /// <summary>
    /// 所有游戏轮次保存的比赛信息
    /// </summary>
    public List<OneRoundRaceResultData> OneRoundRaceResultDatas;
    // build / race scene objects
    [SerializeField] internal GameObject buildObjects;
    [SerializeField] internal GameObject raceObjects;

    // has new scene been loaded for game state?
    bool gamestateNewScene = false;

    public MissionManager missionManager;
    public CarInfoSerach CarInfoSerach;
    // singleton instance
    internal static MyGameManager instance;

    void Awake()
    {
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
        OneRoundRaceResultDatas = new();
        if (FindObjectsOfType<CarInfoSerach>().Length == 0)
        {
            Instantiate(CarInfoSerach);
        }
    }
    public void AddRaceResult(OneRoundRaceResultData oneRoundRaceResultData)
    {
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
            Debug.Log("已清除图标" + i.gameObject.name);
            Destroy(i.gameObject);
        }
        foreach (var Car in CarManager)
        {
            Debug.Log("已清除车辆" + Car.gameObject.name);
            Destroy(Car.gameObject);
        }
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
    internal CarInfoSerach GetCarInfoSerach() { return FindObjectOfType<CarInfoSerach>(); }
    internal GameObject GetObjectWithTag(string tag) { return GameObject.FindGameObjectWithTag(tag); }
    internal GameStateManager GetSceneManager() { return FindObjectOfType<GameStateManager>(); }
    internal RaceManager GetRaceManager() { return FindObjectOfType<RaceManager>(); }
    internal SaveMap GetSaveMap() { return FindObjectOfType<SaveMap>(); }
    internal ObjectPlacer GetObjectPlacer() { return FindObjectOfType<ObjectPlacer>(); }
    internal CurrencyManager GetCurrencyManager() { return FindObjectOfType<CurrencyManager>(); }
    internal MapPieceInfo[] GetMapPieceWayPointsObjects() { return FindObjectsOfType<MapPieceInfo>(); }
    internal CameraManager GetCameraManager() { return FindObjectOfType<CameraManager>(); }

    #endregion
}
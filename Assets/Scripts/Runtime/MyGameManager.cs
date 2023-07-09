using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
/// <summary>
/// 游戏主流程
/// </summary>
public class MyGameManager : MonoBehaviour
{
    public FactorsBaseObject FactorsBaseObject;
    public GameObject Map;
    public IGameState gameState;
    public Material litMaterial;

    // build / race scene objects
    [SerializeField] internal GameObject buildObjects;
    [SerializeField] internal GameObject raceObjects;

    // has new scene been loaded for game state?
    bool gamestateNewScene = false;

    // singleton instance
    internal static MyGameManager instance;

    void Awake()
    {
        SetupInstance();

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
        SetNewState(SceneManager.GetActiveScene().buildIndex, false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState != null)
            gameState.UpdateState();

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
    internal CustomSceneManager GetSceneManager() { return FindObjectOfType<CustomSceneManager>(); }
    internal RaceManager GetRaceManager() { return FindObjectOfType<RaceManager>(); }
    internal SaveMap GetSaveMap() { return FindObjectOfType<SaveMap>(); }
    internal ObjectPlacer GetObjectPlacer() { return FindObjectOfType<ObjectPlacer>(); }
    internal MissionManager GetMissionManager() { return FindObjectOfType<MissionManager>(); }
    internal CurrencyManager GetCurrencyManager() { return FindObjectOfType<CurrencyManager>(); }
    internal MapPieceInfo[] GetMapPieceWayPointsObjects() { return FindObjectsOfType<MapPieceInfo>(); }
    internal CameraManager GetCameraManager() { return FindObjectOfType<CameraManager>(); }

    #endregion
}
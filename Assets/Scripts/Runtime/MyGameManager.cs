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
        SetNewState(SceneManager.GetActiveScene().buildIndex);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState != null)
            gameState.UpdateState();

        if (Input.GetKeyDown(KeyCode.P))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // 使用索引重新加载当前场景
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(gameState!=null)
            gameState.StartState();
    }

    internal void InstantiateMapObject(GameObject loadedPrefab)
    {
        Instantiate(loadedPrefab);
    }

    internal StartPieceInfo GetStartPieceInfoObject()
    {
        return FindObjectOfType<StartPieceInfo>();
    }
    internal CustomSceneManager GetSceneManager()
    {
        return FindObjectOfType<CustomSceneManager>();
    }
    internal RaceManager GetRaceManager()
    {
        return FindObjectOfType<RaceManager>();
    }

    internal MapPieceWayPoints[] GetMapPieceWayPointsObjects()
    {
       return FindObjectsOfType<MapPieceWayPoints>();
    }

    internal void SetNewState(int index)
    {
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
                gameState = new State_Splash();
                break;
            case 1:
                gameState = new State_CheckRacers();
                break;
            case 2:
                gameState = new State_Build();
                break;
            case 3:
                gameState = new State_Race();
                break;
            case 4:
                gameState = new State_SeasonEnd();
                break;
        }

        Debug.Log("Changing state to: "+gameState);
    }

    public void OnContinuePress()
    {
        gameState.OnAction();
    }
}
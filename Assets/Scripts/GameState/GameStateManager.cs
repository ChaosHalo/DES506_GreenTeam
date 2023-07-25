using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] internal GameObject buildObjects;
    [SerializeField] internal GameObject raceObjects;
    public GameObject trackBuilderUI;

    public enum Index { CHECK_RACERS = 0, BUILD = 1, RACE = 2 };
    public Index sceneIndex;
    
    private void Start()
    {
        ChangeToState(Index.BUILD);
    }
    
    public void LoadNewScene(Index index)
    {
        SceneManager.LoadScene((int)index);
        MyGameManager.instance.SetNewState((int)index, true);
    }

    private void ChangeToState(Index index)
    {
        MyGameManager.instance.SetNewState((int)index, false);
    }

    public bool ChangeToState_Race()
    {
        MyGameManager.instance.GetPlacementSystem().EndCurrentState();
        ChangeToState(Index.RACE);
        return true;
    }
    public void ChangeToState_Build()
    {
        trackBuilderUI.SetActive(true);
        ChangeToState(Index.BUILD);
    }
    public void ChangeToState_CheckRacers()
    {
        ChangeToState(Index.CHECK_RACERS);
    }
}

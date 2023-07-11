using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] internal GameObject buildObjects;
    [SerializeField] internal GameObject raceObjects;
    public GameObject trackBuilderUI;

    public enum Index { CHECK_RACERS = 0, BUILD = 1, RACE = 2 };
    public Index sceneIndex;

    public void LoadNewScene(Index index)
    {
        SceneManager.LoadScene((int)index);
        MyGameManager.instance.SetNewState((int)index, true);
    }

    public void ChangeToState(Index index)
    {
        if(index ==Index.BUILD)
            trackBuilderUI.SetActive(true);
        MyGameManager.instance.SetNewState((int)index, false);
    }

    public void LoadNewScene_Build() => LoadNewScene(Index.BUILD);
    public void LoadNewScene_CheckRacers() => LoadNewScene(Index.CHECK_RACERS);
    public bool ChangeToState_Race()
    {
        if (MyGameManager.instance.GetObjectPlacer().IsTrackFullyConnected())
        {
            Debug.Log("Track fully connected");
            ChangeToState(Index.RACE);
            return true;
        }
        else
        {
            Debug.Log("Track not connected");
            return false;
        }
    }
    public void ChangeToState_Build()
    {
        ChangeToState(Index.BUILD);
    }
}

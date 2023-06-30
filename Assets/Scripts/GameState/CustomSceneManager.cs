using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] internal GameObject buildObjects;
    [SerializeField] internal GameObject raceObjects;
    [SerializeField] private float splashDuration = 1.5f;

     public enum Index { CHECK_RACERS = 0, BUILD = 1, RACE = 2};
 public Index sceneIndex;

    public void LoadNewScene(Index index)
    {
        SceneManager.LoadScene((int)index);
        MyGameManager.instance.SetNewState((int)index, true);
    }

    public void ChangeToState(Index index)
    {
        MyGameManager.instance.SetNewState((int)index, false);
    }

    public void LoadNewScene_Build() => LoadNewScene(Index.BUILD);
    public void LoadNewScene_CheckRacers() => LoadNewScene(Index.CHECK_RACERS);
    public void ChangeToState_Race() => ChangeToState(Index.RACE);
    public void ChangeToState_Build() => ChangeToState(Index.BUILD);
}

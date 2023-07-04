using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class State_Splash : IGameState
{
    public State_Splash()
    {
        StartState();
    }

    public void StartState() 
    {
        //MyGameManager.instance.GetSceneManager().StartCoroutine(MyGameManager.instance.GetSceneManager().RunSplashScreen());
    }
    public void EndState() { }
    public void OnAction() { }
    public void UpdateState() { }
}

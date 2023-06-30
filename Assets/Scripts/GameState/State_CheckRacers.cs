using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_CheckRacers : IGameState
{
    public State_CheckRacers()
    {
    }

    public void StartState() { }
    public void EndState() { }
    public void OnAction() 
    {
        MyGameManager.instance.GetSceneManager().LoadNewScene(CustomSceneManager.Index.BUILD);
    }
    public void UpdateState() { }
}

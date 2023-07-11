using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_CheckRacers : IGameState
{
    public State_CheckRacers()
    {
    }

    public void StartState()
    {
        MyGameManager.instance.missionManager.InitialiseMissions();
    }
    public void EndState() { }
    public void OnAction() 
    {
        MyGameManager.instance.GetSceneManager().LoadNewScene(GameStateManager.Index.BUILD);
    }
    public void UpdateState() { }
}

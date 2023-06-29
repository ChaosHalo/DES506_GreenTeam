using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Build : IGameState
{
    public State_Build()
    {
    }

    public void StartState() { }
    public void EndState() { }
    public void OnAction()
    {
        MyGameManager.instance.GetSceneManager().LoadNewScene(3);
    }
    public void UpdateState() { }
}

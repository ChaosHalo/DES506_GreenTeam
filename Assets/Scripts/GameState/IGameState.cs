using UnityEngine;

public interface IGameState
{
    void StartState();
    void EndState();
    void OnAction();
    void UpdateState();
}

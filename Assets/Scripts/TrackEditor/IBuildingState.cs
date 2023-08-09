using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int gridPosition, bool isWithinBounds);
    void UpdateState(Vector3 gridPosition, Vector3 mousePosition, bool isWithinBounds);
}
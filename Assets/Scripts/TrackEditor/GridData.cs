using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    public Vector2Int gridSize;
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public GridData(Vector2Int gridSize)
    {
        this.gridSize = gridSize;
    }

    public void AddObjectAt(Vector3Int gridPosition,
                          Vector2Int objectSize,
                          int ID,
                          int type,
                          int placedObjectIndex,
                          int rotationState,
                          bool canModify,
                          int cost,
                          bool isBuildable)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, rotationState);
        PlacementData data = new PlacementData(positionToOccupy, gridPosition, ID, type, placedObjectIndex, rotationState, objectSize, canModify, cost, isBuildable);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell positiojn {pos}");
            placedObjects[pos] = data;
        }
    }

    internal void ClearData()
    {
        placedObjects.Clear();
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize, int rotationState)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                int newX = 0;
                int newY = 0;

                switch (rotationState)
                {
                    case 0:
                        newX = -x;
                        newY = -y;
                        break;
                    case 1:
                        newX = -y;
                        newY = x;
                        break;
                    case 2:
                        newX = x;
                        newY = y;
                        break;
                    case 3:
                        newX = y;
                        newY = -x;
                        break;
                }
                returnVal.Add(gridPosition + new Vector3Int(newX, 0, newY));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObejctAt(Vector3Int gridPosition, Vector2Int objectSize, int rotationState)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, rotationState);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;

            // is within grid?
            if (pos.x < -gridSize.x / 2)
                return false;
            if (pos.x+1 > gridSize.x / 2)
                return false;
            if (pos.z < -gridSize.y / 2)
                return false;
            if (pos.z > gridSize.y / 2)
                return false;
        }
        return true;
    }

    public bool IsBuildableAt(Vector3Int gridPosition, Vector2Int objectSize, int rotationState)
    {
        bool isBuildable = true;

        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, rotationState);
        foreach (var pos in positionToOccupy)
        {
            PlacementData data = GetObjectDataAt(pos);
            if(data!= null)
                if(data.isBuildable==false)
                    isBuildable = false;
        }
        return isBuildable;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;

        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }

    internal PlacementData GetObjectDataAt(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return null;

        return placedObjects[gridPosition];
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public Vector3Int originPosition;
    public int ID { get; private set; }
    public int objectType { get; private set; }
    public int PlacedObjectIndex { get; private set; }
    public int RotationState { get; private set; }
    public Vector2Int Size { get; private set; }
    public bool canModify { get; private set; }
    public int cost { get; private set; }
    public bool isBuildable { get; private set; }


    public PlacementData(List<Vector3Int> occupiedPositions,
                         Vector3Int originPosition,
                         int iD,
                         int type,
                         int placedObjectIndex,
                         int rotationState,
                         Vector2Int size,
                         bool canModify,
                         int cost,
                         bool isBuildable)
    {
        this.occupiedPositions = occupiedPositions;
        this.originPosition = originPosition;
        ID = iD;
        objectType = type;
        PlacedObjectIndex = placedObjectIndex;
        RotationState = rotationState;
        Size = size;
        this.canModify = canModify;
        this.cost = cost;
        this.isBuildable = isBuildable;
    }


}
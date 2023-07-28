using MoreMountains.HighroadEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class State_PlaceTerrain : IBuildingState
{
    private bool spawnObjectOnce = true;
    private int gameObjectIndex = -1;
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    ObjectsDatabaseSO database;
    GridData terrainData;
    GridData trackData;
    ObjectPlacer objectPlacer;
    PreviewSystem previewSystem;
    PlacementSystem placementSystem;
    CurrencyManager currencyManager;


    public State_PlaceTerrain(int iD,
                          Grid grid,
                          ObjectsDatabaseSO database,
                          GridData terrainData,
                          GridData trackData,
                          ObjectPlacer objectPlacer,
                          PreviewSystem previewSystem,
                          PlacementSystem placementSystem,
                          CurrencyManager currencyManager)
    {
        ID = iD;
        this.grid = grid;
        this.database = database;
        this.terrainData = terrainData;
        this.trackData = trackData;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;
        this.placementSystem = placementSystem;
        this.currencyManager = currencyManager;

        previewSystem.StopShowingPreview();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        // EnablePreview();
    }

    private void EnablePreview()
    {
        if (selectedObjectIndex > -1)
        {
            // set new preview indicator
            previewSystem.SetNewPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size, placementSystem);
        }
        else
        {
            throw new System.Exception($"No object with ID {ID}");
        }
    }

    public void EndState()
    {
        // clear current indicator object
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition, bool isWithinBounds)
    {
        // don't allow placement inside UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            placementSystem.EndCurrentState();
            return;
        }

        // check if existing terrain is of same type, do NOT place if TRUE
        if (terrainData.IsSameType(gridPosition, (int)database.objectsData[selectedObjectIndex].terrainType))
        {
            placementSystem.EndCurrentState();
            return;
        }

        // check placement validity
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false || isWithinBounds == false)
        {
            placementSystem.EndCurrentState();
            return;
        }

        // check currency
        int cost = database.objectsData[selectedObjectIndex].cost;
        if (cost > 0)
        {
            if (currencyManager.MakePurchase(database.objectsData[selectedObjectIndex].cost) == false)
            {
                placementSystem.EndCurrentState();
                return;
            }
        }

        // handle action
        OnActionHandle(gridPosition);
    }

    private void OnActionHandle(Vector3Int gridPosition)
    {
        // chose data type
        GridData selectedData = terrainData;

        // check for existing data
        PlacementData existingData = selectedData.GetObjectDataAt(gridPosition);

        // get index at selected position
        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

        // object to place index
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), 0, true, ObjectData.ObjectType.Terrain, database.objectsData[ID].trackType, database.objectsData[ID].terrainType, true);

        // remove existing object from database
        if (existingData != null)
        {
            selectedData.RemoveObjectAt(gridPosition);
        }

        // remove existing object from world
        if (gameObjectIndex != -1)
        {
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        // add new data
        selectedData.AddObjectAt(gridPosition,
                                 database.objectsData[selectedObjectIndex].Size,
                                 database.objectsData[selectedObjectIndex].ID,
                                 ((int)database.objectsData[selectedObjectIndex].objectType),
                                 index,
                                 0,
                                 true,
                                 database.objectsData[selectedObjectIndex].cost,
                                 database.objectsData[selectedObjectIndex].isBuildable,
                                 (int)database.objectsData[selectedObjectIndex].terrainType);

        previewSystem.UpdatePreview(grid.CellToWorld(gridPosition), false);
        placementSystem.EndCurrentState();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        if (terrainData.IsBuildableAt(gridPosition, database.objectsData[selectedObjectIndex].Size, 0) == false)
            return false;

        if (terrainData.IsSameType(gridPosition, (int)database.objectsData[selectedObjectIndex].terrainType))
            return false;

        return true;
    }

    public void UpdateState(Vector3 position, bool isWithinBounds)
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (spawnObjectOnce == true)
            {
                spawnObjectOnce = false;
                EnablePreview();
            }
        }

        bool placementValidity = false;
        if (isWithinBounds && currencyManager.CanAfford(database.objectsData[selectedObjectIndex].cost))
        {
            placementValidity = CheckPlacementValidity(grid.WorldToCell(position), selectedObjectIndex);
        }
        previewSystem.UpdatePreview(position, placementValidity);
    }
}

using MoreMountains.HighroadEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_PlaceTerrain : IBuildingState
{
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
        // check placement validity
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false || isWithinBounds == false)
        {
            placementSystem.EndCurrentState();
            return;
        }

        // check currency
        if (currencyManager.MakePurchase(database.objectsData[selectedObjectIndex].cost) == false)
        {
            placementSystem.EndCurrentState();
            return;
        }

        // object to place index
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), 0, true, ObjectData.ObjectType.Terrain, database.objectsData[ID].trackType, database.objectsData[ID].terrainType, true);

        // chose data type
        GridData selectedData = terrainData;

        // check for existing data
        PlacementData existingData = selectedData.GetObjectDataAt(gridPosition);

        // get index at selected position
        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

        // remove existing object from database
        if (existingData != null)
        {
            selectedData.RemoveObjectAt(gridPosition);
        }

        // remove existing object from world
        if(gameObjectIndex != -1)
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
                                 database.objectsData[selectedObjectIndex].isBuildable);

        previewSystem.UpdatePreview(grid.CellToWorld(gridPosition), false);
        placementSystem.EndCurrentState();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        if (terrainData.IsBuildableAt(gridPosition, database.objectsData[selectedObjectIndex].Size, 0) == false)
            return false;

        return true;
    }

    public void UpdateState(Vector3 position, bool isWithinBounds)
    {
        bool placementValidity = false;
        if (isWithinBounds && currencyManager.CanAfford(database.objectsData[selectedObjectIndex].cost))
        {
            placementValidity = CheckPlacementValidity(grid.WorldToCell(position), selectedObjectIndex);
        }
        previewSystem.UpdatePreview(position, placementValidity);
    }
}

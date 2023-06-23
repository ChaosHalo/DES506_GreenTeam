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
        if (isWithinBounds == false)
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
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), Quaternion.identity, true);

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
                                 database.objectsData[selectedObjectIndex].cost);

        previewSystem.UpdatePreview(grid.CellToWorld(gridPosition), false);
        placementSystem.EndCurrentState();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].objectType == ObjectData.Type.Terrain ? terrainData : trackData;
        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size, 0);
    }

    public void UpdateState(Vector3 position, bool isWithinBounds)
    {
        if (currencyManager.CanAfford(database.objectsData[selectedObjectIndex].cost))
            previewSystem.UpdatePreview(position, isWithinBounds);
    }
}

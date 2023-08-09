using MoreMountains.HighroadEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class State_PlaceTrack : IBuildingState
{
    private bool spawnObjectOnce = true;
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


    public State_PlaceTrack(int iD,
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
        //EnablePreview();
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
        if (MyGameManager.instance.GetInputManager().IsPointerOverUI == true)
        {
            placementSystem.EndCurrentState();
            return;
        }

        // check validity
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

        // handle action
        OnActionHandle(gridPosition);
    }

    private void OnActionHandle(Vector3Int gridPosition)
    {
        // place object
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab,
                                             grid.CellToWorld(gridPosition),
                                             previewSystem.GetCurrentRotationState(),
                                             true,
                                             ObjectData.ObjectType.Track,
                                             database.objectsData[ID].trackType,
                                             database.objectsData[ID].terrainType,
                                             true,
                                             database,
                                             ID);

        GridData selectedData = database.objectsData[selectedObjectIndex].objectType == ObjectData.ObjectType.Terrain ? terrainData : trackData;

        selectedData.AddObjectAt(gridPosition,
                                 database.objectsData[selectedObjectIndex].Size,
                                 database.objectsData[selectedObjectIndex].ID,
                                 ((int)database.objectsData[selectedObjectIndex].objectType),
                                 index,
                                 GetCurrentPreviewRotationState(),
                                 true,
                                 database.objectsData[selectedObjectIndex].cost,
                                 database.objectsData[selectedObjectIndex].isBuildable,
                                 (int)database.objectsData[selectedObjectIndex].terrainType);

        previewSystem.UpdatePreview(grid.CellToWorld(gridPosition), grid.CellToWorld(gridPosition), false);
        placementSystem.EndCurrentState();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].objectType == ObjectData.ObjectType.Terrain ? terrainData : trackData;

        if (terrainData.IsBuildableAt(gridPosition, database.objectsData[selectedObjectIndex].Size, GetCurrentPreviewRotationState()) == false)
            return false;

        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size, GetCurrentPreviewRotationState());
    }

    private int GetCurrentPreviewRotationState()
    {
        int rotationState = 0;
        if (previewSystem.previewObjectRotation)
            rotationState = previewSystem.previewObjectRotation.GetRotationState();
        return rotationState;
    }

    public void UpdateState(Vector3 position, Vector3 indicatorPosition, bool isWithinBounds)
    {
        if (MyGameManager.instance.GetInputManager().IsPointerOverUI == false)
        {
            if (spawnObjectOnce == true)
            {
                spawnObjectOnce = false;
                EnablePreview();
            }
        }

        bool placementValidity = false;
        if(isWithinBounds && currencyManager.CanAfford(database.objectsData[selectedObjectIndex].cost))
        {
            Vector3 tempIndicatorPos = indicatorPosition;
            tempIndicatorPos.x -= 50;
            tempIndicatorPos.z -= 50;
            tempIndicatorPos.y += 12.5f;
            placementValidity = CheckPlacementValidity(grid.WorldToCell(tempIndicatorPos), selectedObjectIndex);
        }
        previewSystem.UpdatePreview(position, indicatorPosition, placementValidity);
    }
}

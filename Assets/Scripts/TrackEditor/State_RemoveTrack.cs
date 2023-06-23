using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_RemoveTrack : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    GridData terrainData;
    GridData trackData;
    ObjectPlacer objectPlacer;
    PreviewSystem previewSystem;
    PlacementSystem placementSystem;
    UIManager uiManager;
    CurrencyManager currencyManager;

    public State_RemoveTrack(Grid grid,
                         GridData terrainData,
                         GridData trackData,
                         ObjectPlacer objectPlacer,
                         PreviewSystem previewSystem,
                         UIManager uiManager,
                         PlacementSystem placementSystem,
                         CurrencyManager currencyManager)
    {
        this.grid = grid;
        this.terrainData = terrainData;
        this.trackData = trackData;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;
        this.uiManager = uiManager;
        this.placementSystem = placementSystem;
        this.currencyManager = currencyManager;

        previewSystem.StartShowingRemovePreview();
        uiManager.toggleButton_Remove.ToggleButtons();

        // immediately cancel remove state if no objects are avaiable to remove
        if (objectPlacer.AreObjectsAvailable() == false)
        {
            placementSystem.EndCurrentState();
            uiManager.toggleButton_Remove.ResetButtons();
        }

    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
        uiManager.toggleButton_Remove.ResetButtons();
    }

    public void OnAction(Vector3Int gridPosition, bool isWithinBounds)
    {
        GridData selectedData = null;

        if(trackData.CanPlaceObejctAt(gridPosition, Vector2Int.one, 0) == false)
        {
            selectedData = trackData;
        }

        if (selectedData == null)
        {
            // play sound here
            return;
        }

        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

        if (gameObjectIndex == -1)
            return;

        // only allow actions for modifyable data
        PlacementData existingData = selectedData.GetObjectDataAt(gridPosition);
        if (existingData != null)
            if (existingData.canModify == false)
                return;


        // remove object
        selectedData.RemoveObjectAt(gridPosition);
        objectPlacer.RemoveObjectAt(gameObjectIndex);

        // refund currency
        currencyManager.RefundCost(existingData.cost);

        // update preview
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePreview(cellPosition, CheckIfSelectionIsValid(gridPosition));

        // cancel remove state if no objects are left to remove
        if (objectPlacer.AreObjectsAvailable() == false)
        {
            placementSystem.EndCurrentState();
            uiManager.toggleButton_Remove.ResetButtons();
        }
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(trackData.CanPlaceObejctAt(gridPosition, Vector2Int.one, 0) && terrainData.CanPlaceObejctAt(gridPosition, Vector2Int.one, 0));
    }

    public void UpdateState(Vector3 position, bool isWithinBounds)
    {
        bool validity = CheckIfSelectionIsValid(grid.WorldToCell(position));
        previewSystem.UpdatePreview(position, validity);
    }
}

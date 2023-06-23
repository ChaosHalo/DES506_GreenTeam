using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_RotateTrack : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    ObjectsDatabaseSO database;
    GridData terrainData;
    GridData trackData;
    ObjectPlacer objectPlacer;
    PreviewSystem previewSystem;
    UIManager uiManager;
    PlacementSystem placementSystem;

    public State_RotateTrack(Grid grid,
                         ObjectsDatabaseSO database,
                         GridData terrainData,
                         GridData trackData,
                         ObjectPlacer objectPlacer,
                         PreviewSystem previewSystem,
                         UIManager uIManager,
                         PlacementSystem placementSystem)
    {
        this.grid = grid;
        this.database = database;
        this.terrainData = terrainData;
        this.trackData = trackData;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;
        this.uiManager = uIManager;
        this.placementSystem = placementSystem;

        previewSystem.StartShowingRemovePreview();
        uiManager.toggleButton_Rotate.ToggleButtons();

        // immediately cancel remove state if no objects are avaiable to remove
        if (objectPlacer.AreObjectsAvailable() == false)
        {
            placementSystem.EndCurrentState();
            uiManager.toggleButton_Rotate.ResetButtons();
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
        uiManager.toggleButton_Rotate.ResetButtons();
    }

    public void OnAction(Vector3Int gridPosition, bool isWithinBounds)
    {
        GridData selectedData = null;

        if (trackData.CanPlaceObejctAt(gridPosition, Vector2Int.one, 0) == false)
        {
            selectedData = trackData;
        }

        if (selectedData == null)
        {
            // play sound here
            return;
        }

        // get index at selected position
        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

        // make sure index is valid
        if (gameObjectIndex == -1)
            return;

        // only allow actions for modifyable data
        PlacementData existingData = selectedData.GetObjectDataAt(gridPosition);
        if (existingData != null)
            if (existingData.canModify == false)
                return;

        // get new rotation state
        int newRotationState = objectPlacer.RotateObjectAt(gameObjectIndex);

        // make sure rotation state is valid
        if (newRotationState != -1)
        {

            // remove existing from database
            selectedData.RemoveObjectAt(gridPosition);

            // re-add rotated
            selectedData.AddObjectAt(gridPosition,
                                     existingData.Size,
                                     existingData.ID,
                                     existingData.objectType,
                                     gameObjectIndex,
                                     newRotationState,
                                     true,
                                     existingData.cost);
        }

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePreview(cellPosition, CheckIfSelectionIsValid(gridPosition));
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

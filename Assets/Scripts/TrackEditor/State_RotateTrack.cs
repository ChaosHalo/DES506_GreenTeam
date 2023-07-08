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
    CameraManager cameraManager;

    public State_RotateTrack(Grid grid,
                         ObjectsDatabaseSO database,
                         GridData terrainData,
                         GridData trackData,
                         ObjectPlacer objectPlacer,
                         PreviewSystem previewSystem,
                         UIManager uIManager,
                         PlacementSystem placementSystem,
                         CameraManager cameraManager)
    {
        this.grid = grid;
        this.database = database;
        this.terrainData = terrainData;
        this.trackData = trackData;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;
        this.uiManager = uIManager;
        this.placementSystem = placementSystem;
        this.cameraManager = cameraManager;

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
        // don't allow action if camera is panning
        if (cameraManager.isPanning == true)
            return;

        // get index at selected position
        gameObjectIndex = trackData.GetRepresentationIndex(gridPosition);

        // make sure index is valid
        if (gameObjectIndex == -1)
            return;

        // only allow actions for modifyable data
        PlacementData existingData = trackData.GetObjectDataAt(gridPosition);
        if (existingData != null)
            if (existingData.canModify == false)
                return;

        // remove existing from database
        trackData.RemoveObjectAt(gridPosition);

        // make list of valid rotation states
        List<int> validRotationStates = new();
        int originalState = existingData.RotationState;
        int tempState = originalState;
        int finalState = originalState;
        validRotationStates.Add(originalState);
        for (int i = 0; i < 3; i++)
        {
            tempState++;
            if (tempState > 3)
                tempState = 0;

            if (CheckPlacementValidity(existingData.originPosition, tempState, existingData.Size) == true)
                validRotationStates.Add(tempState);
        }

        // other valid rotation states exist, pick next one in line
        if (validRotationStates.Count > 1)
            finalState = validRotationStates[1];

        // rotate object in world
        objectPlacer.RotateObjectToState(gameObjectIndex, finalState);

        // re-add rotated
        trackData.AddObjectAt(existingData.originPosition,
                                 existingData.Size,
                                 existingData.ID,
                                 existingData.objectType,
                                 gameObjectIndex,
                                 finalState,
                                 true,
                                 existingData.cost);


        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePreview(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int newRotationState, Vector2Int size)
    {
        return trackData.CanPlaceObejctAt(gridPosition, size, newRotationState);
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

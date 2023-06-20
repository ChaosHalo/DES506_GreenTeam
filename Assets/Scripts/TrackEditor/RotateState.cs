using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateState : IBuildingState
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

    public RotateState(Grid grid,
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

        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

        if (gameObjectIndex == -1)
            return;

        int newRotationState = objectPlacer.RotateObjectAt(gameObjectIndex);
        if (newRotationState != -1)
        {
            PlacementData existingData = selectedData.GetObjectDataAt(gridPosition);
            selectedData.RemoveObjectAt(gridPosition);
            selectedData.AddObjectAt(gridPosition,
                                     existingData.Size,
                                     existingData.ID,
                                     existingData.objectType,
                                     gameObjectIndex,
                                     newRotationState);
        }
        //{
        //    selectedData.RemoveObjectAt(gridPosition);
        //    selectedData.AddObjectAt(gridPosition,
        //                         database.objectsData[gameObjectIndex].Size,
        //                         database.objectsData[gameObjectIndex].ID,
        //                         ((int)database.objectsData[gameObjectIndex].objectType),
        //                         gameObjectIndex,
        //                         newRotationState);
        //}

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

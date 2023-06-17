using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    GridData terrainData;
    GridData trackData;
    ObjectPlacer objectPlacer;
    PreviewSystem previewSystem;

    public RemovingState(Grid grid,
                         GridData terrainData,
                         GridData trackData,
                         ObjectPlacer objectPlacer,
                         PreviewSystem previewSystem)
    {
        this.grid = grid;
        this.terrainData = terrainData;
        this.trackData = trackData;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;

        if(trackData.CanPlaceObejctAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = trackData;
        }
        //else if(terrainData.CanPlaceObejctAt(gridPosition, Vector2Int.one) == false)
        //{
        //    selectedData = terrainData;
        //}

        if (selectedData == null)
        {
            // play sound here
            return;
        }

        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

        if (gameObjectIndex == -1)
            return;

        selectedData.RemoveObjectAt(gridPosition);
        objectPlacer.RemoveObjectAt(gameObjectIndex);

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePreview(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(trackData.CanPlaceObejctAt(gridPosition, Vector2Int.one) && terrainData.CanPlaceObejctAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3 position, bool isWithinBounds)
    {
        bool validity = CheckIfSelectionIsValid(grid.WorldToCell(position));
        previewSystem.UpdatePreview(position, validity);
    }
}

using MoreMountains.HighroadEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    ObjectsDatabaseSO database;
    GridData terrainData;
    GridData trackData;
    ObjectPlacer objectPlacer;
    PreviewSystem previewSystem;
    PlacementSystem placementSystem;


    public PlaceState(int iD,
                          Grid grid,
                          ObjectsDatabaseSO database,
                          GridData terrainData,
                          GridData trackData,
                          ObjectPlacer objectPlacer,
                          PreviewSystem previewSystem,
                          PlacementSystem placementSystem)
    {
        ID = iD;
        this.grid = grid;
        this.database = database;
        this.terrainData = terrainData;
        this.trackData = trackData;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;
        this.placementSystem = placementSystem;

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
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (placementValidity == false || isWithinBounds == false)
        {
            placementSystem.EndCurrentState();
            return;
        }

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), previewSystem.GetCurrentRotation());


        GridData selectedData = database.objectsData[selectedObjectIndex].objectType == ObjectData.Type.Terrain ? terrainData : trackData;
        selectedData.AddObjectAt(gridPosition,
                                 database.objectsData[selectedObjectIndex].Size,
                                 database.objectsData[selectedObjectIndex].ID,
                                 ((int)database.objectsData[selectedObjectIndex].objectType),
                                 index,
                                 GetCurrentPreviewRotationState());

        previewSystem.UpdatePreview(grid.CellToWorld(gridPosition), false);

        placementSystem.EndCurrentState();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].objectType == ObjectData.Type.Terrain ? terrainData : trackData;
        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size, GetCurrentPreviewRotationState());
    }

    private int GetCurrentPreviewRotationState()
    {
        int rotationState = 0;
        if (previewSystem.previewObjectRotation)
            rotationState = previewSystem.previewObjectRotation.GetRotationState();
        return rotationState;
    }

    public void UpdateState(Vector3 position, bool isWithinBounds)
    {
        bool placementValidity = false;
        if(isWithinBounds)
        {
            placementValidity = CheckPlacementValidity(grid.WorldToCell(position), selectedObjectIndex);
        }
        previewSystem.UpdatePreview(position, placementValidity);
    }
}

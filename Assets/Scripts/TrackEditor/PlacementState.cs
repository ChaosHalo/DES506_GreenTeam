using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    ObjectsDatabaseSO database;
    GridData terrainData;
    GridData trackData;
    ObjectPlacer objectPlacer;
    PreviewSystem previewSystem;


    public PlacementState(int iD,
                          Grid grid,
                          ObjectsDatabaseSO database,
                          GridData terrainData,
                          GridData trackData,
                          ObjectPlacer objectPlacer,
                          PreviewSystem previewSystem)
    {
        ID = iD;
        this.grid = grid;
        this.database = database;
        this.terrainData = terrainData;
        this.trackData = trackData;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;

        previewSystem.StopShowingPreview();

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            // set new preview indicator
            previewSystem.SetNewPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
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

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (placementValidity == false)
        {
            //StopPlacement();
            return;
        }

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));


        GridData selectedData = database.objectsData[selectedObjectIndex].objectType == ObjectData.Type.Terrain ? terrainData : trackData;

        selectedData.AddObjectAt(gridPosition,
                                 database.objectsData[selectedObjectIndex].Size,
                                 database.objectsData[selectedObjectIndex].ID,
                                 ((int)database.objectsData[selectedObjectIndex].objectType),
                                 index);

        previewSystem.UpdatePreview(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].objectType == ObjectData.Type.Terrain ? terrainData : trackData;

        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
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

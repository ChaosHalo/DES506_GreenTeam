using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private GameObject previewIndicator;
    private MeshRenderer previewRenderer;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualisation;

    private GridData terrainData, trackData;

    private List<GameObject> placedObjects = new();


    private void Start()
    {
        StopPlacement();
        terrainData = new();
        trackData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        // set new preview indicator
        previewIndicator = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        previewRenderer = previewIndicator.GetComponentInChildren<MeshRenderer>();

        // enable visualisations
        gridVisualisation.SetActive(true);
        inputManager.OnRelease += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            StopPlacement();
            return;
        }
        if (inputManager.isWithinPlacementBounds == false)
        {
            StopPlacement();
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (placementValidity == false)
        {
            StopPlacement();
            return;
        }

        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.GetComponentInChildren<PlacableObject>().StopScaling();
        placedObjects.Add(newObject);

        // assign datatype
        //ObjectData.Type checkType = database.objectsData[selectedObjectIndex].objectType;
        //GridData selectedData = terrainData;
        //if (checkType == ObjectData.Type.Track)
        //    selectedData = trackData;


        GridData selectedData = database.objectsData[selectedObjectIndex].objectType == ObjectData.Type.Terrain ? terrainData : trackData;

        selectedData.AddObjectAt(gridPosition,
                                 database.objectsData[selectedObjectIndex].Size,
                                 database.objectsData[selectedObjectIndex].ID,
                                 ((int)database.objectsData[selectedObjectIndex].objectType),
                                 placedObjects.Count - 1);

        StopPlacement();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].objectType == ObjectData.Type.Terrain ? terrainData : trackData;

        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        // clear current indicator object
        Destroy(previewIndicator);
        previewIndicator = null;

        selectedObjectIndex = -1;
        gridVisualisation.SetActive(false);
        inputManager.OnRelease -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedObjectIndex < 0)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // adjust for object pivot point offset
        mousePosition.x -= 0.5f;
        mousePosition.z -= 0.5f;

        // snap if over placable grid
        // do not snap if outside of placable grid
        previewIndicator.transform.position = inputManager.isWithinPlacementBounds ? grid.CellToWorld(gridPosition) : mousePosition;


        // change validity indicator
        //if (previewRenderer)
        //{
        //    bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        //    previewRenderer.material.color = placementValidity ? Color.white : Color.red;
        //}
    }
}

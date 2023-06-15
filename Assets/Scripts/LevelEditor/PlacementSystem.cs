using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private GameObject cellIndicator;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualisation;

    private void Start()
    {
        StopPlacement();
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

        // clear current indicator object
        Destroy(cellIndicator);
        cellIndicator = null;
        cellIndicator = Instantiate(database.objectsData[selectedObjectIndex].Prefab);

        // enable visualisations
        gridVisualisation.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnRelease += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
            return;
        if (!inputManager.canPlace)
        {
            StopPlacement();
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        StopPlacement();
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualisation.SetActive(false);
        cellIndicator.SetActive(false);
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
        cellIndicator.transform.position = inputManager.canPlace ? grid.CellToWorld(gridPosition) : mousePosition;
    }
}

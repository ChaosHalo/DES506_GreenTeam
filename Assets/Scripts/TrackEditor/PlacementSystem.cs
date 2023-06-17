using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualisation;

    private GridData terrainData, trackData;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    [SerializeField]
    private PreviewSystem previewSystem;

    [SerializeField]
    private ToggleButton toggleButton_Remove;

    [SerializeField]
    private bool gridVisualAlwaysOn = false;

    IBuildingState buildingState;


    private void Start()
    {
        EndCurrentState();
        terrainData = new();
        trackData = new();
    }

    public void StartPlacement(int ID)
    {
        EndCurrentState();

        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(true);

        buildingState = new PlacementState(ID, grid, database, terrainData, trackData, objectPlacer, previewSystem, this);

        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
    }

    public void StartRemoving()
    {
        EndCurrentState();

        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(true);

        buildingState = new RemovingState(grid, terrainData, trackData, objectPlacer, previewSystem, this, toggleButton_Remove);

        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
    }

    private void PerformAction()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        buildingState.OnAction(gridPosition, inputManager.isWithinPlacementBounds);
    }

    public void EndCurrentState()
    {
        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(false);

        if (buildingState == null)
            return;


        buildingState.EndState();
        inputManager.OnRelease -= PerformAction;
        inputManager.OnExit -= EndCurrentState;
        buildingState = null;
    }

    void Update()
    {
        if (buildingState == null)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // adjust for object pivot point offset
        mousePosition.x -= 50;
        mousePosition.z -= 50;

        // snap if over placable grid
        // do not snap if outside of placable grid
        Vector3 newPreviewPosition = inputManager.isWithinPlacementBounds ? grid.CellToWorld(gridPosition) : mousePosition;

        buildingState.UpdateState(newPreviewPosition, inputManager.isWithinPlacementBounds);
    }
}

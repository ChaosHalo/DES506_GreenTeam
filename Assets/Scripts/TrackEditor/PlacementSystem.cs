using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private int gridSize = 10;

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
    private UIManager uiManager;

    [SerializeField]
    private CurrencyManager currencyManager;

    [SerializeField]
    private bool gridVisualAlwaysOn = false;

    IBuildingState buildingState;


    private void Start()
    {
        EndCurrentState();
        terrainData = new();
        trackData = new();
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        buildingState = new State_GenerateWorld(terrainData, trackData, database, grid, objectPlacer, 10);
        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
        EndCurrentState();
    }

    public void StartPlacement(int ID)
    {
        EndCurrentState();

        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(true);

        buildingState = new State_PlaceTrack(ID, grid, database, terrainData, trackData, objectPlacer, previewSystem, this, currencyManager);

        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
    }

    public void StartRemoving()
    {
        EndCurrentState();

        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(true);

        buildingState = new State_RemoveTrack(grid, terrainData, trackData, objectPlacer, previewSystem, uiManager, this, currencyManager);

        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
    }

    public void StartRotating()
    {
        EndCurrentState();

        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(true);

        buildingState = new State_RotateTrack(grid, database, terrainData, trackData, objectPlacer, previewSystem, uiManager, this);

        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
    }

    public void StartTerrain(int ID)
    {
        EndCurrentState();

        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(true);

        buildingState = new State_PlaceTerrain(ID, grid, database, terrainData, trackData, objectPlacer, previewSystem, this, currencyManager);

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
        // TEMP - allow scene reload
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (buildingState == null)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        inputManager.gridCellPos = grid.WorldToCell(mousePosition);
        inputManager.mouseWorldPos = mousePosition;
        inputManager.gridWorldPos=grid.CellToWorld(inputManager.gridCellPos);

        // adjust for object pivot point offset
        mousePosition.x -= 50;
        mousePosition.z -= 50;

        // snap if over placable grid
        // do not snap if outside of placable grid
        Vector3 newPreviewPosition = inputManager.isWithinPlacementBounds ? grid.CellToWorld(inputManager.gridCellPos) : mousePosition;

        buildingState.UpdateState(newPreviewPosition, inputManager.isWithinPlacementBounds);
    }
}
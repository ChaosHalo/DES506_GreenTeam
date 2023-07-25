using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    internal WorldgenDatabaseSO worldgenDatabase;

    [SerializeField]
    private GameObject gridVisualisation;

    internal GridData terrainData, trackData;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    [SerializeField]
    private PreviewSystem previewSystem;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private CurrencyManager currencyManager;

    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private PerlinNoise perlinNoise;

    [SerializeField]
    private bool gridVisualAlwaysOn = false;

    internal IBuildingState buildingState;

    internal bool isGenerating = false;

    public Vector3 LastPlacedPosition;


    private void Start()
    {
        EndCurrentState();
        terrainData = new(new(gridSize, gridSize));
        trackData = new(new(gridSize, gridSize));
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        buildingState = new State_GenerateWorld(terrainData, trackData, database, worldgenDatabase, grid, objectPlacer, perlinNoise, 10, this);
        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
    }

    public void StartPlacement(int ID)
    {
        if (isGenerating==true)
            return;

        EndCurrentState();

        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(true);

        buildingState = new State_PlaceTrack(ID, grid, database, terrainData, trackData, objectPlacer, previewSystem, this, currencyManager);

        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
    }

    public void StartRemoving()
    {
        if (isGenerating == true)
            return;

        EndCurrentState();

        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(true);

        buildingState = new State_RemoveTrack(grid, terrainData, trackData, objectPlacer, previewSystem, uiManager, this, currencyManager, cameraManager, inputManager);

        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
    }

    public void StartRotating()
    {
        if (isGenerating == true)
            return;

        EndCurrentState();

        if (gridVisualAlwaysOn == false)
            gridVisualisation.SetActive(true);

        buildingState = new State_RotateTrack(grid, database, terrainData, trackData, objectPlacer, previewSystem, uiManager, this, cameraManager, inputManager);

        inputManager.OnRelease += PerformAction;
        inputManager.OnExit += EndCurrentState;
    }

    public void StartTerrain(int ID)
    {
        if (isGenerating == true)
            return;

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

        // remove delegates
        inputManager.OnRelease -= PerformAction;
        inputManager.OnExit -= EndCurrentState;


        buildingState = null;
    }

    void Update()
    {
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
        LastPlacedPosition = inputManager.mouseWorldPos;

        buildingState.UpdateState(newPreviewPosition, inputManager.isWithinPlacementBounds);
    }






    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetLevel(bool resetCurrency = false)
    {
        if (objectPlacer.IsTrackAnimating() == false)
        {
            EndCurrentState();
            GenerateWorld();
            MyGameManager.instance.GetCameraManager().ResetCamera();

            if(resetCurrency)
            {
                MyGameManager.instance.GetCurrencyManager().ResetCurrency();
                MyGameManager.instance.missionManager.ResetMissions();
            }
        }
    }

    #region SAVEDATA
    [System.Serializable]
    public struct WorldData
    {
        public int currency;
        public GridData terrainData;
        public GridData trackData;
        public List<GameObject> placedObjects;

        public WorldData(int currency, GridData terrainData, GridData trackData, List<GameObject> placedObjects)
        {
            this.currency = currency;
            this.terrainData = terrainData;
            this.trackData = trackData;
            this.placedObjects = placedObjects;
        }
    }
    private WorldData savedData;

    public void SaveData()
    {
        SavePlacedPieces();

        GridData newTerrainData = new(terrainData.gridSize);
        newTerrainData.placedObjects.AddRange(terrainData.placedObjects);
        GridData newTrackData = new(trackData.gridSize);
        newTrackData.placedObjects.AddRange(trackData.placedObjects);


        savedData = new(currencyManager.GetPlayerCurrency(), newTerrainData, newTrackData, objectPlacer.placedObjects);
    }

    public void LoadData()
    {
        currencyManager.SetCurrencyTo(savedData.currency);

        GridData newTerrainData = new(savedData.terrainData.gridSize);
        newTerrainData.placedObjects.AddRange(savedData.terrainData.placedObjects);
        GridData newTrackData = new(savedData.trackData.gridSize);
        newTrackData.placedObjects.AddRange(savedData.trackData.placedObjects);

        terrainData.ClearData();
        trackData.ClearData();

        terrainData = newTerrainData;
        trackData= newTrackData;

        foreach(GameObject obj in objectPlacer.placedObjects)
        {
            if (obj != null)
            {
                PlacableObject placableObject = obj.GetComponentInChildren<PlacableObject>();
                if (placableObject != null)
                {
                   // if (!placableObject.isSaved)
                            Destroy(obj);
                }
            }
        }

        objectPlacer.placedObjects.Clear();
        
        foreach(var data in terrainData.placedObjects)
        {
            objectPlacer.PlaceObject(database.objectsData[data.Value.ID].Prefab,
                                                 grid.CellToWorld(data.Value.originPosition),
                                                 0,
                                                 true,
                                                 ObjectData.ObjectType.Terrain,
                                                 database.objectsData[data.Value.ID].trackType,
                                                 database.objectsData[data.Value.ID].terrainType,
                                                 true);
        }
        foreach (var data in trackData.placedObjects)
        {
            objectPlacer.PlaceObject(database.objectsData[data.Value.ID].Prefab,
                                                 grid.CellToWorld(data.Value.originPosition),
                                                 0,
                                                 true,
                                                 ObjectData.ObjectType.Terrain,
                                                 database.objectsData[data.Value.ID].trackType,
                                                 database.objectsData[data.Value.ID].terrainType,
                                                 true);
        }


        // objectPlacer.placedObjects = savedData.placedObjects;
    }

    private void SavePlacedPieces()
    {
        foreach (GameObject obj in objectPlacer.placedObjects)
        {
            if(obj != null)
            {
                PlacableObject placableObject = obj.GetComponentInChildren<PlacableObject>();
                if (placableObject != null)
                {
                    placableObject.isSaved = true;
                }
            }
        }
    }
    #endregion
}

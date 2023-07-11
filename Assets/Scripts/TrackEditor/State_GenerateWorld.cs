using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class State_GenerateWorld : IBuildingState
{
    GridData terrainData;
    GridData trackData;
    ObjectPlacer objectPlacer;
    Grid grid;
    ObjectsDatabaseSO database;
    PerlinNoise perlinNoise;
    PlacementSystem placementSystem;
    int gridSize;

    // generate pre-placed track pieces within these bounds
    int generationBoundsX = 4;
    int generationBoundsY = 4;

    // store used track positions
    List<Vector3Int> availablePositions = new();

    float curTime = 0;
    float delayTime = 1f;
    bool placedTrack = false;

    public struct ObjectToPlace
    {
        public GameObject prefab;
        public Vector3 position;
        public int rotationState;
        public bool canModify;
        public ObjectData.ObjectType objectType;
        public ObjectData.TrackType trackType;
        public ObjectData.TerrainType terrainType;
        public bool placedByUser;

        public ObjectToPlace(GameObject prefab, Vector3 position, int rotationState, bool canModify, ObjectData.ObjectType objectType, ObjectData.TrackType trackType, ObjectData.TerrainType terrainType, bool placedByUser)
        {
            this.prefab = prefab;
            this.position = position;
            this.rotationState = rotationState;
            this.canModify = canModify;
            this.objectType = objectType;
            this.trackType = trackType;
            this.terrainType = terrainType;
            this.placedByUser = placedByUser;
        }
    }

    private List<ObjectToPlace> allObjectsToPlace=new();
    int objectNumber = 0;

    public State_GenerateWorld(GridData terrainData, GridData trackData, ObjectsDatabaseSO database, Grid grid, ObjectPlacer objectPlacer, PerlinNoise perlinNoise, int gridSize, PlacementSystem placementSystem)
    {
        this.terrainData = terrainData;
        this.trackData = trackData;
        this.objectPlacer = objectPlacer;
        this.gridSize = gridSize;
        this.database = database;
        this.grid = grid;
        this.perlinNoise = perlinNoise;
        this.placementSystem = placementSystem;

        BeginGeneration();
    }

    private void BeginGeneration()
    {
        availablePositions.Clear();
        objectPlacer.ClearAllObjects();
        terrainData.ClearData();
        trackData.ClearData();
        allObjectsToPlace.Clear();
        objectNumber = 0;

        perlinNoise.BeginGenerate();
        GenerateTerrain();
        GenerateTrack();
        placementSystem.isGenerating = false;
    }

    private void GenerateTerrain()
    {
        int ID = 3; // 3 = grass
        int type = (int)ObjectData.ObjectType.Terrain;
        int rotationState = 0;
        GridData selectedData = terrainData;
        Vector2Int size = new(1, 1);

        int halfGrid = gridSize / 2;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3Int gridPosition = new(x -halfGrid, 0, y-halfGrid);

                // get tile type from perlin noise
                ID = perlinNoise.GetTileID(y, x);

                // place world object (index 3 = grass)
                ObjectToPlace newObject = new ObjectToPlace(database.objectsData[ID].Prefab, grid.CellToWorld(gridPosition), 0, true, ObjectData.ObjectType.Terrain, database.objectsData[ID].trackType, database.objectsData[ID].terrainType, false);
                allObjectsToPlace.Add(newObject);

                //int index = objectPlacer.PlaceObject(database.objectsData[ID].Prefab, grid.CellToWorld(gridPosition), 0, true, ObjectData.ObjectType.Terrain, database.objectsData[ID].trackType, database.objectsData[ID].terrainType, false);



                // place database object
                selectedData.AddObjectAt(gridPosition, size, ID, type, objectNumber, rotationState, true, database.objectsData[ID].cost, database.objectsData[ID].isBuildable);
                objectNumber++;
            }
        }
    }

    private void GenerateTrack()
    {
        List<int> IDs = new List<int>() { 0, 1, 8 }; // track pieces to place
       //List<int> IDs = new List<int>() { 8 }; // track pieces to place
        int type = (int)ObjectData.ObjectType.Track;
        GridData selectedData = trackData;

        int halfX = generationBoundsX / 2;
        int halfY = generationBoundsY / 2;

        GenerateUnusedPositions(halfX, halfY);


        List<Vector3> rotations = new()
        {
            new(0, 0, 0),
            new(0, 90, 0),
            new(0, 180, 0),
            new(0, -90, 0)
        };


        for (int i = 0; i < IDs.Count; i++)
        {
            if (availablePositions.Count <= 0)
            {
                Debug.LogWarning("WARNING: No Free Tiles Available. Regenerating World...");
                BeginGeneration();
                return;
            }

            int rotationState = Random.Range(0, 4);
            Vector3Int gridPosition = GetRandomPosition(halfX, halfY);

            // place world object (index 3 = grass)
            ObjectToPlace newObject = new ObjectToPlace(database.objectsData[IDs[i]].Prefab, grid.CellToWorld(gridPosition), rotationState, false, ObjectData.ObjectType.Track, database.objectsData[IDs[i]].trackType, database.objectsData[IDs[i]].terrainType, false);
            allObjectsToPlace.Add(newObject);
            
            //int index = objectPlacer.PlaceObject(database.objectsData[IDs[i]].Prefab, grid.CellToWorld(gridPosition), rotationState, false, ObjectData.ObjectType.Track, database.objectsData[IDs[i]].trackType, database.objectsData[IDs[i]].terrainType, false);

            // place database object
            selectedData.AddObjectAt(gridPosition, database.objectsData[IDs[i]].Size, IDs[i], type, objectNumber, rotationState, false, database.objectsData[IDs[i]].cost, database.objectsData[IDs[i]].isBuildable);
            objectNumber++;
        }

        PlaceObjects();
        placementSystem.EndCurrentState();
    }

    private void PlaceObjects()
    {
        foreach (ObjectToPlace obj in allObjectsToPlace)
            objectPlacer.PlaceObject(obj.prefab, obj.position, obj.rotationState, obj.canModify, obj.objectType, obj.trackType, obj.terrainType, obj.placedByUser);
    }

    private void GenerateUnusedPositions(int halfX, int halfY)
    {
        List<Vector3Int> unavailablePositions = new();

        for (int x = -halfX; x < halfX; x++)
        {
            for (int y = -halfY; y < halfY; y++)
            {
                Vector3Int newPos = new(x, 0, y);

                if (terrainData.IsBuildableAt(newPos, new(1, 1), 0) == true)
                    availablePositions.Add(newPos);
                else
                {
                    unavailablePositions.Add(newPos);
                    unavailablePositions.AddRange(CalculateAdjacentPositions(newPos));
                }
            }
        }

        // remove unavailable positions from available
        var theSet = new HashSet<Vector3Int>(unavailablePositions);
        availablePositions.RemoveAll(item => theSet.Contains(item));
    }

    private Vector3Int GetRandomPosition(int halfX, int halfY)
    {
        // get random position
        Vector3Int newPos = availablePositions[Random.Range(0, availablePositions.Count)];

        // remove adjacent positions from list of available positions
        List<Vector3Int> unavailablePositions = CalculateAdjacentPositions(newPos);

        // remove unavailable positions from available
        var theSet = new HashSet<Vector3Int>(unavailablePositions);
        availablePositions.RemoveAll(item => theSet.Contains(item));

        // return new position
        return newPos;
    }

    List<Vector3Int> CalculateAdjacentPositions(Vector3Int originPos)
    {
        // all 8 adjacent positions
        return new()
            {
                originPos,
                new(originPos.x-1, 0, originPos.z),
                new(originPos.x+1, 0, originPos.z),
                new(originPos.x, 0, originPos.z-1),
                new(originPos.x, 0, originPos.z+1),
                new(originPos.x-1, 0, originPos.z-1),
                new(originPos.x+1, 0, originPos.z-1),
                new(originPos.x-1, 0, originPos.z+1),
                new(originPos.x+1, 0, originPos.z+1),
            };
    }

    public void EndState() 
    {
        placementSystem.isGenerating = false;
    }
    public void OnAction(Vector3Int gridPosition, bool isWithinBounds) { }
    public void UpdateState(Vector3 gridPosition, bool isWithinBounds) 
    {
        //curTime += Time.deltaTime;
        //if (curTime >= delayTime)
        //{
        //    if(placedTrack==false)
        //    {
        //        placedTrack = true;
        //        GenerateTrack();
        //    }
        //}
    }
}

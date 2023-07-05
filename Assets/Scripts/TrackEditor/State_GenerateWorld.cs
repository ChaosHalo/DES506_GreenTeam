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
    int gridSize;

    // generate pre-placed track pieces within these bounds
    int generationBoundsX = 4;
    int generationBoundsY = 4;

    // store used track positions
    List<Vector3Int> availablePositions = new();

    public State_GenerateWorld(GridData terrainData, GridData trackData, ObjectsDatabaseSO database, Grid grid, ObjectPlacer objectPlacer, PerlinNoise perlinNoise, int gridSize)
    {
        this.terrainData = terrainData;
        this.trackData = trackData;
        this.objectPlacer = objectPlacer;
        this.gridSize = gridSize;
        this.database = database;
        this.grid = grid;
        this.perlinNoise = perlinNoise;

        GenerateTerrain();
        GenerateTrack();
        EndState();
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
                int index = objectPlacer.PlaceObject(database.objectsData[ID].Prefab, grid.CellToWorld(gridPosition), 0, true, ObjectData.ObjectType.Terrain, database.objectsData[ID].trackType, database.objectsData[ID].terrainType, false);

                // place database object
                selectedData.AddObjectAt(gridPosition, size, ID, type, index, rotationState, true, database.objectsData[ID].cost);
            }
        }
    }

    private void GenerateTrack()
    {
        List<int> IDs = new List<int>() { 0, 1, 8 }; // track pieces to place
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
            int rotationState = Random.Range(0, 4);
            Vector3Int gridPosition = GetRandomPosition(halfX, halfY);

            // place world object (index 3 = grass)
            int index = objectPlacer.PlaceObject(database.objectsData[IDs[i]].Prefab, grid.CellToWorld(gridPosition), rotationState, false, ObjectData.ObjectType.Track, database.objectsData[IDs[i]].trackType, database.objectsData[IDs[i]].terrainType, false);

            // place database object
            selectedData.AddObjectAt(gridPosition, database.objectsData[IDs[i]].Size, IDs[i], type, index, rotationState, false, database.objectsData[IDs[i]].cost);
        }
    }

    private void GenerateUnusedPositions(int halfX, int halfY)
    {
        for (int x = -halfX; x < halfX; x++)
        {
            for (int y = -halfY; y < halfY; y++)
            {
                availablePositions.Add(new(x, 0, y));
            }
        }
    }

    private Vector3Int GetRandomPosition(int halfX, int halfY)
    {
        // get random position
        Vector3Int newPos = availablePositions[Random.Range(0, availablePositions.Count)];

        // temp store this and adjacent positions
        List<Vector3Int> unavailablePositions = new()
            {
                newPos,
                new(newPos.x-1, 0, newPos.z),
                new(newPos.x+1, 0, newPos.z),
                new(newPos.x, 0, newPos.z-1),
                new(newPos.x, 0, newPos.z+1),
                new(newPos.x-1, 0, newPos.z-1),
                new(newPos.x+1, 0, newPos.z-1),
                new(newPos.x-1, 0, newPos.z+1),
                new(newPos.x+1, 0, newPos.z+1),
            };

        // remove unavailable positions from available
        var theSet = new HashSet<Vector3Int>(unavailablePositions);
        availablePositions.RemoveAll(item => theSet.Contains(item));

        // return new position
        return newPos;
    }

    public void EndState() { }
    public void OnAction(Vector3Int gridPosition, bool isWithinBounds) { }
    public void UpdateState(Vector3 gridPosition, bool isWithinBounds) { }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private Button trackModeButton;


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
    private GridData CloneGridData(GridData originalGridData)
    {
        if (originalGridData == null)
            return null;

        GridData newGridData = new GridData(originalGridData.gridSize);
        newGridData.placedObjects.AddRange(originalGridData.placedObjects);
        return newGridData;
    }

    public void SaveData()
    {
        SavePlacedPieces();

        GridData newTerrainData = CloneGridData(placementSystem.terrainData);
        GridData newTrackData = CloneGridData(placementSystem.trackData);

        savedData = new(currencyManager.GetPlayerCurrency(), newTerrainData, newTrackData, objectPlacer.placedObjects);
    }

    public void LoadData()
    {
        // clone saved data
        GridData newTerrainData = CloneGridData(savedData.terrainData);
        GridData newTrackData = CloneGridData(savedData.trackData);

        // clear current data
        placementSystem.terrainData.ClearData();
        placementSystem.trackData.ClearData();

        // restore saved data
        placementSystem.terrainData = newTerrainData;
        placementSystem.trackData = newTrackData;

        // clear current objects
        foreach (GameObject obj in objectPlacer.placedObjects)
            Destroy(obj);
        objectPlacer.placedObjects.Clear();

        // restore saved objects
        RestoreObjects(placementSystem.terrainData);
        RestoreObjects(placementSystem.trackData);

        // handle misc
        currencyManager.SetCurrencyTo(savedData.currency);
        trackModeButton.onClick.Invoke();
    }

    private void RestoreObjects(GridData gridData)
    {
        foreach (var data in gridData.placedObjects)
        {
            objectPlacer.PlaceObject(placementSystem.database.objectsData[data.Value.ID].Prefab,
                                                 placementSystem.grid.CellToWorld(data.Value.originPosition),
                                                 data.Value.RotationState,
                                                 data.Value.canModify,
                                                 (ObjectData.ObjectType)data.Value.objectType,
                                                 placementSystem.database.objectsData[data.Value.ID].trackType,
                                                 placementSystem.database.objectsData[data.Value.ID].terrainType,
                                                 false);
        }
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
                    placableObject.SetIsSaved(true);
                }
            }
        }
    }
}

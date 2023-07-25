using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Missions;
using static GameStateManager;
using UnityEditor;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> placedObjects = new();

    [Header("Events")]
    public MissionEvent onPlaceTrack;
    public MissionEvent onPlaceTrackStraight;
    public MissionEvent onPlaceTrackCurve;
    public MissionEvent onPlaceTrackLoop;
    public MissionEvent onPlaceTerrainGrass;
    public MissionEvent onPlaceTerrainDesert;
    public MissionEvent onPlaceTerrainSnow;
    public MissionEvent onPlaceTerrainSea;
    public MissionEvent onPlaceTerrainMountain;
    public MissionEvent OnSpendCurrency;

    public int PlaceObject(GameObject prefab, Vector3 position, int rotationState, bool canModify, ObjectData.ObjectType objectType, ObjectData.TrackType trackType, ObjectData.TerrainType terrainType, bool placedByUser)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        PlacableObject placeableObject = newObject.GetComponentInChildren<PlacableObject>();
        placeableObject.StopScaling();
        placeableObject.SetModifyable(canModify);
        placeableObject.objectType = objectType;
        placeableObject.objectPlacer = this;
        if (placeableObject.autoRotate)
            placeableObject.autoRotate.SetRotationState(rotationState, true);
        placeableObject.SetIsPlaced(true);
        placedObjects.Add(newObject);

        int verticalOffset = 100;

        // EVENTS
        if (placedByUser == true)
        {
            if (objectType == ObjectData.ObjectType.Track)
            {
                onPlaceTrack.Raise();
                if (trackType == ObjectData.TrackType.Straight)
                    onPlaceTrackStraight.Raise();
                if (trackType == ObjectData.TrackType.Curve)
                    onPlaceTrackCurve.Raise();
                if (trackType == ObjectData.TrackType.Loop)
                    onPlaceTrackLoop.Raise();
            }
            if (objectType == ObjectData.ObjectType.Terrain)
            {
                if (terrainType == ObjectData.TerrainType.Grass)
                    onPlaceTerrainGrass.Raise();
                if (terrainType == ObjectData.TerrainType.Desert)
                    onPlaceTerrainDesert.Raise();
                if (terrainType == ObjectData.TerrainType.Snow)
                    onPlaceTerrainSnow.Raise();
                if (terrainType == ObjectData.TerrainType.Sea)
                    onPlaceTerrainSea.Raise();
                if (terrainType == ObjectData.TerrainType.Mountain)
                    onPlaceTerrainMountain.Raise();
            }
        }
        else
        {
            if (objectType == ObjectData.ObjectType.Terrain)
                verticalOffset = UnityEngine.Random.Range(300, 500);
            if (objectType == ObjectData.ObjectType.Track)
                verticalOffset = 100;
        }

        //Vector3 trackYOffset = new Vector3(position.x, position.y + verticalOffset, position.z);
        //newObject.transform.position = trackYOffset;
        placeableObject.verticalOffset = verticalOffset;
        placeableObject.TriggerFallAnim(placedByUser);

        return placedObjects.Count - 1;
    }

    internal void ClearAllObjects()
    {
        for (int i = 0; i < placedObjects.Count; i++)
            RemoveObjectAt(i, false);

        placedObjects.Clear();
    }

    internal void RemoveObjectAt(int gameObjectIndex, bool doDelay = true)
    {
        if (placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return;

        PlacableObject placableObject = placedObjects[gameObjectIndex].GetComponentInChildren<PlacableObject>();
        placableObject.OnDelete();

        if (doDelay == false)
        {
            Destroy(placedObjects[gameObjectIndex]);
            placedObjects[gameObjectIndex] = null;
            return;
        }

        if(placableObject.objectType==ObjectData.ObjectType.Track)
        StartCoroutine(DeleteAfterDelay(gameObjectIndex, 0.2f));
        if (placableObject.objectType == ObjectData.ObjectType.Terrain)
            StartCoroutine(DeleteAfterDelay(gameObjectIndex, 0));
    }

    private IEnumerator DeleteAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay/2f);
        UpdateTrackConnections();
        yield return new WaitForSeconds(delay / 2f);

        Destroy(placedObjects[index]);
        placedObjects[index] = null;
    }

    internal bool AreObjectsAvailable()
    {
        if (placedObjects.Count < 1)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < placedObjects.Count; i++)
            {
                if (placedObjects[i] != null)
                    return true;
            }
        }
        return false;
    }

    // return new rotation state
    internal int RotateObjectAt(int gameObjectIndex)
    {
        if (placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return -1;

        AutoRotate objectToRotate = placedObjects[gameObjectIndex].GetComponentInChildren<AutoRotate>();

        if (objectToRotate == null)
            return -1;

        int newState = objectToRotate.GetRotationState();
        newState++;
        if (newState > 3)
            newState = 0;

        objectToRotate.SetRotationState(newState, false);
       // UpdateTrackConnections();
        return newState;
    }

    // only use if state is guaranteed valid
    internal void RotateObjectToState(int gameObjectIndex, int newState)
    {
        if (placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return;

        AutoRotate objectToRotate = placedObjects[gameObjectIndex].GetComponentInChildren<AutoRotate>();

        if (objectToRotate == null)
            return;

        objectToRotate.SetRotationState(newState, false);
       // UpdateTrackConnections();
    }

    internal void UpdateTrackConnections()
    {
        foreach (GameObject obj in placedObjects)
        {
            if (obj != null)
            {
                PlacableObject placableObj = obj.GetComponentInChildren<PlacableObject>();
                if (placableObj != null)
                {
                    placableObj.UpdateConnectionState();
                }
            }
        }
    }

    internal void StartChainCheck()
    {
        // reset pieces
        foreach (PlacableObject obj in GetAllPlacableObjects())
        {
            obj.hasCompletedChainCheck = false;
        }

        // find Start Piece
        PlacableObject startPiece = null;
        foreach (PlacableObject obj in GetAllPlacableObjects())
        {
            if (obj.isStartPiece)
            {
                startPiece = obj;
                break;
            }
        }

        // start chain at start piece
        if (startPiece != null)
        {
            startPiece.BeginChainCheck();
        }
    }

    internal bool IsTrackFullyConnected()
    {
        // check if each piece is individually connected
        // check each piece in chain for connection to start
        foreach (PlacableObject obj in GetAllPlacableObjects())
        {
            if (obj.IsConnectedToStart() == false)
            {
                Debug.Log("Not Connected To Start");
                return false;
            }
            if (obj.GetConnectedStatus() == false)
            {
                Debug.Log("Pieces Not Connected");
                return false;
            }
        }

        return true;
    }

    internal bool IsTrackAnimating()
    {
        foreach(PlacableObject obj in GetAllPlacableObjects())
        {
            if (obj.GetFallingStatus() == true)
            {
                return true;
            }
        }
        return false;
    }

    private List<PlacableObject> GetAllPlacableObjects()
    {
        List<PlacableObject> newList = new();

        foreach (GameObject obj in placedObjects)
        {
            if (obj != null)
            {
                PlacableObject placableObject = obj.GetComponentInChildren<PlacableObject>();
                if (placableObject != null)
                {
                    if (placableObject.objectType == ObjectData.ObjectType.Track)
                    {
                        newList.Add(placableObject);
                    }
                }
            }
        }

        return newList;
    }


    internal void TriggerFaillAnimations()
    {
        foreach (GameObject obj in placedObjects)
            if(obj!= null)
                obj.GetComponentInChildren<PlacableObject>().TriggerFallAnim(false);
    }
}

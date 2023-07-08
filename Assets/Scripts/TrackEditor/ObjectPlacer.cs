using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Missions;
using static CustomSceneManager;

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

        // EVENTS
        if (placedByUser == true)
        {
            Vector3 trackYOffset = new Vector3(position.x, position.y + 100, position.z);
            newObject.transform.position = trackYOffset;
            placeableObject.OnPlace();

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

        return placedObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return;

        PlacableObject placableObject = placedObjects[gameObjectIndex].GetComponentInChildren<PlacableObject>();
        placableObject.OnDelete();

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

    internal bool IsTrackFullyConnected()
    {
        foreach (GameObject obj in placedObjects)
        {
            if (obj != null)
            {
                PlacableObject placableObject = obj.GetComponentInChildren<PlacableObject>();
                if (placableObject != null)
                {
                    if (placableObject.objectType == ObjectData.ObjectType.Track)
                    {
                        if (placableObject.GetConnectedStatus() == false)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }
}

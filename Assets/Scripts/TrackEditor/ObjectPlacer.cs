using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position, int rotationState, bool canModify, ObjectData.Type type)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        PlacableObject placeableObject = newObject.GetComponentInChildren<PlacableObject>();
        placeableObject.StopScaling();
        placeableObject.SetModifyable(canModify);
        placeableObject.objectType = type;
        if(placeableObject.autoRotate)
            placeableObject.autoRotate.SetRotationState(rotationState);
        placedObjects.Add(newObject);
        UpdateTrackConnections();
        return placedObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return;

        Destroy(placedObjects[gameObjectIndex]);
        placedObjects[gameObjectIndex] = null;

        Invoke("UpdateTrackConnections", 0.01f);
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

        objectToRotate.SetRotationState(newState);
        UpdateTrackConnections();
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

        objectToRotate.SetRotationState(newState);
        UpdateTrackConnections();
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
}

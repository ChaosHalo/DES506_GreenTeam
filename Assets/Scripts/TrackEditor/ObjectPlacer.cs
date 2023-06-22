using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position, Quaternion rotation, bool canModify)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        newObject.transform.GetChild(1).localRotation = rotation;
        PlacableObject placeableObject = newObject.GetComponentInChildren<PlacableObject>();
        placeableObject.StopScaling();
        placeableObject.SetModifyable(canModify);
        placedObjects.Add(newObject);
        return placedObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return;

        Destroy(placedObjects[gameObjectIndex]);
        placedObjects[gameObjectIndex] = null;
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
        return newState;
    }
}

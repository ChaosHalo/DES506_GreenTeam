using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        newObject.GetComponentInChildren<PlacableObject>().StopScaling();
        placedObjects.Add(newObject);
        return placedObjects.Count - 1;
    }
}

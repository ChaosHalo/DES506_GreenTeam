using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObject : MonoBehaviour
{
    [Header("Spawn Conditions")]
    [SerializeField] private int minCount = 3;
    [SerializeField] private int maxCount = 6;
    [SerializeField] private int minBounds = -45;
    [SerializeField] private int maxBound = 45;

    public enum Type { NONE, Grass, Desert, Snow, Sea, Mountain}

    [Header("Terrain Type Categorisation")]
    [SerializeField]
    public Type terrainType;
    public Type GetTerrainType() { return terrainType; }


    [Header("Cosmetic Objects")]
    public List<GameObject> objectPrefabs = new();
    public GameObject specialObject;

    private List<GameObject> instantiatedObjects = new();

    int failCounter;

    public void GenerateObjects()
    {
        // special object
        if (specialObject != null)
        {
            GameObject newObject = Instantiate(specialObject);
            newObject.transform.position = transform.position;
            newObject.transform.parent = transform;
            newObject.transform.Rotate(new(0, 0, Random.Range(0, 360)));
        }


        // normal objects
        if (objectPrefabs.Count == 0)
            return;

        int count = Random.Range(minCount, maxCount);

        for(int i = 0; i < count; i++)
        {
            failCounter = 0;
            GameObject newObject = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Count)]);
            newObject.transform.position = GetNewPosition();
            newObject.transform.parent = transform;
            newObject.transform.Rotate(new(0, 0, Random.Range(0, 360)));
            instantiatedObjects.Add( newObject );
        }
    }

    private Vector3 GetNewPosition()
    {
        float xCoord = Random.Range(minBounds, maxBound);
        float yCoord = Random.Range(minBounds, maxBound);

        Vector3 newPos = new(transform.position.x + xCoord, transform.position.y, transform.position.z + yCoord);

        foreach(GameObject obj in instantiatedObjects)
        {
            if (Vector3.Distance(obj.transform.position, newPos) < 20)
            {
                failCounter++;
                if (failCounter < 50)
                    return GetNewPosition();
                else
                    Debug.Log("Reached 50 fails");
            }
        }

        return newPos;
    }
}

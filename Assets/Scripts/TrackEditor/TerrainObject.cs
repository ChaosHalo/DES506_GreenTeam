using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObject : MonoBehaviour
{
    public enum Type { NONE, Grass, Desert, Snow, Sea, Mountain}

    [Header("Terrain Type Categorisation")]
    [SerializeField]
    public Type terrainType;
    public Type GetTerrainType() { return terrainType; }

    [Header("Cosmetic Objects")]
    public List<GameObject> objectPrefabs = new();

    private List<GameObject> instantiatedObjects = new();

    int failCounter;

    public void GenerateObjects()
    {
        if (objectPrefabs.Count == 0)
            return;

        int count = Random.Range(3, 6);

        for(int i = 0; i < count; i++)
        {
            failCounter = 0;
            GameObject newObject = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Count)]);
            newObject.transform.position = GetNewPosition();
            newObject.transform.parent = transform;
            instantiatedObjects.Add( newObject );
        }
    }

    private Vector3 GetNewPosition()
    {
        float xCoord = Random.Range(-45, 45);
        float yCoord = Random.Range(-45, 45);

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

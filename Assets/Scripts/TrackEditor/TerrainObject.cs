using MoreMountains.HighroadEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObject : MonoBehaviour
{
    [Header("Components")]
    private MoreMountains.HighroadEngine.SoundManager soundManager;
    [SerializeField] private PlacableObject placableObject;

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
    public List<GameObject> specialObject = new();

    private List<GameObject> instantiatedObjects = new();

    int failCounter;

    private void Start()
    {
        SetupSound();
    }

    private void SetupSound()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
            return;

        ObjectData data = MyGameManager.instance.GetPlacementSystem().database.objectsData[placableObject.ID];
        AudioClip clipToPlay = soundManager.GetRandomClip(data.soundsAmbient);
        soundManager.PlayLoop(clipToPlay, transform.position, transform, 0, data.rangeAmbient, data.volumeAmbient);
    }

    public void GenerateObjects()
    {
        // special object
        if (specialObject.Count > 0)
        {
            GameObject newObject = Instantiate(specialObject[Random.Range(0, specialObject.Count)]);
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

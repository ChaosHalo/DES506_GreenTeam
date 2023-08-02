using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTextureChanger : MonoBehaviour
{
    public bool doChangeTexture = true;

    [Header("Textures")]
    public Material defaultMaterial;
    public Material sandMaterial;
    public Material snowMaterial;
    public Material waterMaterial;

    public PlacableObject placableObject;
    public GameObject inflatableObject;

    private void OnTriggerStay(Collider other)
    {
        TerrainObject terrainObject = other.GetComponentInChildren<TerrainObject>();
        if (terrainObject == null)
            return;

        switch (terrainObject.terrainType)
        {
            case TerrainObject.Type.Sea:
                SetMaterialForAllInstances(waterMaterial);
                inflatableObject.SetActive(placableObject.isPlaced ? true : false);
                break;
            case TerrainObject.Type.Grass:
                SetMaterialForAllInstances(defaultMaterial);
                inflatableObject.SetActive(false);
                break;
            case TerrainObject.Type.Snow:
                SetMaterialForAllInstances(snowMaterial);
                inflatableObject.SetActive(false);
                break;
            case TerrainObject.Type.Desert:
                SetMaterialForAllInstances(sandMaterial);
                inflatableObject.SetActive(false);
                break;
        }
    }

    private void SetMaterialForAllInstances(Material newMaterial)
    {
        if (doChangeTexture == false)
            return;

        for (int i = 0; i < placableObject.originalMaterialInstance.Count; i++)
        {
            placableObject.originalMaterialInstance[i] = newMaterial;
        }
    }
}

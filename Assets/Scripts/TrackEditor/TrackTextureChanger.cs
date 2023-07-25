using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTextureChanger : MonoBehaviour
{
    [Header("Textures")]
    public Material defaultMaterial;
    public Material sandMaterial;
    public Material snowMaterial;
    public Material waterMaterial;

    public PlacableObject placableObject;

    private void OnTriggerEnter(Collider other)
    {
        TerrainObject terrainObject = other.GetComponentInChildren<TerrainObject>();
        if (terrainObject == null)
            return;

        switch (terrainObject.terrainType)
        {
            case TerrainObject.Type.Sea:
                SetMaterialForAllInstances(waterMaterial);
                break;
            case TerrainObject.Type.Grass:
                SetMaterialForAllInstances(defaultMaterial);
                break;
            case TerrainObject.Type.Snow:
                SetMaterialForAllInstances(snowMaterial);
                break;
            case TerrainObject.Type.Desert:
                SetMaterialForAllInstances(sandMaterial);
                break;
        }
    }

    private void SetMaterialForAllInstances(Material newMaterial)
    {
        for (int i = 0; i < placableObject.originalMaterialInstance.Count; i++)
        {
            placableObject.originalMaterialInstance[i] = newMaterial;
        }
    }
}

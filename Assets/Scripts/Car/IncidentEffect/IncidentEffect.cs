using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;

public class IncidentEffect : MonoBehaviour
{
    private TerrainObject terrain;
    private TerrainObject.Type type;
    public IncidentEffectScriptableObject IncidentEffectScriptableObject;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<TerrainObject>();
        type = terrain.terrainType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            CarManager carManager = other.GetComponent<CarManager>();
            ResetEffct(carManager);
            FSMEffect(carManager, IncidentEffectScriptableObject);
        }
    }
    private void ResetEffct(CarManager carManager)
    {
        carManager.InitCarData();
    }
    private void FSMEffect(CarManager carManager, IncidentEffectScriptableObject IncidentEffectScriptableObject)
    {
        IIncidentEffect incidentEffect = new GrassEffect();
        switch (type)
        {
            case TerrainObject.Type.Grass:
                incidentEffect = new GrassEffect();
                break;
            case TerrainObject.Type.Desert:
                incidentEffect = new DessertEffect();
                break;
            case TerrainObject.Type.Sea:
                incidentEffect = new WaterEffect();
                break;
            case TerrainObject.Type.Snow:
                incidentEffect = new SnowEffect();
                break;
        }
        incidentEffect.EnterEffect(carManager, IncidentEffectScriptableObject);
    }
}

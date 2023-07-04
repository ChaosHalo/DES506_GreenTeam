using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObject : MonoBehaviour
{
    public enum Type { NONE, Grass, Desert, Snow, Sea, Mountain}

    [SerializeField]
    public Type terrainType;
    public Type GetTerrainType() { return terrainType; }
}

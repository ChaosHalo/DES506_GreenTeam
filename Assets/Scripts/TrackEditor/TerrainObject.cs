using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObject : MonoBehaviour
{
    internal enum Type { GRASS, DESERT, SNOW, SEA, MOUNTAIN}

    [SerializeField]
    private Type terrainType;
    internal Type GetTerrainType() { return terrainType; }
}

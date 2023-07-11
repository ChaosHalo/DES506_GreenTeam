using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;
}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } =  Vector2Int.one;

    public enum ObjectType{ Terrain, Track};

    [field: SerializeField]
    public ObjectType objectType;

    public enum TrackType {None, Straight, Curve, Loop};

    [field: SerializeField]
    public TrackType trackType;

    public enum TerrainType { None, Grass, Desert, Snow, Sea, Mountain};

    [field: SerializeField]
    public TerrainType terrainType;


    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public int cost { get; private set; }

    [field: SerializeField]
    public bool isBuildable { get; private set; }

    [field: SerializeField]
    public int minimumGeneratedCount { get; private set; }
}

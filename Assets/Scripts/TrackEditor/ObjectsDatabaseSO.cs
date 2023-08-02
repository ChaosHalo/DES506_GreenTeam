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
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public Vector2Int Size { get; private set; } =  Vector2Int.one;
    [field: SerializeField] public int cost { get; private set; }
    [field: SerializeField] public int unlockCost { get; private set; }

    public enum ObjectType{ Terrain, Track};

    [field: SerializeField, Space(10)]
    public ObjectType objectType;

    public enum TrackType {None, Straight, Curve, Loop};

    [field: SerializeField]
    public TrackType trackType;

    public enum TerrainType { None, Grass, Desert, Snow, Sea, Mountain};

    [field: SerializeField]
    public TerrainType terrainType;




    [field: SerializeField]
    public bool isBuildable { get; private set; }

    [field: SerializeField]
    public int minimumGeneratedCount { get; private set; }
    [field: SerializeField]
    public int maximumGeneratedCount { get; private set; }

    [field: SerializeField, Space(10)] public List<AudioClip> soundsPlace{ get; private set; }
    [field: SerializeField, Range(0, 1)] public float volumePlace { get; private set; } = 1f;
    [field: SerializeField, Space(10)] public List<AudioClip> soundsAmbient{ get; private set; }
    [field: SerializeField, Range(0, 1)] public float volumeAmbient { get; private set; } = 1f;
    [field: SerializeField, Range(0, 750), Tooltip("Maximum distance is the distance sound stops attenuating at")] public int rangeAmbient { get; private set; } = 150;
    [field: SerializeField, Space(10)] public List<AudioClip> soundsRotate{ get; private set; }
    [field: SerializeField, Range(0, 1)] public float volumeRotate { get; private set; } = 1f;
    [field: SerializeField, Space(10)] public List<AudioClip> soundsDelete{ get; private set; }
    [field: SerializeField, Range(0, 1)] public float volumeDelete { get; private set; } = 1f;


}

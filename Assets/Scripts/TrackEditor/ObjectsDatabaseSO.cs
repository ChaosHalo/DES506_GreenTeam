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

    public enum Type{ Terrain, Track};

    [field: SerializeField]
    public Type objectType;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public int cost { get; private set; }
}

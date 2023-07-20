using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

[CreateAssetMenu]
public class WorldgenDatabaseSO : ScriptableObject
{
    public List<WorldgenData> worldgenData;
}

[Serializable]
public class WorldgenData
{
    [field: SerializeField]
    public List<int> trackIDs { get; private set; }

    [field: SerializeField]
    public List<int> randomTrackIdPool { get; private set; }

    [field: SerializeField]
    public int randomTrackCount { get; private set; }

    [field: SerializeField]
    public Vector2Int trackGenerationGrid { get; private set; }

    [field: SerializeField]
    public int mountainCount { get; private set; }
}

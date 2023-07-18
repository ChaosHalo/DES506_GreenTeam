using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncidentEffectData", menuName = "Data/IncidentEffectData", order = 0)]
public class IncidentEffectScriptableObject : ScriptableObject
{
    public enum Type { Reduce, Increase }
    public Type type;
    [Range(0f, 1f)]
    public float Value;
}

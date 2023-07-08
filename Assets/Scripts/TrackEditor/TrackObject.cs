using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour
{
    public enum Type { NONE, Straight, Curve}

    [SerializeField]
    public Type trackType;
    public Type GetTerrainType() { return trackType; }
}

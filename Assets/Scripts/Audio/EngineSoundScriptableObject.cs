using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EngineSoundData", menuName = "Data/EngineSoundData")]
public class EngineSoundScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct RPM 
    {
        public float Min;
        public float Max; 
    }
    [System.Serializable]
    public struct Load
    {
        public float Min;
        public float Max;
    }

    public RPM rPM;
    public Load load;
}

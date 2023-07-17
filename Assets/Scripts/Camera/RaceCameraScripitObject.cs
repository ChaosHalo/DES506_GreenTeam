using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RaceCameraData", menuName = "Data/RaceCameraData", order = 0)]
public class RaceCameraScripitObject : ScriptableObject
{
    public List<CameraData> cameraDatas;
}

[System.Serializable]
public class CameraData
{
    public enum CameraTypes
    {
        OverheadView,
        LeftView,
        RightView,
        RearView,
        FrontView
    }
    public CameraTypes cameraType;
    public Vector3 FollowOffset;
}
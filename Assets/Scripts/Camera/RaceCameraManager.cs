using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Lovatto.MiniMap;

public static class RaceCameraManager
{
    public static void SwitchCamera(GameObject camera, Vector3 followOffset, float fov)
    {
        CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
        CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        //transposer.m_FollowOffset = followOffset;
        virtualCamera.m_Lens.FieldOfView = fov;
    }
    public static void SetTarget(GameObject camera, Transform target)
    {
        CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.m_Follow = target;
        virtualCamera.m_LookAt = target;
    }

    public static void SetFOV(GameObject camera, float fov)
    {
        CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.m_Lens.FieldOfView = fov;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public static class RaceCameraManager
{
    public static void SwitchCamera(GameObject camera, Vector3 followOffset)
    {
        CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
        CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset = followOffset;
    }
    public static void SetTarget(GameObject camera, Transform target)
    {
        CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.m_Follow = target;
        virtualCamera.m_LookAt = target;
    }
}

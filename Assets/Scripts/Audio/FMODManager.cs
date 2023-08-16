using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FMODManager : Singleton<FMODManager>
{
    public EngineSoundScriptableObject sound;
    private CinemachineBrain cinemachineBrain;
    private bool FMODOn = true;
    private void Start()
    {
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateListenerPosition();
    }
    private void UpdateListenerPosition()
    {
        // 检查是否有激活的Virtual Camera
        if (cinemachineBrain != null && cinemachineBrain.ActiveVirtualCamera != null)
        {
            // 获取当前激活的CinemachineVirtualCamera
            CinemachineVirtualCamera activeVirtualCamera = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

            if (activeVirtualCamera != null)
            {
                if (activeVirtualCamera.Follow == null) return;
                transform.position = activeVirtualCamera.Follow.position;
            }
        }
    }
    public void TurnOnAllCarEngineSound()
    {
        FMODCarController[] fMODCarControllers = FindObjectsOfType<FMODCarController>();
        foreach (var i in fMODCarControllers)
        {
            i.TurnOnSound();
        }
    }
    public void TurnOffAllCarEngineSound()
    {
        FMODCarController[] fMODCarControllers = FindObjectsOfType<FMODCarController>();
        foreach (var i in fMODCarControllers)
        {
            i.TurnOffSound();
        }
    }
    public bool IsFMODOn() => FMODOn;
}

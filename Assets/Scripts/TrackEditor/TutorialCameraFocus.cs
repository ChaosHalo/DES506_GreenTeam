using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCameraFocus : MonoBehaviour
{
    [SerializeField] private GameObject camera;

    private void Start()
    {
        FindObjectOfType<CameraManager>().cameraTemporary = camera;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPiece : MonoBehaviour
{
    [SerializeField] private GameObject focusCamera;

    private void Start()
    {
        FindObjectOfType<CameraManager>().cameraTemporary = focusCamera;
    }
}

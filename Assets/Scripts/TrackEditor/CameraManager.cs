using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineComponentBase componentBase;


    [SerializeField]
    private float minZoom;
    [SerializeField]
    private float maxZoom;

    [SerializeField]
    float zoomSpeed = 10f;

    private float cameraDistance;

    private bool isMouseDown = false;
    private Vector3 mouseDown;
    private Vector3 anchorStartPos;

    private void Start()
    {
        anchorStartPos = transform.position;

        cameraDistance = 700;
    }

    private void Update()
    {
        if (componentBase == null)
        {
            componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }
        if (Input.mouseScrollDelta.y != 0)
        {

            if (componentBase is CinemachineFramingTransposer)
            {
                cameraDistance = Mathf.Clamp((componentBase as CinemachineFramingTransposer).m_CameraDistance -= Input.mouseScrollDelta.y * zoomSpeed, minZoom, maxZoom);
                (componentBase as CinemachineFramingTransposer).m_CameraDistance = cameraDistance;
            }
        }
    }

    internal void UpdatePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            mouseDown = Input.mousePosition;
            anchorStartPos = transform.position;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            anchorStartPos = transform.position;
        }

        Vector3 anchorNewPos = anchorStartPos;

        if (isMouseDown)
        {
            float modifiedCameraSpeed = Mathf.Sin(cameraDistance / maxZoom);
            float mouseDistanceX = mouseDown.x - Input.mousePosition.x;
            float mouseDistanceY = mouseDown.y - Input.mousePosition.y;
            anchorNewPos.x += mouseDistanceX * modifiedCameraSpeed;
            anchorNewPos.z += mouseDistanceY * modifiedCameraSpeed;

            transform.position = anchorNewPos;
        }

    }
}

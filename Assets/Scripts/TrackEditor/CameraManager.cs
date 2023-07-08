using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject anchor;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private PlacementSystem placementSystem;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineComponentBase componentBase;

    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] float zoomSpeed = 10f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private float cameraDistance;
    private bool isMouseDown = false;
    private Vector3 mouseDown;
    private Vector3 anchorStartPos;
    private Vector3 anchorOriginalPos;
    private Vector3 anchorCurPos;
    private Vector3 camPos;

    internal bool isPanning = false;

    private void Start()
    {
        anchorOriginalPos= anchor.transform.position;
        anchorStartPos = anchor.transform.position;
        anchorCurPos = anchor.transform.position;
        cameraDistance = maxZoom;
        camPos=virtualCamera.transform.position;
    }

    private void Update()
    {
        if (componentBase == null)
            componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);

       // if (placementSystem.buildingState == null)
        {
            UpdateCameraZoom();
            UpdateCameraPan();
        }
    }

    private void UpdateCameraZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            if (componentBase is CinemachineFramingTransposer)
            {
                cameraDistance = Mathf.Clamp((componentBase as CinemachineFramingTransposer).m_CameraDistance -= Input.mouseScrollDelta.y * zoomSpeed, minZoom, maxZoom);
                ApplyZoomToCamera(cameraDistance);
            }
        }
    }

    private void ApplyZoomToCamera(float zoom)
    {
        (componentBase as CinemachineFramingTransposer).m_CameraDistance = zoom;
        ClampAndSetCameraPosition();
    }

    private void UpdateCameraPan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (inputManager.IsPointerOverUI() == false)
            {
                isMouseDown = true;
                mouseDown = Input.mousePosition;
                anchorStartPos = anchor.transform.position;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            anchorStartPos = anchor.transform.position;
        }

        anchorCurPos = anchorStartPos;

        if (isMouseDown)
        {
            float scalingValue = Mathf.Sin(cameraDistance / maxZoom);
            float mouseDistanceX = mouseDown.x - Input.mousePosition.x;
            float mouseDistanceY = mouseDown.y - Input.mousePosition.y;
            anchorCurPos.x += mouseDistanceX * scalingValue;
            anchorCurPos.z += mouseDistanceY * scalingValue;
            ClampAndSetCameraPosition();
        }

        // camera is panning if mouse down does not equal mouse current
        float panDistance = Vector3.Distance(mouseDown, Input.mousePosition);
        isPanning = panDistance > 5;
    }

    private void ClampAndSetCameraPosition()
    {
        float scalingX = 1f - Mathf.Sin(cameraDistance / maxZoom);
        float scalingZ = cameraDistance / maxZoom;
        anchorCurPos.x = Mathf.Clamp(anchorCurPos.x, minBounds.x * scalingX, maxBounds.x * scalingX);
        anchorCurPos.z = Mathf.Clamp(anchorCurPos.z, minBounds.y * 1 - scalingZ, maxBounds.y * 1 - scalingZ);
        anchor.transform.position = anchorCurPos;
    }

    public void ResetCamera()
    {
        isMouseDown = false;
        cameraDistance = maxZoom;
        ApplyZoomToCamera(cameraDistance);
        anchorStartPos = anchorOriginalPos;
        anchorCurPos = anchorOriginalPos;
        virtualCamera.transform.position = camPos;
        anchor.transform.position = anchorOriginalPos;
    }
}

using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] private float startZoom;
    [SerializeField] float zoomSpeed = 1f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private float cameraDistance;
    private Vector3 anchorStartPos;
    private Vector3 anchorOriginalPos;
    private Vector3 anchorCurPos;
    private Vector3 camPos;

    internal bool isPanning = false;
    bool isZooming = false;

    // windows specific
    private bool isMouseDown = false;
    private Vector3 mouseDown;

    private void Start()
    {
        anchorOriginalPos= anchor.transform.position;
        anchorStartPos = anchor.transform.position;
        anchorCurPos = anchor.transform.position;
        cameraDistance = startZoom;
        camPos=virtualCamera.transform.position;
    }

    private void Update()
    {
        if (componentBase == null)
            componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);

        isZooming = false;

        PanCameraWindows();
    }

    private bool CanUpdateCamera()
    {
        if (placementSystem.buildingState == null)
        {
            return true;
        }
        else if (placementSystem.buildingState.GetType() != typeof(State_PlaceTerrain) && placementSystem.buildingState.GetType() != typeof(State_PlaceTrack))
        {
            return true;
        }
        return false;
    }

    internal void ZoomCamera(float offset)
    {
        if (offset == 0 || CanUpdateCamera() == false)
            return;

        if (componentBase is CinemachineFramingTransposer)
        {
            isZooming = true;
            cameraDistance = Mathf.Clamp((componentBase as CinemachineFramingTransposer).m_CameraDistance - offset * zoomSpeed, minZoom, maxZoom);
            ApplyZoomToCamera(cameraDistance);
        }
    }

    private void ApplyZoomToCamera(float zoom)
    {
        (componentBase as CinemachineFramingTransposer).m_CameraDistance = zoom;
        ClampAndSetCameraPosition();
    }

    internal void PanCamera()
    {
        if (CanUpdateCamera() == false)
            return;

        anchorCurPos = anchorStartPos;
        float panSpeed = Mathf.Sin(cameraDistance / maxZoom);
        float mouseDistanceX = inputManager.posMouseDown.x - inputManager.posMouseCur.x;
        float mouseDistanceY = inputManager.posMouseDown.y - inputManager.posMouseCur.y;

        // limit max panning speed
        panSpeed = Mathf.Clamp(panSpeed, 0f, 0.5f);

        // lower panning speed while zooming
        if (isZooming && panSpeed > 0.2f)
            panSpeed = 0.2f;

        anchorCurPos.x += mouseDistanceX * panSpeed;
        anchorCurPos.z += mouseDistanceY * panSpeed;
        ClampAndSetCameraPosition();

        // camera is panning if mouse down does not equal mouse current
        float panDistance = Vector3.Distance(inputManager.posMouseDown, inputManager.posMouseCur);
        isPanning = panDistance > 5;
    }

    internal void PanCameraWindows()
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


    public void OnCameraPanTap()
    {
        if (inputManager.IsPointerOverUI() == false)
        {
            anchorStartPos = anchor.transform.position;
        }
    }
    public void OnCameraPanRelease()
    {
        anchorStartPos = anchor.transform.position;
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
        if (componentBase is CinemachineFramingTransposer)
        {
            cameraDistance = startZoom;
            ApplyZoomToCamera(cameraDistance);
            anchorStartPos = anchorOriginalPos;
            anchorCurPos = anchorOriginalPos;
            virtualCamera.transform.position = camPos;
            anchor.transform.position = anchorOriginalPos;
        }
    }
}

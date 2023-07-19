using MoreMountains.HighroadEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placeableLayermask;

    [SerializeField]
    private LayerMask gridLayermask;

    public event Action OnTap, OnRelease, OnExit, OnHold;

    internal bool isWithinPlacementBounds { get; private set; }

    public CameraManager cameraManager;

    [SerializeField]
    public Vector3 gridWorldPos;

    [SerializeField]
    public Vector3Int gridCellPos;

    [SerializeField]
    public Vector3 mouseWorldPos;


    // android specific
    internal Vector3 posMouseDown;
    internal Vector3 posMouseUp;
    internal Vector3 posMouseCur;
    private Vector2[] lastZoomPositions;

    internal bool wasZoomingLastFrame = false;
    bool firstFingerLifted = false;

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.isMobilePlatform)
            CheckAndroidInput();
        else
            CheckWindowsInput();

        //Debug.Log(mouseWorldPos + " ::::: " + gridWorldPos);
    }


    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();


    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = posMouseCur;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        // is cursor over world grid? Snap to grid if true
        if (Physics.Raycast(ray, out hit, 2000, gridLayermask))
        {
            lastPosition = hit.point;

            // is cursor over placable grid? allow placement if true
            isWithinPlacementBounds = Physics.Raycast(ray, out hit, 2000, placeableLayermask) ? true : false;
        }

        return lastPosition;
    }


    #region ANDROID
    private void CheckAndroidInput()
    {
        switch (Input.touchCount)
        {
            case 1:
                wasZoomingLastFrame = false;
                HandleOneTouch(Input.GetTouch(0));
                break;
            case 2:
                HandleOneTouch(Input.GetTouch(0));
                HandleTwoTouch(Input.GetTouch(0), Input.GetTouch(1));
                break;
            default:
                cameraManager.OnCameraPanRelease();
                wasZoomingLastFrame = false;
                firstFingerLifted = false;
                break;
        }
    }

    private void HandleOneTouch(Touch touch)
    {
        if (firstFingerLifted == true)
            return;

        if (touch.phase == TouchPhase.Began)
        {
            posMouseDown = touch.position;
            posMouseCur = touch.position;
            OnTap?.Invoke();
        }
        if (touch.phase == TouchPhase.Moved)
        {
            posMouseCur = touch.position;
            cameraManager.PanCamera();
        }
        if (touch.phase == TouchPhase.Ended)
        {
            posMouseUp = touch.position;
            firstFingerLifted = true;
            OnRelease?.Invoke();
        }
    }


    private void HandleTwoTouch(Touch touch1, Touch touch2)
    {
        Vector2[] newPositions = new Vector2[] { touch1.position, touch2.position };
        if (!wasZoomingLastFrame)
        {
            lastZoomPositions = newPositions;
            wasZoomingLastFrame = true;
        }
        else
        {
            float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
            float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
            float offset = (newDistance - oldDistance) * 0.75f;
            float maxOffset = 15;
            offset= Mathf.Clamp(offset, -maxOffset, maxOffset);  

            cameraManager.ZoomCamera(offset);

            lastZoomPositions = newPositions;
        }
    }
    #endregion



    #region WINDOWS
    private void CheckWindowsInput()
    {
        if (Input.GetMouseButtonDown(0))
            HandleClickDown();
        if (Input.GetMouseButtonUp(0))
            HandleClickUp();
        if (Input.GetMouseButton(0))
            HandleClickHold();

        HandleScroll(Input.mouseScrollDelta.y);
    }
    private void HandleClickDown()
    {
        posMouseDown = Input.mousePosition;
        posMouseCur = Input.mousePosition;
        OnTap?.Invoke();
    }

    private void HandleClickUp()
    {
        posMouseUp = Input.mousePosition;
        posMouseCur = Input.mousePosition;
        OnRelease?.Invoke();
    }

    private void HandleClickHold()
    {
        posMouseCur = Input.mousePosition;
        cameraManager.PanCamera();
    }

    private void HandleScroll(float delta)
    {
        if (delta == 0)
            return;

        cameraManager.ZoomCamera(delta * 100);
    }
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
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

    public event Action OnTap, OnRelease, OnExit;

    internal bool isWithinPlacementBounds { get; private set; }


    [SerializeField]
    public Vector3 gridWorldPos;

    [SerializeField]
    public Vector3 mouseWorldPos;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnTap?.Invoke();
        if (Input.GetMouseButtonUp(0))
            OnRelease?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();

        //Debug.Log(mouseWorldPos + " ::::: " + gridWorldPos);
    }


    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();


    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
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
}

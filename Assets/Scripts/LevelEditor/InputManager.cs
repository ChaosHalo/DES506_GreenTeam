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
    private LayerMask placementLayermask;

    [SerializeField]
    private LayerMask worldLayermask;

    public event Action OnTap, OnRelease, OnExit;

    internal bool IsOverGrid { get; private set; }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnTap?.Invoke();
        if (Input.GetMouseButtonUp(0))
            OnRelease?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }


    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();


    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
            IsOverGrid = true;
        }
        else
        {
            IsOverGrid = false;
        }

        if (Physics.Raycast(ray, out hit, 100, worldLayermask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}

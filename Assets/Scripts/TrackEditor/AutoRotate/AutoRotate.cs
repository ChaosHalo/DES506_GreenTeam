using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField]
    internal PlacableObject placableObject;
    internal bool mouseInRightHalf = false;
    internal bool mouseInBottomHalf = false;
    internal Vector3 lastMousePos = Vector3.zero;
    internal Vector3 mouseWorldPos = Vector3.zero;
    internal Vector3 gridWorldPos = Vector3.zero;
    internal bool connectionLeft = false;
    internal bool connectionRight = false;
    internal bool connectionDown = false;
    internal bool connectionUp = false;

    [SerializeField]
    private int rotationState = 0;
    internal int GetRotationState() { return rotationState; }
    [SerializeField]
    internal List<Vector3> rotations = new List<Vector3>();
    [SerializeField]
    private List<Vector3> connectionDirections = new();
    private List<Vector3> connectionDirectionsAdjusted = new();
    [SerializeField]
    private float connectionDistanceMulti = 1;
    [SerializeField]
    private GameObject rayCastOrigin;

    private void Awake()
    {
        AdjustConnectionDirectionsForRotationState(rotationState);
    }

    private void FixedUpdate()
    {
        if (placableObject.canScale == false)
            return;
        if (lastMousePos == Input.mousePosition)
            return;

        DoConnectivityRaycasts();
        UpdateMouseAxes();
        UpdatePlacementRotation();

        // update last pos
        lastMousePos = Input.mousePosition;
    }

    void DoConnectivityRaycasts()
    {
        Vector3 originPos = placableObject.inputManager.gridWorldPos;
        originPos.x += 50;
        originPos.z += 50;

        connectionLeft = RayCastCheck(new(-1, 0, 0), originPos, 1);
        connectionRight = RayCastCheck(new(1, 0, 0), originPos, 1);
        connectionDown = RayCastCheck(new(0, 0, -1), originPos, 1);
        connectionUp = RayCastCheck(new(0, 0, 1), originPos, 1);
    }

    private void UpdateMouseAxes()
    {
        // check which half of grid cell is mouse
        mouseWorldPos = placableObject.inputManager.mouseWorldPos;
        gridWorldPos = placableObject.inputManager.gridWorldPos;

        // adjust for half grid cell size
        mouseWorldPos.x -= 50;
        mouseWorldPos.z -= 50;

        // horisontal
        if (gridWorldPos.x - mouseWorldPos.x < 0)
            mouseInRightHalf = true;
        else
            mouseInRightHalf = false;

        // vertical
        if (gridWorldPos.z - mouseWorldPos.z < 0)
            mouseInBottomHalf = true;
        else
            mouseInBottomHalf = false;
    }

    private void UpdatePlacementRotation()
    {
        SetRotationState(CalculateAutoRotationState());
    }

    internal virtual int CalculateAutoRotationState()
    {
        return -1;
    }

    internal void SetRotationState(int state)
    {
        if (state == -1)
            return;

        rotationState = state;
        transform.parent.localRotation = Quaternion.Euler(rotations[state]);
        AdjustConnectionDirectionsForRotationState(state);

    }

    private void AdjustConnectionDirectionsForRotationState(int state)
    {
        connectionDirectionsAdjusted.Clear();
        foreach (Vector3 direction in connectionDirections)
        {
            float newX = 0;
            float newZ = 0;

            switch (state)
            {
                case 0:
                    newX = direction.x;
                    newZ = direction.z;
                    break;
                case 1:
                    newX = direction.z;
                    newZ = -direction.x;
                    break;
                case 2:
                    newX = -direction.x;
                    newZ = -direction.z;
                    break;
                case 3:
                    newX = -direction.z;
                    newZ = direction.x;
                    break;
            }

            connectionDirectionsAdjusted.Add(new(newX, 0, newZ));
        }
    }

    internal bool RayCastCheck(Vector3 direction, Vector3 originPos, float distanceMulti)
    {
        // offset origin point in direction of check
        Vector3 originOffset = direction * 45;

        // adjust origin point and distance by connection distance multiplier
        originOffset *= distanceMulti;
        float distance = 40;

        // do raycast
        RaycastHit[] hits = Physics.RaycastAll(originPos + originOffset, direction, distance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.transform != this.transform.parent)
                {
                    if (hit.transform.CompareTag("RotationHitbox"))
                    {
                        Debug.Log(hit.transform + ":::::" + this.transform.parent);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    internal bool CheckForConnections()
    {
        bool isConnected = true;

        for (int i = 0; i < connectionDirectionsAdjusted.Count; i++)
        {
            if (RayCastCheck(connectionDirectionsAdjusted[i], rayCastOrigin.transform.position, connectionDistanceMulti) == false)
            {
                isConnected = false;
            }
        }

        return isConnected;
    }

}

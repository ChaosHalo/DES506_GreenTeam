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
    internal bool connectionLeft =false;
    internal bool connectionRight = false;
    internal bool connectionDown = false;
    internal bool connectionUp = false;

    [SerializeField]
    private int rotationState = 0;
    internal int GetRotationState() { return rotationState; }
    [SerializeField]
    internal List<Vector3> rotations = new List<Vector3>();

    private void FixedUpdate()
    {
        if (placableObject.canScale == false)
            return;
        if (lastMousePos == Input.mousePosition)
            return;

        RayCastChecks();
        UpdateMouseAxes();
        UpdatePlacementRotation();

        // update last pos
        lastMousePos = Input.mousePosition;
    }

    void RayCastChecks()
    {
        connectionLeft = RayCastCheck(new(-1, 0, 0));
        connectionRight = RayCastCheck(new(1, 0, 0));
        connectionDown = RayCastCheck(new(0, 0, -1));
        connectionUp = RayCastCheck(new(0, 0, 1));
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
    }

    bool RayCastCheck(Vector3 direction)
    {
        Vector3 startPos = placableObject.inputManager.gridWorldPos;
        startPos.x += 50;
        startPos.z += 50;

        RaycastHit[] hits = Physics.RaycastAll(startPos + direction * 40, direction, 60);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.transform.CompareTag("RotationHitbox"))
                {
                    return true;
                }
            }
        }
        return false;
    }

}

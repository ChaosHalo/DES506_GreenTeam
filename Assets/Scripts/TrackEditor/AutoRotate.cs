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
        Vector3 newRotation = new(-1, -1, -1);
        Vector3 nullRotation = new(-1, -1, -1);

        newRotation = GetTrackRotation();

        if (newRotation != nullRotation)
            transform.parent.localRotation = Quaternion.Euler(newRotation);
    }

    internal virtual Vector3 GetTrackRotation()
    {
        return new Vector3(-1, -1, -1);
    }

    bool RayCastCheck(Vector3 direction)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.parent.position + direction * 40, direction, 60);
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

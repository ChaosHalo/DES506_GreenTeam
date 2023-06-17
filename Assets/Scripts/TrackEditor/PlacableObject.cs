using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class PlacableObject : MonoBehaviour
{
    private Vector3 lastMousePos = Vector3.zero;
    private int mouseVertical = 1;
    private int mouseHorisontal = 1;

    [SerializeField]
    private string trackPieceType;

    private bool canScale = true;
    private float scaleSpeed = 0.01f;

    // scale presets
    private float multiLarge = 1.2f;
    private float multiSmall = 1.0f;
    private Vector3 scaleSmall;
    private Vector3 scaleLarge;
    private Vector3 scaleNext;
    private Vector3 scaleOriginal;

    private void Awake()
    {
        scaleOriginal = transform.localScale;
        scaleSmall = scaleOriginal * multiSmall;
        scaleSmall.z = scaleOriginal.z;
        scaleLarge = scaleOriginal * multiLarge;
        scaleLarge.z = scaleOriginal.z;
        scaleNext = scaleLarge;
    }

    private void Update()
    {
        UpdateMouseAxes();
    }

    private void FixedUpdate()
    {
        UpdateScale();
        UpdatePlacementRotation();
    }

    private void UpdateMouseAxes()
    {
        if (canScale == false)
            return;

        if (lastMousePos != Input.mousePosition)
        {
            // vertical
            if (lastMousePos.y > Input.mousePosition.y)
                mouseVertical = 1;
            else if (lastMousePos.y < Input.mousePosition.y)
                mouseVertical = -1;

            // horisontal
            if (lastMousePos.x > Input.mousePosition.x)
                mouseHorisontal = 1;
            else if (lastMousePos.x < Input.mousePosition.x)
                mouseHorisontal = -1;

            // update last pos
            lastMousePos = Input.mousePosition;
        }
    }

    internal virtual void UpdateScale()
    {
        if (canScale == false)
            return;

        transform.localScale = Vector3.MoveTowards(transform.localScale, scaleNext, scaleSpeed * Time.deltaTime);

        if (transform.localScale == scaleLarge)
            scaleNext = scaleSmall;
        if (transform.localScale == scaleSmall)
            scaleNext = scaleLarge;
    }

    internal void StopScaling()
    {
        canScale = false;
        transform.localScale = scaleOriginal;
    }





    private void UpdatePlacementRotation()
    {
        if (canScale == false)
            return;

        Vector3 newRotation = new(-1, -1, -1);
        Vector3 nullRotation = new(-1, -1, -1);

        if (trackPieceType == "Straight")
            newRotation = RotationsStraight();

        if (trackPieceType == "Curve")
            newRotation = RotationsCurve();

        if (newRotation != nullRotation)
            transform.parent.localRotation = Quaternion.Euler(newRotation);
    }

    private Vector3 RotationsCurve()
    {
        Vector3 rot = new(-1, -1, -1);

        bool left = false;
        bool right = false;

        //LEFT
        if (RayCastCheck(new(-1, 0, 0)))
        {
            left = true;

            // connect up
            if (mouseVertical == 1)
                rot = new(0, 180, 0);
            // connect down
            else
                rot = new(0, -90, 0);
        }

        // RIGHT
        if (RayCastCheck(new(1, 0, 0)))
        {
            right = true;

            // connect up
            if (mouseVertical == 1)
                rot = new(0, 90, 0);
            // connect down
            else
                rot = new(0, 0, 0);
        }

        // DOWN
        if (RayCastCheck(new(0, 0, -1)))
        {
            // down-left corner
            if (left && !right)
                rot = new(0, 180, 0);
            // down-right corner
            else if (!left && right)
                rot = new(0, 90, 0);
            // connect right
            else if (mouseHorisontal == 1)
                rot = new(0, 180, 0);
            // connect left
            else
                rot = new(0, 90, 0);
        }

        // UP
        if (RayCastCheck(new(0, 0, 1)))
        {
            // down-left corner
            if (left && !right)
                rot = new(0, -90, 0);
            // down-right corner
            else if (!left && right)
                rot = new(0, 0, 0);
            // connect right
            else if (mouseHorisontal == 1)
                rot = new(0, -90, 0);
            // connect left
            else
                rot = new(0, 0, 0);
        }

        return rot;
    }

    private Vector3 RotationsStraight()
    {
        Vector3 rot = new(-1, -1, -1);

        //LEFT
        if (RayCastCheck(new(-1, 0, 0)))
            rot = new(0, 0, 0);

        // DOWN
        if (RayCastCheck(new(0, 0, -1)))
            rot = new(0, 90, 0);

        // RIGHT
        if (RayCastCheck(new(1, 0, 0)))
            rot = new(0, 0, 0);

        // UP
        if (RayCastCheck(new(0, 0, 1)))
            rot = new(0, 90, 0);

        return rot;
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

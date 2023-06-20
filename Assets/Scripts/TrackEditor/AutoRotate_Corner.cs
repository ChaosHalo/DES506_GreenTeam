using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate_Corner : AutoRotate
{
    internal override Vector3 GetTrackRotation()
    {
        Vector3 rot = new(-1, -1, -1);

        //LEFT
        if (connectionLeft)
        {
            if (mouseInBottomHalf)
                rot = rotations[0];
            else
                rot = rotations[1];
        }

        // RIGHT
        if (connectionRight)
        {
            if (mouseInBottomHalf)
                rot = rotations[3];
            else
                rot = rotations[2];
        }

        // DOWN
        if (connectionDown)
        {
            // down-left corner
            if (connectionLeft && !connectionRight)
                rot = rotations[1];
            // down-right corner
            else if (!connectionLeft && connectionRight)
                rot = rotations[2];
            else if (mouseInRightHalf)
                rot = rotations[2];
            else
                rot = rotations[1];
        }

        // UP
        if (connectionUp)
        {
            // down-left corner
            if (connectionLeft && !connectionRight)
                rot = rotations[0];
            // down-right corner
            else if (!connectionLeft && connectionRight)
                rot = rotations[3];
            else if (mouseInRightHalf)
                rot = rotations[3];
            else
                rot = rotations[0];
        }

        return rot;
    }
}

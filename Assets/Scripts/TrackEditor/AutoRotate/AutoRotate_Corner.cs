using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate_Corner : AutoRotate
{
    internal override int CalculateAutoRotationState()
    {
        int state = -1;

        //LEFT
        if (connectionLeft)
        {
            if (mouseInBottomHalf)
                state = 3;
            else
                state = 2;
        }

        // RIGHT
        if (connectionRight)
        {
            if (mouseInBottomHalf)
                state = 0;
            else
                state = 1;
        }

        // DOWN
        if (connectionDown)
        {
            // down-left corner
            if (connectionLeft && !connectionRight)
                state = 2;
            // down-right corner
            else if (!connectionLeft && connectionRight)
                state = 1;
            else if (mouseInRightHalf)
                state = 1;
            else
                state = 2;
        }

        // UP
        if (connectionUp)
        {
            // down-left corner
            if (connectionLeft && !connectionRight)
                state = 3;
            // down-right corner
            else if (!connectionLeft && connectionRight)
                state = 0;
            else if (mouseInRightHalf)
                state = 0;
            else
                state = 3;
        }

        return state;
    }
}

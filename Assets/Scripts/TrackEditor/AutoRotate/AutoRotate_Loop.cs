using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate_Loop : AutoRotate
{
    internal override int CalculateAutoRotationState()
    {
        int state = -1;

        //LEFT
        if (connectionLeft)
        {
            state = 2;
        }

        // RIGHT
        if (connectionRight)
        {
            state = 0;
        }

        // UP
        if (connectionUp)
        {
            state = 3;
        }

        // DOWN
        if (connectionDown)
        {
            state = 1;
        }


        // CORNER
        if (connectionDown || connectionUp)
        {
            if (connectionLeft || connectionRight)
            {
                // calculate distances to edges
                float distanceToUp = 999;
                float distanceToDown = 999;
                float distanceToLeft = 999;
                float distanceToRight = 999;
                Vector3 gridAdjusted = gridWorldPos;

                if (connectionLeft)
                {
                    gridAdjusted = gridWorldPos;
                    gridAdjusted.x -= 50;
                    distanceToLeft = Vector3.Distance(mouseWorldPos, gridAdjusted);
                }
                if (connectionRight)
                {
                    gridAdjusted = gridWorldPos;
                    gridAdjusted.x += 50;
                    distanceToRight = Vector3.Distance(mouseWorldPos, gridAdjusted);
                }
                if (connectionUp)
                {
                    gridAdjusted = gridWorldPos;
                    gridAdjusted.z += 50;
                    distanceToUp = Vector3.Distance(mouseWorldPos, gridAdjusted);
                }
                if (connectionDown)
                {
                    gridAdjusted = gridWorldPos;
                    gridAdjusted.z -= 50;
                    distanceToDown = Vector3.Distance(mouseWorldPos, gridAdjusted);
                }

                // find shortest distance to an edge
                float shortestDistance = Mathf.Min(distanceToUp, distanceToDown, distanceToLeft, distanceToRight);

                // apply rotation in direction of shortest distance
                if (shortestDistance == distanceToLeft)
                    state = 2;
                else if (shortestDistance == distanceToRight)
                    state = 0;
                else if (shortestDistance == distanceToUp)
                    state = 3;
                else if (shortestDistance == distanceToDown)
                    state = 1;
            }
        }

        return state;
    }
}

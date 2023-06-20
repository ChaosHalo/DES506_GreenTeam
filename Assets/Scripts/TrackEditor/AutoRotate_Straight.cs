using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class AutoRotate_Straight : AutoRotate
{
    internal override Vector3 GetTrackRotation()
    {
        Vector3 rot = new(-1, -1, -1);

        //LEFT
        if (connectionLeft)
        {
            rot = rotations[0];
        }

        // RIGHT
        if (connectionRight)
        {
            rot = rotations[1];
        }

        // UP
        if (connectionUp)
        {
            rot = rotations[3];
        }

        // DOWN
        if (connectionDown)
        {
            rot = rotations[2];
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
                    rot = rotations[0];
                else if (shortestDistance == distanceToRight)
                    rot = rotations[1];
                else if (shortestDistance == distanceToUp)
                    rot = rotations[2];
                else if (shortestDistance == distanceToDown)
                    rot = rotations[3];
            }
        }

        return rot;
    }
}

using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyAIPath : AIPath
{
    protected override void MovementUpdateInternal(float deltaTime,
                                                   out Vector3 nextPosition,
                                                   out Quaternion nextRotation)
    {
        // TODO(Jonas): Override so that movement is based on accelleration, not 
        // teleportation
    
        if (updatePosition) {
            // Get our current position. We read from transform.position as few times as possible as it is relatively slow
            // (at least compared to a local variable)
            simulatedPosition = tr.position;
        }
        if (updateRotation) { 
            simulatedRotation = tr.rotation;
        }
        var currentPosition = simulatedPosition;
        // Update which point we are moving towards
        interpolator.MoveToCircleIntersection2D(currentPosition, pickNextWaypointDist, movementPlane);
        var dir = movementPlane.ToPlane(steeringTarget - currentPosition);
        
        
        nextPosition = new Vector3(0,0,0);
        nextRotation = new Quaternion(0,0,0,0);
        Debug.LogWarning("Test");
        // base.MovementUpdateInternal(deltaTime, out nextPosition, out nextRotation);
    
    }

    public override void OnTargetReached()
    {

        // TODO(Jonas): Do something
    }
}

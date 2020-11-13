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
         
        // base.MovementUpdateInternal(deltaTime, out nextPosition, out nextRotation);
    
    }
}

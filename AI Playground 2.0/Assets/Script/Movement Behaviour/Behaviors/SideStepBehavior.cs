using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Behaviour/SideStep")]
public class SideStepBehavior : MovementBehaviour
{
    public float senseRadius = 4f;
    public float effectivenessRange = 6f;
    public LayerMask avoidanceLayer;
    public LayerMask obstacleLayer;

    public override Vector2 CalculateDirection(MovementModule movementModule)
    {
        // Detect surrounding environment and put it in an array
        Collider2D[] colliders = Physics2D.OverlapCircleAll(movementModule.transform.position, senseRadius, avoidanceLayer);
        // Check individually if the objects detected are within enitity's vision
        foreach(Collider2D col in colliders)
        {
            //Ignore itself
            if (col.gameObject == movementModule.gameObject) continue;

            //check if within vision
            bool hit = CheckIfWithinVision(movementModule.transform.position, col.transform.position, obstacleLayer);
            if(hit == false)
            {
                Vector2 desiredVec = movementModule.transform.position - col.transform.position;
                float distBetweenTarget = Vector2.Distance(movementModule.transform.position, movementModule.CurrentTargetPos);
                return Mathf.Abs(Vector2.Dot(movementModule.transform.right, movementModule.transform.position - col.transform.position)) * desiredVec * ((effectivenessRange-distBetweenTarget)/effectivenessRange);
                //if(CheckIfLeftOrRight(movementModule.transform, col.transform.position) > 0)
                //{
                //    Debug.DrawRay(movementModule.transform.position, (-movementModule.transform.right * (5 - Vector2.Distance(movementModule.transform.position, col.transform.position))) , Color.white);
                //    return -movementModule.transform.right * (5 - Vector2.Distance(movementModule.transform.position, col.transform.position));
                //}
                //else
                //{
                //    Debug.DrawRay(movementModule.transform.position, (movementModule.transform.right * (5 - Vector2.Distance(movementModule.transform.position, col.transform.position))), Color.white);
                //    return movementModule.transform.right * (5 - Vector2.Distance(movementModule.transform.position, col.transform.position));                    
                //}
            }
        }
        return Vector2.zero;
        // If within vision then return left or right vector accordingly
    }
}

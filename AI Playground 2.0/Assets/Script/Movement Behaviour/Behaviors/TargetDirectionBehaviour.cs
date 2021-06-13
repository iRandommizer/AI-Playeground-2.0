using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Behaviour/Target")]
public class TargetDirectionBehaviour : MovementBehaviour
{
    public Vector2 target;
    public float radiusThreshold = 5f;
    public override Vector2 CalculateDirection(MovementModule movementModule)
    {
        target = movementModule.CurrentTargetPos;   
        Vector2 direction = Vector2.zero;        
        Vector2 distance = target - ((Vector2)movementModule.transform.position + movementModule.rb.velocity.normalized * 0.3f * movementModule.rb.velocity.magnitude);
        //Debug.DrawLine(movementModule.transform.position, (Vector2)movementModule.transform.position + movementModule.rb.velocity.normalized * 0.3f * movementModule.rb.velocity.magnitude, Color.green);
        float t = distance.magnitude / radiusThreshold;
        if (t < 0.9f)
        {
            return Vector2.zero;
        }
        return (distance * t * t).normalized;
    }
}

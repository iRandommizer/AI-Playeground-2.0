using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Behaviour/CentralizedAvoidance")]
public class CentralizedAvoidance : MovementBehaviour
{
    public float avoidanceDistance = 3;
    public int numOfRays = 8;
    public LayerMask avoidanceLayer;
    public override Vector2 CalculateDirection(MovementModule movementModule)
    {
        float modifiedAvoidanceDistance;
        float t = 1;
        Vector2 undesiredDirection = Vector2.zero;
        float angle = 360 / numOfRays;
        for (int i = 0; i < numOfRays; i++)
        {
            Vector2 dir = RotateBy(movementModule.transform.up, i * angle);
            RaycastHit2D hit;
            Vector2 predictedPosition = (Vector2)movementModule.transform.position + (Vector2)(movementModule.rb.velocity.normalized * movementModule.rb.velocity.magnitude * 0.15f);
            modifiedAvoidanceDistance = avoidanceDistance * movementModule.rb.velocity.magnitude * 0.03f + avoidanceDistance/2;
            hit = Physics2D.Raycast(predictedPosition, dir.normalized, modifiedAvoidanceDistance, avoidanceLayer);
            if (hit)
            {
                if (hit.collider == movementModule.GetComponent<Collider2D>()) continue;
                //Debug.Log("hit");
                float dist = (hit.point - predictedPosition).magnitude;
                t = dist / modifiedAvoidanceDistance;

                if (t == 0)
                {
                    return Vector2.Perpendicular(movementModule.rb.velocity);
                }
                undesiredDirection += (hit.point - (Vector2)movementModule.transform.position);
                //Debug.DrawLine(predictedPosition, dir.normalized * modifiedAvoidanceDistance + predictedPosition, Color.red);
            }
            else
            {
                //Debug.DrawLine(predictedPosition, dir.normalized * modifiedAvoidanceDistance + predictedPosition, Color.cyan);
            }
        }
        //DrawArrow.ForDebug(movementModule.transform.position, undesiredDirection.normalized / t / t, Color.red);
        return (-undesiredDirection.normalized + movementModule.desiredDirection.normalized) / t / (t * 8 * ((movementModule.maxSpeed - movementModule.rb.velocity.magnitude) / movementModule.maxSpeed));
    }

    public Vector2 RotateBy(Vector2 v, float a, bool bUseRadians = false)
    {
        if (!bUseRadians) a *= Mathf.Deg2Rad;
        var ca = Mathf.Cos(a);
        var sa = Mathf.Sin(a);
        var rx = v.x * ca - v.y * sa;

        return new Vector2((float)rx, (float)(v.x * sa + v.y * ca));
    }
}

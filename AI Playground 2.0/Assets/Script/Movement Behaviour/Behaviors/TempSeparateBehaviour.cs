using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Behaviour/Separation")]
public class TempSeparateBehaviour : MovementBehaviour
{
    public float detectionSize = 3f;
    public LayerMask detectionLayer;
    Vector2 currentVel;

    public override Vector2 CalculateDirection(MovementModule movementModule)
    {
        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
        Collider2D[] cols = Physics2D.OverlapCircleAll(movementModule.transform.position, detectionSize, detectionLayer);
        foreach (Collider2D col in cols)
        {
            if(col.GetComponent<MovementModule>() != null && col.GetComponent<MovementModule>() != movementModule)
            {
                nAvoid++;
                avoidanceMove += (Vector2)(movementModule.transform.position - col.transform.position);
            }
        }
        if(nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }

        avoidanceMove = Vector2.SmoothDamp(movementModule.rb.velocity.normalized, avoidanceMove, ref currentVel, 0.5f);
        //DrawArrow.ForDebug(movementModule.transform.position, avoidanceMove,Color.red);
        return avoidanceMove.normalized;
    }
}

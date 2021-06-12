using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBehaviour : ScriptableObject
{
    public abstract Vector2 CalculateDirection(MovementModule movementModule);

    protected bool CheckIfWithinVision(Vector2 posA, Vector2 posB, LayerMask layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(posA, (posB - posA).normalized, Vector2.Distance(posA, posB), layerMask);
        return hit;
    }


    protected float CheckIfLeftOrRight(Transform target, Vector2 posB)
    {
        Debug.DrawLine(target.position, posB, Color.magenta);
        return Vector2.Dot(target.right, posB); // Greater than 0 means right side, Lesser than 0 means left side
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Movement/Behaviour/Random")]
public class RandomDirectionBehaviour : MovementBehaviour
{
    public override Vector2 CalculateDirection(MovementModule movementModule)
    {
        Vector2 direction = (Random.insideUnitCircle.normalized + movementModule.rb.velocity.normalized).normalized;
        return direction;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Behaviour/Composite")]
public class CompositeSteeringBehaviour : MovementBehaviour
{
    public List<MovementBehaviour> movementBehaviours;
    public List<float> movementWeights;
    public float steeringDampening; // To make the movement not jitter so much

    public override Vector2 CalculateDirection(MovementModule movementModule)
    {
        Vector2 direction = movementModule.rb.velocity.normalized * steeringDampening;
        for (int i = 0; i < movementBehaviours.Count; i++)
        {
            direction += movementBehaviours[i].CalculateDirection(movementModule) * movementWeights[i];
        }

        direction /= movementBehaviours.Count;

        return direction.normalized;
    }
}

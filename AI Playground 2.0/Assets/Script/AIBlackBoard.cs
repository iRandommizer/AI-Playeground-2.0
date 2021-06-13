using System.Collections.Generic;
using UnityEngine;

public class AIBlackBoard : BaseBlackBoard // might have to further breakdown this into its own subclass
{
    public MovementModule MovementModule { get; }// Reference to the movement module of the enemy 
    public FieldOfView Fow { get; } //!! Need to change it to Sensory System to compensate for multiple types of senses
    public Animator Animator { get; }
    public AIAgent Agent { get; } // Reference to the agent
    public List<Strategy> AvailableStrategies = new List<Strategy>();
    public AIBlackBoard(AIAgent agent, FieldOfView fow, Animator animator, MovementModule movementModule, List<Strategy> strategies)
    {
        Agent = agent;
        Fow = fow;
        Animator = animator;
        MovementModule = movementModule;
        AvailableStrategies = strategies;
    }
}

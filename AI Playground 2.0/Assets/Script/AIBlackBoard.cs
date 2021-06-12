using UnityEngine;

public class AIBlackBoard : BaseBlackBoard
{
    public MovementModule MovementModule { get; }// Reference to the movement module of the enemy 
    public FieldOfView Fow { get; } //!! Need to change it to Sensory System to compensate for multiple types of senses
    public Animator Animator { get; }
    public AIAgent Agent { get; } // Reference to the agent
    
    public AIBlackBoard(AIAgent agent, FieldOfView fow, Animator animator, MovementModule movementModule)
    {
        Agent = agent;
        Fow = fow;
        Animator = animator;
        MovementModule = movementModule;
    }
}

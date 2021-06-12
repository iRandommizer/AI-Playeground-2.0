using UnityEngine;

public class AIBlackBoard : BaseBlackBoard
{
    public MovementModule MovementModule { get; }// Reference to the movement module of the enemy 
    public FieldOfView Fow { get; } //!! Need to change it to Sensory System to compensate for multiple types of senses
    public Animator Animator { get; }
    public AIAgent Agent { get; } // Reference to the agent

    #region Helper Expressions
    public Vector2 Postion => Agent.transform.position;
    public Vector2 Forward => Agent.transform.up;
    public Rigidbody2D RB => MovementModule.rb;
    public Vector2 Velocity => RB.velocity;
    public float Magnitude => Velocity.magnitude;
    public Vector2 PosForwardx1 => Postion + (Forward * Magnitude * 0.01f);
    public Vector2 PosForwardx2 => Postion + (Forward * Magnitude * 0.02f);
    public Vector2 PosForwardx3 => Postion + (Forward * Magnitude * 0.04f);
    public Vector2 PosForwardx4 => Postion + (Forward * Magnitude * 0.08f);
    #endregion

    public AIBlackBoard(AIAgent agent, FieldOfView fow, Animator animator, MovementModule movementModule)
    {
        Agent = agent;
        Fow = fow;
        Animator = animator;
        MovementModule = movementModule;
    }
}

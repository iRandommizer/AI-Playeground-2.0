using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Strictly for holding information about the Herd. Very minimal processing happens in this class
/// </summary>
public class HerdAIBlackBoard : BaseBlackBoard
{
    #region Private Data
    [SerializeField] private float displacementPeriod = 4;
    #endregion

    #region Component Data

    public RequestSystem requestSystem;

    #endregion

    public List<AIAgent> AIAgents = new List<AIAgent>(); // Keeps track of all the agents within it's herd
    public List<AIAgent> enemyTargets = new List<AIAgent>(); // Need to consider that there could be more than 1 enemy
    public AIAgent CurrentTarget { get; set; }

    public float totalHealthOfHerd;
    public float totalHealthOfEnemies;
    public List<float> enemiesDPS = new List<float>(); // keeps track of damage of enemies per second
    public List<float> enemiesDisplacement = new List<float>(); // keeps track of displacement of enemies within a per x seconds

    public List<Vector2> prevDisplacementPos = new List<Vector2>(); 
    
    #region Helper Expressions
    public Vector2 CurTargetPos => CurrentTarget.transform.position;
    public Vector2 CurTargetForward => CurrentTarget.transform.up;
    #endregion
    
    public float Time { get; set; } // AI Context keeps track of it's own time
    public float DeltaTime { get; set; } // AI Context keeps track of it's own delta time
    public float GenericTimer { get; set; } // Temporary time keeper for some process like animation, etc

    public HerdAIBlackBoard(List<AIAgent> assignedAgents)
    {
        AIAgents = assignedAgents;

        requestSystem = new RequestSystem();
    }
}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIBlackBoard : BaseBlackBoard // might have to further breakdown this into its own subclass
{
    public MovementModule MovementModule { get; }// Reference to the movement module of the enemy 
    public BaseEnemy EnemyDecider { get; }
    public FieldOfView Fow { get; } //!! Need to change it to Sensory System to compensate for multiple types of senses
    public Animator Animator { get; }
    public AIAgent Agent { get; } // Reference to the agent
    public RequestHandler RequestHandler { get; set; }

    public float Time { get; set; } // AI Context keeps track of it's own time
    public float DeltaTime { get; set; } // AI Context keeps track of it's own delta time
    public float GenericTimer { get; set; } // Temporary time keeper for some process like animation, etc

    public List<EEffect> capableEffects = new List<EEffect>();
    
    public List<AIEntityState> EligableAIEntityState = new List<AIEntityState>() {AIEntityState.HasMultipleAttackAmmo}; // Make data for it
    public List<AIEntityStatePair> EntityStatePair { get; set; } = new List<AIEntityStatePair>();

    public AIBlackBoard(AIAgent agent, FieldOfView fow, Animator animator, MovementModule movementModule, BaseEnemy enemyDecider)
    {
        Agent = agent;
        Fow = fow;
        Animator = animator;
        MovementModule = movementModule;
        EnemyDecider = enemyDecider;
        
        Init();
        
    }

    public bool FindResultOfState(AIEntityState _state)
    {
        return FindAIEntityPair(_state).Value;
    }

    public AIEntityStatePair SetEntityStateValue(AIEntityState _state, bool value)
    {
        AIEntityStatePair pair = FindAIEntityPair(_state);
        pair.Value = value;
        return pair;
    }

    public AIEntityStatePair FindAIEntityPair(AIEntityState _state)
    {
        for (int i = 0; i < EntityStatePair.Count; i++)
        {
            if (EntityStatePair[i].State == _state)
            {
                return EntityStatePair[i];
            }
        }

        return null;
    }

    public void Init()
    {
        foreach (var t in EligableAIEntityState)
        {
            EntityStatePair.Add(new AIEntityStatePair(t, false));
            //Debug.Log($"{t} has been initialised by {Agent.gameObject.name}");
        }
    }
}

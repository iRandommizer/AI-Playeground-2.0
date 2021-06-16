using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This class is for me to run the "Tick" functions for sensors, planners(FSM), blackboard
public class AIAgent : MonoBehaviour
{
    //private AISenses senses; //might not need this yet
    
    public AIBlackBoard blackBoard;
    private BaseEnemy baseEnemy; // Reference to the agent's own decider
    

    //private SensorySystem sensory;
    private void Awake()
    {
        //blackBoard = new AIBlackBoard(this, GetComponent<FieldOfView>(),GetComponent<Animator>(),GetComponent<MovementModule>(), )
    }
}

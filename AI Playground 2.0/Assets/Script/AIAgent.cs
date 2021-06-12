using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    //private AISenses senses; //might not need this yet
    
    private AIBlackBoard blackBoard;

    public BaseEnemy EnemyFSMSystem { get; private set; } // !! I need to make the class more abstract

    //private SensorySystem sensory;

    public AITask CurrentTask { get; set; }
}

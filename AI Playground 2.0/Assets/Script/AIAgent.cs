using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This class is for me to run the "Tick" functions for sensors, planners(FSM), blackboard
public class AIAgent : MonoBehaviour, ICooperative
{
    public bool AgentException;
    
    //private AISenses senses; //might not need this yet
    
    public AIBlackBoard blackBoard;
    private BaseEnemy baseEnemy; // Reference to the agent's own decider
    private SensorySystem SensorySystem;

    public HerdAIBlackBoard HerdAIBlackBoard { get; set; }
    public RequestSystem HerdRequestSystem { get; set; }
    public RequestHandler RequestHandler { get; set; }

    //private SensorySystem sensory;
    private void Awake()
    {
        SensorySystem = new SensorySystem(this);
        
        blackBoard = new AIBlackBoard(this,
            GetComponent<FieldOfView>(), 
            GetComponent<Animator>(), 
            GetComponent<MovementModule>(),
            GetComponent<BaseEnemy>());
    }

    private void Start()
    {
        RequestHandler = new RequestHandler(HerdRequestSystem, blackBoard); 
        Debug.Log(HerdRequestSystem);
    }

    private void Update()
    {
        SensorySystem.Tick(blackBoard);
        RequestHandler.Tick();
        
        blackBoard.Time = Time.time;
        blackBoard.DeltaTime = Time.deltaTime;

        // !! Need to fix. There should be a more optimized way to set the request handler right
        if (blackBoard.RequestHandler == null)
        {
            blackBoard.RequestHandler = RequestHandler;
        }
    }
}

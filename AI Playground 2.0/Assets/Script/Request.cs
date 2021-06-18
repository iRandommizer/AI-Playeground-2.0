using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Request
{
    public RequestStatus status = RequestStatus.NotTaken;
    public Effect DesiredEffect;
    public AIAgent Requester;
    public AIEntityStatePair IntialStateValue;
    public bool goalStateValue;
    public float timeElapsed;

    public Request(Effect effect, AIAgent agent, AIEntityStatePair requestTrigger)
    {
        DesiredEffect = effect;
        Requester = agent;
        IntialStateValue = requestTrigger;
        goalStateValue = !IntialStateValue.Value;
    }

    public void RequestTaken(AIBlackBoard blackBoard)
    {
        status = RequestStatus.OnGoing;
        blackBoard.EnemyDecider.SetState(blackBoard.EnemyDecider.currentEnemyState);
    }
    
    public void UpdateRequest(AIBlackBoard blackBoard)
    {
        timeElapsed += blackBoard.DeltaTime;
        if (((EnemyState)(blackBoard.EnemyDecider.mFSM.m_currentState)).Effect != DesiredEffect.EffectTitle)
        {
            status = RequestStatus.Failed;
            AbortRequest();
        }
        
        status = blackBoard.FindAIEntityPair(IntialStateValue.State).Value == goalStateValue ? RequestStatus.Success : RequestStatus.OnGoing;
    }

    public void AbortRequest()
    {
        
    }
    //Update the status of a request
}

public enum RequestStatus
{
    NotTaken,
    OnGoing,
    Success,
    Failed
}

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class will handle looking for requests within the request system
/// </summary>
public class RequestHandler
{
    public float tickRate = 0.5f;
    public Request currentRequest;
    
    public RequestSystem assignedRequestSystem;
    public AIBlackBoard blackBoard;

    public RequestHandler(RequestSystem _assignedRequestSystem, AIBlackBoard _blackBoard)
    {
        assignedRequestSystem = _assignedRequestSystem;
        blackBoard = _blackBoard;
    }

    public void MakeRequest(Effect effect, AIEntityStatePair trigger)
    {
        if (assignedRequestSystem == null || blackBoard == null || blackBoard.Agent== null)
        {
            Debug.Log(assignedRequestSystem);
            Debug.Log(blackBoard);
            Debug.Log(blackBoard.Agent);
        }
        assignedRequestSystem.CheckRequestValidity(blackBoard.Agent, effect, trigger);
    }

    public void AskForRequest(List<EEffect> effects, AIAgent accepter)
    {
        if (currentRequest != null)
        {
            // Debug.Log($"Currently occupied with another request");
            // Debug.Log(accepter);
        }
        else
        {
            if (assignedRequestSystem == null) return;
            
            currentRequest = assignedRequestSystem.LookForCompatibleRequest(effects, accepter);
            if (currentRequest != null)
            {
                Debug.Log(currentRequest);
                Debug.Log(currentRequest.DesiredEffect);
                currentRequest.RequestTaken(blackBoard);
            }
        }
    }
    
    public void DiscardRequest(Request request)
    {
        assignedRequestSystem.DiscardRequest(request);
        currentRequest = null;
        Debug.Log(request + " is discarded");
    }

    public void Tick()
    {
        if (blackBoard.Agent.AgentException)
        {
                AskForRequest(blackBoard.capableEffects, blackBoard.Agent);
        }
        if (currentRequest == null) return;
        currentRequest.UpdateRequest(blackBoard);
        if (currentRequest.status == RequestStatus.Failed)
        {
            Debug.Log("failed");
            DiscardRequest(currentRequest);
        }
        // if(currentRequest.status == RequestStatus.Success)
        // {
        //         Debug.Log("success");
        //     DiscardRequest(currentRequest);
        // }
    }
}

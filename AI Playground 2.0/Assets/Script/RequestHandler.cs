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

    public void MakeRequest(EEffect effect, AIEntityStatePair trigger)
    {
        assignedRequestSystem.requestWaitingPool.Add(new Request(effect, blackBoard.Agent, trigger));
    }

    public void AskForRequest(List<EEffect> effects)
    {
        if (currentRequest != null)
        {
            Debug.Log($"Currently occupied with another request");
        }
        else
        {
            currentRequest = assignedRequestSystem.LookForCompatibleRequest(effects);
            if (currentRequest != null)
            {
                currentRequest.RequestTaken(blackBoard);
            }
        }
    }

    public void DiscardRequest(Request request)
    {
        assignedRequestSystem.DiscardRequest(request);
    }

    public void Tick()
    {
        if (currentRequest == null) return;
        currentRequest.UpdateRequest(blackBoard);
        if (currentRequest.status == RequestStatus.Failed || currentRequest.status == RequestStatus.Success)
        {
            DiscardRequest(currentRequest);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestSystem
{
    public List<Request> requestWaitingPool = new List<Request>();
    public List<Request> requestOnGoingPool = new List<Request>();

    private bool CheckRequestCompatibility(List<EEffect> passedEffects, Request curReq)
    {
        // Look for all the different beneficial effects the requester is looking for and put it in a list
        List<EEffect> currentEffects = new List<EEffect>();
        if (curReq.DesiredEffect is CompoundEffect compoundEffect)
        {
            currentEffects = compoundEffect.GetAllPrimativeEffects();
        }
        else
        {
            currentEffects.Add(curReq.DesiredEffect.EffectEnum);
        }

        foreach (var c in passedEffects)
        {
            if (currentEffects.Contains(c))
            {
                return true;
            }
        }
        return false;
    }

    public Request LookForCompatibleRequest(List<EEffect> passedEffects)
    {
        foreach (var r in requestWaitingPool)
        {
            if (CheckRequestCompatibility(passedEffects, r))
            {
                AssignRequest(r);
            }
        }
        Debug.Log("No compatible Request right now");
        return null;
    }

    // Updates the pools of request accordingly 
    private Request AssignRequest(Request request)
    {
        requestWaitingPool.Remove(request);
        requestOnGoingPool.Add(request);
        return request;
    }

    public void DiscardRequest(Request request)
    {
        if (requestWaitingPool.Contains(request))
        {
            requestWaitingPool.Remove(request);
        }
        if (requestOnGoingPool.Contains(request))
        {
            requestOnGoingPool.Remove(request);
        }
        Debug.LogWarning("Request has been discarded");
    }
}

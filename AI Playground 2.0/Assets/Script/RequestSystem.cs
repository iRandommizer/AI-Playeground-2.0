using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestSystem
{
    public List<Request> requestWaitingPool = new List<Request>();
    public List<Request> requestOnGoingPool = new List<Request>();

    //This function checks if any of the request's desired effects enum is the same as the capable effects of the agent 
    private bool CheckRequestCompatibility(List<EEffect> passedEffects, Request curReq)
    {
        List<EEffect> currentEffectsEnum = GetAllUniqueEffects(curReq);

        EEffect commonEffect = FindCommonEffect(passedEffects, currentEffectsEnum);
        if (commonEffect != EEffect.None)
        {
            curReq.CommonEffect = commonEffect;
            return true;
        }
        return false;
    }

    public Request LookForCompatibleRequest(List<EEffect> passedEffects, AIAgent accepter)
    {
        
        bool compatible = false;
        Request analysedRequest = null;

        foreach (var r in requestWaitingPool)
        {
            if (r.Requester == accepter) continue; // prevents requesters from taking their own request
            
            if (CheckRequestCompatibility(passedEffects, r))
            {
                compatible = true;
                analysedRequest = r;
            }
        }

        if (compatible)
        {
            Debug.Log("request given");
            return AssignRequest(analysedRequest);
        }
        //Debug.Log("No compatible Request right now");
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

    public void CheckRequestValidity(AIAgent requester, Effect desiredEffect, AIEntityStatePair pair)
    {
        foreach (var r in requestWaitingPool)
        {
            if (r.Requester == requester && r.DesiredEffect == desiredEffect && r.IntialStateValue == pair)
            {
                return;
            }
        }
        requestWaitingPool.Add(new Request(desiredEffect, requester, pair));
    }

    private List<EEffect> GetAllUniqueEffects(Request curReq)
    {
        // Look for all the different beneficial effects the requester is looking for and put it in a list
        List<EEffect> currentEffectsEnum = new List<EEffect>();
        
        if (curReq.DesiredEffect is CompoundEffect compoundEffect)
        {
            List<Effect> currentEffects = new List<Effect>();
            currentEffects = compoundEffect.GetAllPrimativeEffects();
            foreach (var e in currentEffects)
            {
                currentEffectsEnum.Add(e.EffectTitle);
            }
        }
        else
        {
            currentEffectsEnum.Add(curReq.DesiredEffect.EffectTitle);
            Debug.Log(curReq.DesiredEffect.EffectTitle); // !! I forgot what this is for
        }

        return currentEffectsEnum;
    }

    public EEffect FindCommonEffect(List<EEffect> _agentEffects, List<EEffect> _requestEffects)
    {
        foreach (var c in _agentEffects)
        {
            if (_requestEffects.Contains(c))
            {
                return c;
            }
        }

        return EEffect.None;
    }
}

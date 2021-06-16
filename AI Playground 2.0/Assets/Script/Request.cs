using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Request
{
    readonly Effect DesiredEffect;
    readonly AIAgent Requestor;
    public bool isNeeded;
    public float timeElapsed;

    public Request(Effect effect, AIAgent agent)
    {
        DesiredEffect = effect;
        Requestor = agent;
    }

    public Tick()
    {
        
    }
}

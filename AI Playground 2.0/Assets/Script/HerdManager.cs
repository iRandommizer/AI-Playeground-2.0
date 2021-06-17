using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdManager : MonoBehaviour
{
    public List<AIAgent> herdMembers = new List<AIAgent>();
    public HerdAIBlackBoard _blackBoard;

    private void Awake()
    {
        _blackBoard = new HerdAIBlackBoard(herdMembers);
        
        //Initialize the herd blackboard for all the herd members
        foreach (var herdMember in herdMembers)
        {
            herdMember.HerdAIBlackBoard = _blackBoard;
        }
    }
    
}

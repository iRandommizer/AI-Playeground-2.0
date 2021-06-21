using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

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
            herdMember.HerdRequestSystem = _blackBoard.requestSystem;
        }
    }
    
    private void OnDrawGizmos()
    {
        // Handles.color = Color.magenta;
        // if (_blackBoard?.requestSystem != null)
        // {
        //     for (var index = 0; index < _blackBoard.requestSystem.requestWaitingPool.Count; index++)
        //     {
        //         var r = _blackBoard.requestSystem.requestWaitingPool[index];
        //         Handles.Label((Vector2)transform.position + (index * Vector2.down * 2), r.DesiredEffect.ToString());
        //     }
        // }
    }
}

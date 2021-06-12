using System;
using System.Collections;
using System.Collections.Generic;       
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class AITask : ITask
{
    public string Name { get; set; }
    public List<ICondition> conditions { get; }
    public virtual TaskStatus TaskUpdate(IBlackBoard blackBoard) // !? I'm not sure if this is a valid way of using interface and virtual
    {
        Debug.LogError($"TaskUpdate function was not overriden in {this} class!");
        return TaskStatus.Failure;
    }

    public virtual void Stop(IBlackBoard blackBoard)
    {
    }

    protected bool CheckAllCondition(List<ICondition> conditions, IBlackBoard blackBoard)
    {
        return CheckAllCondition<Object>(conditions, blackBoard, null);
    }

    private bool CheckAllCondition<T>(List<ICondition> conditions, IBlackBoard blackBoard, T data1)
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i] is IConditionX1 conX1)
            {
                if (!conX1.IsValid(blackBoard, data1)) return false;
            }
            else if (!conditions[i].IsValid(blackBoard)) return false;
        }
        return true;
    }
}

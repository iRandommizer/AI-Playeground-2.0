using UnityEngine;

public class ExceptionCondition : ICondition
{
    public string Name => "Exception Condition";
    public bool IsValid(IBlackBoard blackBoard)
    {
        if (blackBoard is AIBlackBoard aiBlackBoard)
        {
            return aiBlackBoard.Agent.EnemyFSMSystem.exception;
        }
        Debug.LogError($"blackBoard is not an AI Blackboard!");
        return false;
    }
}

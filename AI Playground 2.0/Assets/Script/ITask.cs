using System.Collections.Generic;

public interface ITask
{
    string Name { get; set; } // For Debugging

    List<ICondition> conditions { get; } // All the conditions involved with the task

    TaskStatus TaskUpdate(IBlackBoard blackBoard);
    
    void Stop(IBlackBoard blackBoard);
}

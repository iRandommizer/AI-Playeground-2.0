using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorySystem
{
    private ISensor[] _sensors;

    public SensorySystem(AIAgent agent)
    {
        _sensors = agent.transform.GetComponents<ISensor>();
    }

    public void Tick(AIBlackBoard blackBoard)
    {
        foreach (var sensor in _sensors)
        {
            if (blackBoard.Time >= sensor.NextTickTime)
            {
                sensor.NextTickTime = blackBoard.Time + sensor.TickRate;
                sensor.Tick(blackBoard);
            }
        }
    }

    public void DrawGizmos(AIBlackBoard blackBoard)
    {
        foreach (var sensor in _sensors)
        {
            sensor.DrawGizmos(blackBoard);
        }
    }
}

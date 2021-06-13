using System.Collections.Generic;
using UnityEngine;

public class StrategyData : ScriptableObject
{
    public List<StrategyWeightPair> strategiesPairs;

    // returns all the strategies the agent is capable of
    public List<EStrategy> GetAllStrategies()
    {
        List<EStrategy> listOfStrategies = new List<EStrategy>();
        foreach (StrategyWeightPair pair in strategiesPairs)
        {
            listOfStrategies.Add(pair.strategy);
        }
        return listOfStrategies;
    }
    
}

[System.Serializable]
public struct StrategyWeightPair
{
    public EStrategy strategy;
    public float weight ;
}

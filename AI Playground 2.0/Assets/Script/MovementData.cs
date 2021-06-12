using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Data/MovementData")]
public class MovementData : ScriptableObject
{
    public List<MovementBehaviorKeyPair> pairs;

    public MovementBehaviour FindMBPair(EnemyStateTypes chosenKey)
    {
        foreach(MovementBehaviorKeyPair pair in pairs)
        {
            if (pair.key == chosenKey)
            {
                return pair.value;
            }
        }
        Debug.LogError("Cannot find pair");
        return null;
    }
}

[System.Serializable]
public struct MovementBehaviorKeyPair
{
    public EnemyStateTypes key;
    public MovementBehaviour value;
}

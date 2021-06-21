using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Compound")]
public class CompoundEffect : Effect
{
    public List<Effect> childEffects => _childEffects;

    [SerializeField]protected List<Effect> _childEffects;

    public List<Effect> GetAllPrimativeEffects(List<Effect> singularList = null)
    {
        List<Effect> effects = new List<Effect>();
        if (singularList != null)
        {
            effects = singularList;
        }       
        for (int i = 0; i < childEffects.Count; i++)
        {
            if (childEffects[i] is  CompoundEffect ce)
            {
                ce.GetAllPrimativeEffects(effects);
                continue;
            }
            
            effects.Add(childEffects[i]);
        }
        return effects;
        // Make a variable 
    }
}

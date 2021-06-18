using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Compound")]
public class CompoundEffect : Effect
{
    public List<Effect> childEffects => _childEffects;

    [SerializeField]protected List<Effect> _childEffects;

    public List<EEffect> GetAllPrimativeEffects(List<EEffect> singularList = null)
    {
        List<EEffect> effects = new List<EEffect>();
        if (singularList != null)
        {
            effects = singularList;
        }       
        for (int i = 0; i < childEffects.Count; i++)
        {
            if (childEffects[i] is  CompoundEffect ce)
            {
                GetAllPrimativeEffects(effects);
                continue;
            }
            
            effects.Add(childEffects[i].EffectTitle);
        }
        return effects;
        // Make a variable 
    }
}

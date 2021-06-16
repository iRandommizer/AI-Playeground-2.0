using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Compound")]
public class CompoundEffect : Effect
{
    public List<Effect> childEffects => _childEffects;

    [SerializeField]protected List<Effect> _childEffects;

    public List<EEffect> GetAllPrimativeEffects(CompoundEffect compoundEffect, List<EEffect> singularList = null)
    {
        List<EEffect> effects = new List<EEffect>();
        if (singularList != null)
        {
            effects = singularList;
        }
        for (int i = 0; i < compoundEffect.childEffects.Count; i++)
        {
            if (compoundEffect.childEffects[i] is  CompoundEffect ce)
            {
                GetAllPrimativeEffects(ce, effects);
                continue;
            }
            
            effects.Add(compoundEffect.childEffects[i].EffectTitle);
        }
        return effects;
        // Make a variable 
    }
}

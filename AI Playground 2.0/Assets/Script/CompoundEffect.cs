using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompoundEffect : ICompoundEffect
{
    public abstract EEffect EffectEnum { get; }
    public abstract List<IEffect> ChildEffects { get;} //I changed the list type to IEffect instead of Effect so this will probably cause issues
    public List<EEffect> GetAllPrimativeEffects(List<EEffect> singularList = null)
    {
        List<EEffect> effects = new List<EEffect>();
        if (singularList != null)
        {
            effects = singularList;
        }       
        for (int i = 0; i < ChildEffects.Count; i++)
        {
            if (ChildEffects[i] is ICompoundEffect ce)
            {
                GetAllPrimativeEffects(effects);
                continue;
            }
            
            effects.Add(ChildEffects[i].EffectEnum);
        }
        return effects;
    }
}

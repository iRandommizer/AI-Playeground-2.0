using System.Collections.Generic;

public interface IEffect
{
    EEffect EffectEnum { get; }
}

public interface ICompoundEffect : IEffect
{ 
    List<IEffect> ChildEffects { get; }
}

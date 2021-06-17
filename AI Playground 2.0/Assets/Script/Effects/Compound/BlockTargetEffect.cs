using System.Collections.Generic;

public class BlockTargetEffect : CompoundEffect
{
    public override EEffect EffectEnum => EEffect.BlockTarget;
    public override List<IEffect> ChildEffects =>  new List<IEffect>()
    {
        new BlockageAbilityEffect(),
        new ProjectilePressureEffect(),
        
    };
}

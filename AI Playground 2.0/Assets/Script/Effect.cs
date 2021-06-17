using UnityEngine;

public abstract class Effect : IEffect
{
    public EEffect EffectEnum => _effectEnum;
    public CompoundEffect parent => _parent;

    public EEffect _effectEnum;
    public CompoundEffect _parent;

}

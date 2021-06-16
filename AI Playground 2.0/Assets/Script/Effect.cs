using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Primative")]
public class Effect : ScriptableObject, IEffect
{
    public EEffect EffectTitle => _effectTitle;
    public CompoundEffect parent => _parent;

    [SerializeField]protected EEffect _effectTitle;
    [SerializeField]protected CompoundEffect _parent;
}

using UnityEngine;

public class EffectAssets : MonoBehaviour
{
    private static EffectAssets _i;

    public static EffectAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("EffectAssets")) as GameObject).GetComponent<EffectAssets>();
            return _i;
        }
    }

    public Effect BlockageAbility;
    public Effect BuffSpeed;
    public Effect PressureTargetAway;
    public Effect ProjectilePressure;
    public Effect SlowAbility;
    public Effect StunTarget;
    public Effect BlockTarget;
    public Effect FasterThanPlayer;
    public Effect SlowTarget;
}

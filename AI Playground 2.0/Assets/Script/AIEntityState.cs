public enum AIEntityState
{
    //Attack
    HasMultipleAttackAmmo,
    CurrentlyTargeted,
    IsCastingAbility,
    //Health
    HasFullHealth,
    HasHighHealth,
    HasMidHealth,
    HasLowHealth,
}

public class AIEntityStatePair
{
    public AIEntityState State;
    public bool Value;

    public AIEntityStatePair(AIEntityState _state, bool _value)
    {
        State = _state;
        Value = _value;
    }
}

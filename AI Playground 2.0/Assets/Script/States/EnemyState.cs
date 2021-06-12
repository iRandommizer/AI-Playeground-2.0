public class EnemyState : State
{
    protected BaseEnemy mBaseEnemy; // Reference to the BaseEnemy script
    private EnemyStateTypes mEnemyStateType; // The enum which will hold the key values for the FSM

    public EnemyStateTypes EnemyStateType { get { return mEnemyStateType; } } // Property which allows other classes to get reference of the state type

    // Constructor for the state to get the references for the BaseEnemy and its state type
    // !! Why is the BaseEnemy script Needed here??
    public EnemyState(FSM fsm, EnemyStateTypes type, BaseEnemy baseEnemy) : base(fsm)
    {
        mBaseEnemy = baseEnemy;
        mEnemyStateType = type;
    }

    public delegate void StateDelegate();

    public StateDelegate OnEnterDelegate { get; set; } = null;
    public StateDelegate OnExitDelegate { get; set; } = null;
    public StateDelegate OnUpdateDelegate { get; set; } = null;
    public StateDelegate OnFixedUpdateDelegate { get; set; } = null;

    public override void Enter()
    {
        base.Enter();
        OnEnterDelegate?.Invoke();
    }

    public override void Exit()
    {
        base.Exit();
        OnExitDelegate?.Invoke();
    }

    public override void Update()
    {
        base.Update();
        OnUpdateDelegate?.Invoke();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        OnFixedUpdateDelegate?.Invoke();
    }
}

public enum EnemyStateTypes
{
    WANDERING,
    INVESTIGATING,
    SCANNING,
    CHASING,
    STRAFING,
    ATTACKING,
    TAUNTING,
    DODGING,
    DEFENDING,
    DAMAGED
}
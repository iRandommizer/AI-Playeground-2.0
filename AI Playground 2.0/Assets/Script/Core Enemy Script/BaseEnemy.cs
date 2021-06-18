using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseEnemy : MonoBehaviour
{
    #region Common Enemy Data and Variables
    // State related
    public FSM mFSM;
    [HideInInspector] public EnemyStateTypes currentEnemyState;

    // External Side Componenets
    protected MovementModule movementModule; // Reference to the movement module of the enemy 
    protected AIBlackBoard blackBoard;
    protected FieldOfView fow;
    protected AttackModule attackModule;

    // Physics
    [HideInInspector] public Rigidbody2D rb;

    // Colliders
    [HideInInspector] public Collider2D mainCollider;

    [SerializeField]protected Transform currentTarget; // Reference to the transform of the current target 
    protected Vector2 lastPosOfTarget;

    // Data
    public MovementData movementData;

    //!! Temp:
    public bool exception = false;
    #endregion

    #region Properties

    #endregion

    #region Monobehaviour

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movementModule = GetComponent<MovementModule>();
        fow = GetComponent<FieldOfView>();
        attackModule = GetComponent<AttackModule>();

        #region FSM Region
        mFSM = new FSM();

        mFSM.Add((int)EnemyStateTypes.WANDERING, new EnemyState(mFSM, EnemyStateTypes.WANDERING, this));
        mFSM.Add((int)EnemyStateTypes.INVESTIGATING, new EnemyState(mFSM, EnemyStateTypes.INVESTIGATING, this));
        mFSM.Add((int)EnemyStateTypes.SCANNING, new EnemyState(mFSM, EnemyStateTypes.SCANNING, this));
        mFSM.Add((int)EnemyStateTypes.CHASING, new EnemyState(mFSM, EnemyStateTypes.CHASING, this));
        mFSM.Add((int)EnemyStateTypes.ATTACKING, new EnemyState(mFSM, EnemyStateTypes.ATTACKING, this));
        mFSM.Add((int)EnemyStateTypes.DAMAGED, new EnemyState(mFSM, EnemyStateTypes.DAMAGED, this));

        //Initializing all the different states in the delegates
        InitializeWanderingState();
        InitializeInvestigatingState();
        InitializeScanningState();
        InitializeChasingState();
        InitializeAttackingState();
        InitializeDamagedState();

        SetState(EnemyStateTypes.WANDERING);
        #endregion
    }

    private void Start()
    {
        blackBoard = GetComponent<AIAgent>().blackBoard;
    }

    public virtual void Update()
    {
        mFSM.Update(); // Order of excecution matters
        #region Debug Tools

        if (Input.GetMouseButtonDown(0))
        {
            movementModule.CurrentTargetPos = Vector2.zero;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetState(EnemyStateTypes.WANDERING);
        }

        #endregion
    }

    public virtual void FixedUpdate()
    {
        mFSM.FixedUpdate();
    }

    #endregion

    #region State Functions
    public virtual void InitializeWanderingState()
    {
        float wanderSpeed = 25;
        if (exception)
        {
            wanderSpeed = 50;
        }
        float randomYval = Random.Range(1, 1000);
        EnemyState state = (EnemyState)mFSM.GetState((int)EnemyStateTypes.WANDERING);

        state.OnEnterDelegate += delegate ()
        {
            movementModule.lookType = LookTypes.INDIRECTION;
            movementModule.mb = movementData.FindMBPair(state.EnemyStateType);
            movementModule.CurrentTargetPos = lastPosOfTarget;
            movementModule.maxSpeed = wanderSpeed;            
        };

        state.OnUpdateDelegate += delegate ()
        {
            float noiseVal = Mathf.PerlinNoise(Time.time/4 + (randomYval * 2), randomYval);
            if(noiseVal < 0.4f && !exception)
            {
                movementModule.maxSpeed = 0;
                movementModule.rb.velocity = Vector2.zero;
            }
            else
            {
                movementModule.maxSpeed = wanderSpeed * noiseVal;
                if (exception)
                {
                    movementModule.maxSpeed = wanderSpeed * (noiseVal / 4 + 0.75f);
                }
            }

            currentTarget = LookForTarget("Player");
            if(currentTarget != null)
            {
                SetState(EnemyStateTypes.CHASING);
            }
            //if (exception)
            //{
            //    if (Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask("Agents")).Length > 2)
            //    {
            //        wanderSpeed = 105;
            //    }
            //    else
            //    {
            //        wanderSpeed = 90;
            //    }
            //}
        };
    }

    public virtual void InitializeInvestigatingState()
    {
        EnemyState state = (EnemyState)mFSM.GetState((int)EnemyStateTypes.INVESTIGATING);

        state.OnEnterDelegate += delegate ()
        {
            movementModule.StopCoroutine("FollowPath");
            movementModule.mb = movementData.FindMBPair(state.EnemyStateType);
            movementModule.maxSpeed = 45;
            PathRequestManager.RequestPath(movementModule.BackPos, lastPosOfTarget, movementModule.OnPathFound);
        };

        state.OnUpdateDelegate += delegate ()
        {
            if(LookForTarget("Player") != null)
            {
                SetState(EnemyStateTypes.CHASING);
            }
            else if(Vector2.Distance(transform.position, lastPosOfTarget) < 1f + (rb.velocity.magnitude / 9f))
            {
                SetState(EnemyStateTypes.WANDERING);
            }
            // If MovementModule Target != null, look for it's position
            if (currentTarget != null)
            {
                movementModule.CurrentTargetPos = currentTarget.GetComponent<MovementModule>().PosForwardx3;
                lastPosOfTarget = (Vector2)currentTarget.position + currentTarget.GetComponent<Rigidbody2D>().velocity.normalized * currentTarget.GetComponent<Rigidbody2D>().velocity.magnitude / 4;
            }
        };

        state.OnExitDelegate += delegate ()
        {
            movementModule.StopCoroutine("FollowPath");
        };
    }

    public virtual void InitializeScanningState()
    {
        EnemyState state = (EnemyState)mFSM.GetState((int)EnemyStateTypes.SCANNING);

        state.OnEnterDelegate += delegate ()
        {

        };
    }

    public virtual void InitializeChasingState()
    {
        EnemyState state = (EnemyState)mFSM.GetState((int)EnemyStateTypes.CHASING);

        state.OnEnterDelegate += delegate ()
        {
            movementModule.mb = movementData.FindMBPair(state.EnemyStateType);
            movementModule.maxSpeed = 40;
        };

        state.OnUpdateDelegate += delegate ()
        {
            // Constantly can the target's place
            currentTarget = LookForTarget("Player");

            // If MovementModule Target == null, then go back wandering state
            if (currentTarget == null)
            {
                SetState(EnemyStateTypes.INVESTIGATING);
            }


            // If MovementModule Target != null, look for it's position
            if (currentTarget != null)
            {

                movementModule.CurrentTargetPos = currentTarget.GetComponent<MovementModule>().PosForwardx2;
                lastPosOfTarget = (Vector2)currentTarget.position + currentTarget.GetComponent<Rigidbody2D>().velocity.normalized * currentTarget.GetComponent<Rigidbody2D>().velocity.magnitude / 4;
            }

            if (currentTarget != null && Vector2.Distance(currentTarget.position, transform.position) < 6 && attackModule.readyToFight)
            {
                SetState(EnemyStateTypes.ATTACKING);
            }
        };
    }

    public virtual void InitializeAttackingState()
    {
        EnemyState state = (EnemyState)mFSM.GetState((int)EnemyStateTypes.ATTACKING);

        float animDur = 0;
        //!!

        state.OnEnterDelegate += delegate ()
        {
            movementModule.lookType = LookTypes.ATTARGET;
            movementModule.maxSpeed = 0;
            attackModule.PlayAnim("Enemy Attack");
            animDur = attackModule.attackAnim.GetCurrentAnimatorStateInfo(0).length;
        };

        state.OnUpdateDelegate += () => // Lambda Expression
        {
            if(animDur > 0f)
            {
                animDur -= Time.deltaTime;
            }
            else
            {
                SetState(EnemyStateTypes.STRAFING);
            }
        };

        state.OnExitDelegate += delegate ()
        {
            attackModule.readyToFight = false;
            attackModule.attackCooldownCounter = 0;
            movementModule.lookType = LookTypes.INDIRECTION;
        };
    }

    public virtual void InitializeDamagedState()
    {
        EnemyState state = (EnemyState)mFSM.GetState((int)EnemyStateTypes.DAMAGED);

        // Lambda expression
        state.OnEnterDelegate += () =>
        {

        };
    }

    public void SetState(EnemyStateTypes type)
    {
        mFSM.SetCurrentState(mFSM.GetState((int)type));
        currentEnemyState = type;
        //Debug.Log(currentEnemyState);
    }
    #endregion

    #region Enemy Function
    //Need to upgrade to find closest target
    public Transform LookForTarget(string tag)
    {
        if (fow.visibleTarget.Count > 0)
        {
            for (int i = 0; i < fow.visibleTarget.Count; i++)
            {
                if (fow.visibleTarget[i].CompareTag(tag))
                {
                    return fow.visibleTarget[i];
                }
            }
        }
        return null;
    }
    #endregion
    
    void OnDrawGizmos()
    {
        //!!            
        //Handles.Label(transform.position, System.Enum.GetName(typeof(EnemyStateTypes), currentEnemyState));
        //Handles.color = Color.red;
        //Handles.DrawSolidArc(lastPosOfTarget, Vector3.forward, Vector3.up, 360, 0.5f);
        //Handles.Label(lastPosOfTarget, "last seen");
        //Handles.color = Color.red;
        //Handles.DrawWireArc(movementModule.FrontPos, Vector3.forward, Vector3.up, 360, 0.5f);
    }
}

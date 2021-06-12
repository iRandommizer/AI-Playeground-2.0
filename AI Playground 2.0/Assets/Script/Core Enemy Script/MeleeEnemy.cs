using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    public float strafingDist = 5;

    public override void Awake()
    {
        base.Awake();

        mFSM.Add((int)EnemyStateTypes.STRAFING, new EnemyState(mFSM, EnemyStateTypes.STRAFING, this));

        //InitializeStrafingState();
    }

    public override void InitializeChasingState()
    {
        base.InitializeChasingState();

        EnemyState state = (EnemyState)mFSM.GetState((int)EnemyStateTypes.CHASING);

        state.OnUpdateDelegate += delegate ()
        {
            if (currentTarget != null && Vector2.Distance(currentTarget.position, transform.position) <= strafingDist && currentTarget.GetComponent<Rigidbody2D>().velocity.magnitude < rb.velocity.magnitude)
            {
                //SetState(EnemyStateTypes.STRAFING);
            }
        };
    }

    public virtual void InitializeStrafingState()
    {
        EnemyState state = (EnemyState)mFSM.GetState((int)EnemyStateTypes.STRAFING);

        state.OnEnterDelegate += delegate ()
        {
            // Make sure that entity is facing the player
            movementModule.lookType = LookTypes.ATTARGET;

            // Change the movement behavior of the entity
            movementModule.mb = movementData.FindMBPair(state.EnemyStateType);

            // Adjust the max speed of the entity
            movementModule.maxSpeed = 45; 
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

            // If target too far then chase again
            if (currentTarget != null &&  Vector2.Distance(currentTarget.position, transform.position) > strafingDist /*should be refactored*/)
            {
                SetState(EnemyStateTypes.CHASING);
            }

            if(currentTarget != null)
            {
                movementModule.CurrentTargetPos = currentTarget.GetComponent<MovementModule>().FrontPos;
                lastPosOfTarget = (Vector2)currentTarget.position + currentTarget.GetComponent<Rigidbody2D>().velocity.normalized * currentTarget.GetComponent<Rigidbody2D>().velocity.magnitude/2;
                //movementModule.CurrentTargetPos = lastPosOfTarget;
            }

            if(currentTarget != null && Vector2.Distance(currentTarget.position, transform.position) < 6 && attackController.readyToFight)
            {
                SetState(EnemyStateTypes.ATTACKING);
            }
        };

        state.OnExitDelegate += delegate ()
        {
            movementModule.lookType = LookTypes.INDIRECTION;
        };
    }
}

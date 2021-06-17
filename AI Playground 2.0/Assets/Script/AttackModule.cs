using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackModule : MonoBehaviour, ISensor
{
    public Animator attackAnim; // Refer to the blackboard
    [HideInInspector]public float attackCooldownCounter = 0;
    public float cooldownDuration = 4;
    private float extraRandomDur;
    public bool readyToFight = false;
    
    //Attack Ammo
    public float attackAmmoCooldown = 4;
    public float attackAmmoCooldownCounter;
    public float attackAmmoAmount = 3; //Maximum number of attacks they can save up before unleashing it
    public float currentAttackAmmoAmount;

    private float _tickRate = 1f;
    
    #region Properties

    public float TickRate => _tickRate;
    public float NextTickTime { get; set; }
    #endregion

    private void Awake()
    {
        attackAnim = GetComponentInChildren<Animator>();
        attackCooldownCounter = cooldownDuration;
        attackAmmoCooldownCounter = attackAmmoCooldown;
    }

    private void Update()
    {
        #region Old Implementation

        // if(readyToFight == false)
        // {
        //     if (attackCooldownCounter < cooldownDuration + extraRandomDur + attackAnim.GetCurrentAnimatorStateInfo(0).length)
        //     {
        //         attackCooldownCounter += Time.deltaTime;
        //     }
        //     else
        //     {
        //         readyToFight = true;
        //         RandomizeExtraDuration(cooldownDuration);
        //     }
        // }

        #endregion

        if (currentAttackAmmoAmount < attackAmmoAmount) //As long as cur is < than max amount
        {
            if (attackAmmoCooldownCounter > 0)
            {
                attackAmmoCooldownCounter -= Time.deltaTime;
            }
            else
            {
                currentAttackAmmoAmount += 1;
                attackAmmoCooldownCounter = attackAmmoCooldown;
            }
        }
        
    }
    public void PlayAnim(string animName)
    {
        attackAnim.Play(animName);
    }

    public void RandomizeExtraDuration(float cooldownDur)
    {
        extraRandomDur = Random.Range(0f, (float)cooldownDur / 2);
    }
    public void Tick(AIBlackBoard blackBoard)
    {
        if (currentAttackAmmoAmount/attackAmmoAmount < 0.5f)
        {
            blackBoard.SetEntityStateValue(AIEntityState.HasMultipleAttackAmmo, false);
        }
        else
        {
            AIEntityStatePair pair = blackBoard.SetEntityStateValue(AIEntityState.HasMultipleAttackAmmo, true);
            RequestHandler requestHandler = GetComponent<AIAgent>().RequestHandler; //!! Need to find a better way to do this
            requestHandler.MakeRequest(EEffect.SlowTarget, pair);
        }
    }

    public void DrawGizmos(AIBlackBoard blackBoard)
    {
        throw new System.NotImplementedException();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Animator attackAnim;
    [HideInInspector]public float attackCooldownCounter = 0;
    public float cooldownDuration = 4;
    private float extraRandomDur;
    public bool readyToFight = false;

    private void Awake()
    {
        attackAnim = GetComponentInChildren<Animator>();
        attackCooldownCounter = cooldownDuration;
    }

    private void Update()
    {
        if(readyToFight == false)
        {
            if (attackCooldownCounter < cooldownDuration + extraRandomDur + attackAnim.GetCurrentAnimatorStateInfo(0).length)
            {
                attackCooldownCounter += Time.deltaTime;
                
            }
            else
            {
                readyToFight = true;
                RandomizeExtraDuration(cooldownDuration);
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
}

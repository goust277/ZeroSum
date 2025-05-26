using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout_Hit : BaseState
{
    private Scout scout;
    private float blinkDuration = 0.5f;
    private float blinkInterval = 0.1f; 
    public float elapsedBlinkTime = 0f;
    private float intervalTimer = 0f; 
    private bool isBlinking = false;
    private float hit_T;

    public Scout_Hit(StateMachine stateMachine, Scout monster) : base(stateMachine)
    {
        this.scout = monster;
    }

    public override void Enter()
    {
        scout.PlayDamagedSound();
        scout.isHit = true;
        scout.anim.SetTrigger("isHit");
        scout.rb.velocity = Vector2.zero;
        hit_T = 0.1f;
    }

    public override void Execute()
    {        
        if (scout.anim.GetCurrentAnimatorStateInfo(0).IsName("Scout_hit")
        && scout.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
        {
            stateMachine.ChangeState(new Scout_Chase(stateMachine, scout));
        }
    }
    public override void Exit()
    {
        scout.anim.ResetTrigger("isHit");
        scout.isHit = false;
        scout.attackCooldown = 3f;
        scout.canAttack = true;
    }
}

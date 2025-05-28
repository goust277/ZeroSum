using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout_Hit : BaseState
{
    private Scout scout;

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
    }

    public override void Execute()
    {        

    }
    public override void Exit()
    {
        scout.isHit = false;
        scout.attackCooldown = 1.5f;
        scout.canAttack = true;
    }
}

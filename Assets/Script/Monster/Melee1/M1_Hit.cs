using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1_Hit : BaseState
{
    private Melee1 m1;

    public M1_Hit(StateMachine stateMachine, Melee1 monster) : base(stateMachine)
    {
        this.m1 = monster;
    }

    public override void Enter()
    {
        //m1.PlayDamagedSound();
        m1.isHit = true;
        m1.anim.SetTrigger("isHit");
        m1.rb.velocity = Vector2.zero;
    }

    public override void Execute()
    {
        stateMachine.ChangeState(new M1_Chase(stateMachine, m1));
    }
    public override void Exit()
    {
        m1.isHit = false;
        m1.attackCooldown = 3f;
        m1.canAttack = true;
    }
}

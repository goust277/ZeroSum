using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Hit : BaseState
{
    private Mission_melee mm;

    public MM_Hit(StateMachine stateMachine, Mission_melee monster) : base(stateMachine)
    {
        this.mm = monster;
    }

    public override void Enter()
    {
        //m1.PlayDamagedSound();
        mm.isHit = true;
        mm.anim.SetTrigger("isHit");
        mm.rb.velocity = Vector2.zero;
    }

    public override void Execute()
    {
        stateMachine.ChangeState(new MM_Chase(stateMachine, mm));
    }
    public override void Exit()
    {
        mm.isHit = false;
        mm.attackCooldown = 3f;
        mm.canAttack = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Attack : BaseState
{
    private Melee m;

    public M_Attack(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격 상태");
        m.anim.SetTrigger("isAttack");
    }

    public override void Execute()
    {
        if(m.anim.GetCurrentAnimatorStateInfo(0).IsName("Melee_Attack") 
            && m.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(new M_Chase(stateMachine, m));
            return;
        }
    }
    public override void Exit()
    {
        m.canAttack = true;
        m.attackCooldown = 3f;
    }
}

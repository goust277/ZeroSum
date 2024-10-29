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
        m.anim.SetTrigger("isAttack");
    }

    public override void Execute()
    {

    }
    public override void Exit()
    {
        m.isAttack = true;
    }
}

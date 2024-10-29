using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Ready : BaseState
{
    private Melee m;

    public M_Ready(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        m.anim.SetTrigger("isReady");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        
    }
}

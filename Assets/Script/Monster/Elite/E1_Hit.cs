using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_Hit : BaseState
{
    private Elite1 m;

    public E1_Hit(StateMachine stateMachine, Elite1 monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

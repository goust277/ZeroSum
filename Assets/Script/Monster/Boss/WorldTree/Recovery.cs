using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recovery : BaseState
{
    private WorldTree boss;

    public Recovery(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        boss.anim.SetBool("Head_up", true);
        boss.headExposed = false;
    }

    public override void Execute()
    {
        if (boss.anim.GetCurrentAnimatorStateInfo(0).IsName("HeadUp")
            && boss.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit() 
    {
        boss.anim.SetBool("Head_up", false);
    }
}


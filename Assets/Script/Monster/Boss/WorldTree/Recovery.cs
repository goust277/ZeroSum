using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recovery : BaseState
{
    private WorldTree boss;
    private float timer = 2f;

    public Recovery(StateMachine stateMachine, WorldTree boss) : base(stateMachine) { this.boss = boss; }

    public override void Enter()
    {
        timer = 2f;
        boss.anim.SetTrigger("Recover");
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            stateMachine.ChangeState(boss.idleState);
        }
    }
}


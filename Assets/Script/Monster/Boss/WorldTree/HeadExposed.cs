using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadExposed : BaseState
{
    private WorldTree boss;
    private float timer;

    public HeadExposed(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        boss.headExposed = true;
        timer = boss.exposedDuration;
        boss.anim.SetTrigger("Head_down");
        boss.Head.enabled = true;
    }

    public override void Execute()
    {
        
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            boss.Recover();
        }
    }

    public override void Exit()
    {
        boss.anim.ResetTrigger("Head_down");
        boss.Head.enabled = false;
    }
}

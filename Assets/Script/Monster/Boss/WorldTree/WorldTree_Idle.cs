using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTree_Idle : BaseState
{
    private WorldTree boss;
    private float waitTime = 1.5f;
    private float timer;

    public WorldTree_Idle(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        timer = waitTime;
        boss.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        // 대기 후 다음 패턴 선택
        if (timer <= 0f)
        {
            if (boss.isDying)
            {
                stateMachine.ChangeState(boss.finalBurstState);
                return;
            }
            boss.GetRandomPattern();
            stateMachine.ChangeState(boss.pattern1);
        }
    }

    public override void Exit() 
    {
        boss.anim.SetBool("isIdle", false);
    }
}

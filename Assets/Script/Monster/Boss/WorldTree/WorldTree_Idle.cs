using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTree_Idle : BaseState
{
    private WorldTree boss;
    private float timer;

    public WorldTree_Idle(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter() { timer = 2f; boss.anim.SetBool("isIdle", true); }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (boss.leftArmDestroyed && boss.rightArmDestroyed && !boss.headExposed)
        {
            boss.ExposeHead();
            return;
        }

        if (timer <= 0f)
        {
            if (boss.nextQueuedState != null)
            {
                stateMachine.ChangeState(boss.nextQueuedState);
                boss.nextQueuedState = null;
                return;
            }

            if (boss.CanUseLeftArm()) stateMachine.ChangeState(boss.leftAttackState);
            else if (boss.CanUseRightArm()) stateMachine.ChangeState(boss.rightAttackState);
        }
    }

    public override void Exit() { boss.anim.SetBool("isIdle", false); }
}

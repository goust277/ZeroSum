using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArm : BaseState
{
    private WorldTree boss;
    private float timer = 2f;

    public RightArm(StateMachine stateMachine, WorldTree boss) : base(stateMachine) { this.boss = boss; }

    public override void Enter()
    {
        timer = 2f;
        boss.anim.SetTrigger("RightArmAttack");
        boss.ResetRightTimer();
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;
        if (boss.rightArmDestroyed) stateMachine.ChangeState(boss.idleState);
        else if (timer <= 0f)
        {
            boss.nextQueuedState = boss.seedDropToLeft;
            stateMachine.ChangeState(boss.idleState);
        }
    }
}

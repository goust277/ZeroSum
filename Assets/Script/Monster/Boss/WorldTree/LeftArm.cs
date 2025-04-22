using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArm : BaseState
{
    private WorldTree boss;
    private float timer = 2f;

    public LeftArm(StateMachine stateMachine, WorldTree boss) : base(stateMachine) { this.boss = boss; }

    public override void Enter()
    {
        timer = 2f;
        boss.anim.SetTrigger("LeftArmAttack");
        boss.ResetLeftTimer();
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;
        if (boss.leftArmDestroyed) stateMachine.ChangeState(boss.idleState);
        else if (timer <= 0f)
        {
            boss.nextQueuedState = boss.seedDropToRight;
            stateMachine.ChangeState(boss.idleState);
        }
    }
}

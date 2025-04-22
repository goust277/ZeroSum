using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedDrop : BaseState
{
    private WorldTree boss;
    private float timer = 3f;
    private bool nextAttackLeft;

    public SeedDrop(StateMachine stateMachine, WorldTree boss, bool nextAttackLeft) : base(stateMachine)
    {
        this.boss = boss;
        this.nextAttackLeft = nextAttackLeft;
    }

    public override void Enter()
    {
        timer = 3f;
        boss.anim.SetTrigger("SeedDrop");
        boss.ResetSeedTimer();
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            boss.nextQueuedState = nextAttackLeft ? boss.leftAttackState : boss.rightAttackState;
            stateMachine.ChangeState(boss.idleState);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadExposed : BaseState
{
    private WorldTree boss;
    private float timer = 10f;

    public HeadExposed(StateMachine stateMachine, WorldTree boss) : base(stateMachine) { this.boss = boss; }

    public override void Enter()
    {
        timer = 10f;
        boss.anim.SetTrigger("HeadExpose");
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            boss.Recover();
        }
    }
}

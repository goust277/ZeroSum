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

        // ��� �� ���� ���� ����
        if (timer <= 0f)
        {
            boss.ChooseOnePattern();
            stateMachine.ChangeState(boss.nextPattern);
            boss.nextPattern = null;
        }
    }

    public override void Exit() 
    {
        boss.anim.SetBool("isIdle", false);
    }
}

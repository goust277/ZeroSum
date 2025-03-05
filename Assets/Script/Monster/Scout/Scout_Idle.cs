using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout_Idle : BaseState
{
    private Scout scout;
    private float timer = 0f;

    public Scout_Idle(StateMachine stateMachine, Scout monster) : base(stateMachine)
    {
        this.scout = monster;
    }

    public override void Enter()
    {
        Debug.Log("대기 상태");
        timer = 3f;
        scout.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (scout.isPlayerInRange)
        {
            stateMachine.ChangeState(new Scout_Chase(stateMachine, scout));
            return;
        }

        if (timer <= 0f)
        {
            stateMachine.ChangeState(new Scout_Patrol(stateMachine, scout));
        }
    }
    public override void Exit()
    {
        scout.anim.SetBool("isIdle", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Idle : BaseState
{
    private Spider s;
    private float timer = 0f;

    public S_Idle(StateMachine stateMachine, Spider monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        Debug.Log("대기 상태");
        timer = 2f;
        s.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (s.isPlayerInRange)
        {
            stateMachine.ChangeState(new S_Chase(stateMachine, s));
            return;
        }

        if (timer <= 0f)
        {
            stateMachine.ChangeState(new S_Patrol(stateMachine, s));
        }
    }
    public override void Exit()
    {
        s.anim.SetBool("isIdle", false);
    }
}

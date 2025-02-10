using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class E1_Idle : BaseState
{
    private Elite1 m;
    private float timer = 0f;

    public E1_Idle(StateMachine stateMachine, Elite1 monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        Debug.Log("대기 상태");
        timer = 2f;
        m.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (m.isPlayerInRange)
        {
            stateMachine.ChangeState(new E1_Chase(stateMachine, m));
            return;
        }

        if (timer <= 0f)
        {
            //stateMachine.ChangeState(new E1_Patrol(stateMachine, m));
        }
    }
    public override void Exit()
    {
        m.anim.SetBool("isIdle", false);
    }
}

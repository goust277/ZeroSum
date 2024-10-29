using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Idle : BaseState
{
    private Melee m;
    private float timer = 0f;

    public M_Idle(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        Debug.Log("대기 상태");
        timer = 2f;
        m.anim.SetTrigger("isIdle");
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if(m.IsPlayerInRange())
        {
            stateMachine.ChangeState(new M_Chase(stateMachine, m));
            return;
        }

        if(timer <= 0f)
        {
            stateMachine.ChangeState(new M_Patrol(stateMachine, m));
        }
    }
    public override void Exit()
    {
        m.anim.ResetTrigger("isIdle");
    }
}
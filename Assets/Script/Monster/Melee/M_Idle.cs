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
        Debug.Log("��� ����");
        timer = 2f;
        m.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if(m.isPlayerInRange)
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
        m.anim.SetBool("isIdle", false);
    }
}
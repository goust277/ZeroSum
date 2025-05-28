using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1_Idle : BaseState
{
    private Melee1 m1;
    private float timer = 0f;

    public M1_Idle(StateMachine stateMachine, Melee1 monster) : base(stateMachine)
    {
        this.m1 = monster;
    }

    public override void Enter()
    {
        Debug.Log("대기 상태");
        timer = 2f;
        m1.StopMoveSound();
        m1.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        if(isFreeze())
        {
            return;
        }

        if (m1.wait_T >= 0)
            return;
        
        timer -= Time.deltaTime;

        if (m1.isPlayerInRange)
        {
            stateMachine.ChangeState(new M1_Chase(stateMachine, m1));
            return;
        }

        if (timer <= 0f)
        {
            stateMachine.ChangeState(new M1_Patrol(stateMachine, m1));
        }
    }
    public override void Exit()
    {
        m1.anim.SetBool("isIdle", false);
    }

    bool isFreeze()
    {
        return (m1.rb.constraints & RigidbodyConstraints2D.FreezePositionX) == RigidbodyConstraints2D.FreezePositionX;
    }
}

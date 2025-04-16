using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Idle : BaseState
{
    Tanker tanker;

    private float timer = 0f;

    public T_Idle(StateMachine stateMachine, Tanker monster) : base(stateMachine)
    {
        this.tanker = monster;
    }

    public override void Enter()
    {
        Debug.Log("대기 상태");
        timer = 3f;
        tanker.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (tanker.isPlayerInRange)
        {
            stateMachine.ChangeState(new T_Chase(stateMachine, tanker));
            return;
        }

        if (timer <= 0f)
        {
            stateMachine.ChangeState(new T_Patrol(stateMachine, tanker));
        }
    }
    public override void Exit()
    {
        tanker.anim.SetBool("isIdle", false);
    }
}

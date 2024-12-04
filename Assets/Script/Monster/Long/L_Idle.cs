using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Idle : BaseState
{
    private Long l;
    private float timer = 0f;

    public L_Idle(StateMachine stateMachine, Long monster) : base(stateMachine)
    {
        this.l = monster;
    }

    public override void Enter()
    {
        timer = 2f;
        l.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (l.isPlayerInRange)
        {
            stateMachine.ChangeState(new L_Chase(stateMachine, l));
            return;
        }

        if (timer <= 0f)
        {
            stateMachine.ChangeState(new L_Patrol(stateMachine, l));
        }
    }
    public override void Exit()
    {
        l.anim.SetBool("isIdle", false);
    }
}

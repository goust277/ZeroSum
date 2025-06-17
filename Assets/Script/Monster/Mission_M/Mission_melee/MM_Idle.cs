using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Idle : BaseState
{
    private Mission_melee mm;
    private float timer = 0f;

    public MM_Idle(StateMachine stateMachine, Mission_melee monster) : base(stateMachine)
    {
        this.mm = monster;
    }

    public override void Enter()
    {
        Debug.Log("대기 상태");
        timer = 2f;
        mm.StopMoveSound();
        mm.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        if(isFreeze())
        {
            return;
        }

        if (mm.wait_T >= 0)
            return;
        
        timer -= Time.deltaTime;

        if (mm.isPlayerInRange)
        {
            stateMachine.ChangeState(new MM_Chase(stateMachine, mm));
            return;
        }
    }
    public override void Exit()
    {
        mm.anim.SetBool("isIdle", false);
    }

    bool isFreeze()
    {
        return (mm.rb.constraints & RigidbodyConstraints2D.FreezePositionX) == RigidbodyConstraints2D.FreezePositionX;
    }
}

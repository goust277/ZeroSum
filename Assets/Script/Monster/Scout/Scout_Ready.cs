using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout_Ready : BaseState
{
    private Scout scout;
    public Scout_Ready(StateMachine stateMachine, Scout monster) : base(stateMachine)
    {
        this.scout = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격준비 상태");

        scout.anim.SetBool("isReady", true);
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = scout.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Scout_ready") && stateInfo.normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(new Scout_Attack(stateMachine, scout));
            return;
        }
    }

    public override void Exit()
    {
        scout.anim.SetBool("isReady", false);
    }
}

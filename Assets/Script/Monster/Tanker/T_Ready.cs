using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Ready : BaseState
{
    Tanker tanker;

    public T_Ready(StateMachine stateMachine, Tanker monster) : base(stateMachine)
    {
        this.tanker = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격준비 상태");
        tanker.StopMoveSound();
        tanker.anim.SetBool("isReady", true);
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = tanker.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("T_ready") && stateInfo.normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(new T_Attack(stateMachine, tanker));
            return;
        }
    }

    public override void Exit()
    {
        tanker.anim.SetBool("isReady", false);
    }
}

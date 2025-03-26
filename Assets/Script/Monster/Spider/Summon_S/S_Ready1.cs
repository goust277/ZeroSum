using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Ready1 : BaseState
{
    private Summon_S s;

    public S_Ready1(StateMachine stateMachine, Summon_S monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        s.rb.velocity = Vector2.zero;
        s.anim.SetBool("isReady", true);
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = s.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("S_ready") && stateInfo.normalizedTime >= 0.75f)
        {
            stateMachine.ChangeState(new S_Attack1(stateMachine, s));
            return;
        }
    }

    public override void Exit() 
    {
        s.anim.SetBool("isReady", false);
    }
}
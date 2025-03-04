using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Ready : BaseState
{
    private Spider s;

    public S_Ready(StateMachine stateMachine, Spider monster) : base(stateMachine)
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

        if (stateInfo.IsName("S_ready") && stateInfo.normalizedTime >= 0.98f)
        {
            stateMachine.ChangeState(new S_Attack(stateMachine, s));
            return;
        }
    }

    public override void Exit() 
    {
        s.anim.SetBool("isReady", false);
    }
}

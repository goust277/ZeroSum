using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Die : BaseState
{
    private Spider s;

    public S_Die(StateMachine stateMachine, Spider monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        s.isDie = true;
        
        s.PlayDeathSound();

        s.anim.SetTrigger("isDie");
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = s.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("S_die") && stateInfo.normalizedTime >= 0.98f)
        {
            s.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class S_Attack1 : BaseState
{
    private Summon_S s;

    public S_Attack1(StateMachine stateMachine, Summon_S monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        s.attack.gameObject.SetActive(true);
        s.PlayAttackSound();
        s.anim.SetBool("isAttack", true);
    }

    public override void Execute()
    {
        if (s.anim.GetCurrentAnimatorStateInfo(0).IsName("S_attack")
            && s.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            s.attack.gameObject.SetActive(false);
            s.gameObject.SetActive(false);
        }
    }

    public override void Exit()
    {
    }
}

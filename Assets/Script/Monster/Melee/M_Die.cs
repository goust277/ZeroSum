using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class M_Die : BaseState
{
    private Melee m;

    public M_Die(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        m.anim.SetTrigger("isDie");
        
        if (m.anim.GetFloat("Dir") > 0)
        {
            //¿À¸¥ÂÊ
            m.sprite.flipX = false;
        }

        if (m.anim.GetFloat("Dir") < 0)
        {
            //¿ÞÂÊ
            m.sprite.flipX = true;
        }
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = m.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("M_Die") && stateInfo.normalizedTime >= 0.98f)
        {
            m.gameObject.SetActive(false);
        }
    }
    public override void Exit()
    {
   
    }
}

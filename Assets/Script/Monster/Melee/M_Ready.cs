using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class M_Ready : BaseState
{
    private Melee m;

    public M_Ready(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격준비 상태");

        m.isDashing = true;
        m.anim.SetBool("isReady", true);

        if (m.anim.GetFloat("Dir") > 0)
        {
            //오른쪽
            m.R_attack.gameObject.SetActive(true);
        }

        if (m.anim.GetFloat("Dir") < 0)
        {
            //왼쪽
            m.L_attack.gameObject.SetActive(true);
        }

        m.rb.velocity = new Vector2(m.dashRange * m.anim.GetFloat("Dir"), m.rb.velocity.y);
    }

    public override void Execute()
    {
        if(m.touchPlayer)
        {
            Debug.Log("닿았음");
            stateMachine.ChangeState(new M_Attack(stateMachine, m));
            m.touchPlayer = false;
            return;
        }

        //if (Mathf.Abs(m.transform.position.x) >= Mathf.Abs(targetPosition.x))
        //{
        //    stateMachine.ChangeState(new M_Attack(stateMachine, m));
        //    return;
        //}

        AnimatorStateInfo stateInfo = m.anim.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("Ready") && stateInfo.normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(new M_Attack(stateMachine, m));
            return;
        }   
    }

    public override void Exit()
    {
        m.isDashing = false;
        m.anim.SetBool("isReady", false);
    }
}

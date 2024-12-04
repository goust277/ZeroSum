using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class M_Attack : BaseState
{
    private Melee m;

    public M_Attack(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격 상태");
        m.anim.SetBool("isAttack", true);
        m.rb.velocity = Vector2.zero;
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = m.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 0.98f)
        {
            stateMachine.ChangeState(new M_Chase(stateMachine, m));
            return;
        }
    }
    public override void Exit()
    {
        m.L_attack.gameObject.SetActive(false);
        m.R_attack.gameObject.SetActive(false);
        m.canAttack = true;
        m.attackCooldown = 3f;
        m.anim.SetBool("isAttack", false);
    }
}

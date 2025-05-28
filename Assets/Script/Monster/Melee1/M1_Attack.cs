using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1_Attack : BaseState
{
    private Melee1 m1;

    public M1_Attack(StateMachine stateMachine, Melee1 monster) : base(stateMachine)
    {
        this.m1 = monster;
    }

    public override void Enter()
    {
        //m1.PlayAttackSound();
        m1.anim.SetBool("isAttack", true);
        if (m1.transform.position.x >= m1.player.position.x)
        {
            m1.sprite.flipX = true;
        }

        else if (m1.transform.position.x < m1.player.position.x)
        {
            m1.sprite.flipX = false;
        }
    }

    public override void Execute()
    {
        if(m1.anim.GetCurrentAnimatorStateInfo(0).IsName("M1_attack")
            && m1.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f)
        {
            stateMachine.ChangeState(new M1_Chase(stateMachine, m1));
        }
    }

    public override void Exit()
    {
        m1.attack.gameObject.SetActive(false);
        m1.canAttack = true;
        m1.attackCooldown = 1.5f;
        m1.anim.SetBool("isAttack", false);
    }
}

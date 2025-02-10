using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_Attack : BaseState
{
    private Elite1 e1;

    public E1_Attack(StateMachine stateMachine, Elite1 monster) : base(stateMachine)
    {
        this.e1 = monster;
    }

    public override void Enter()
    {
        if(e1.sprite.flipX)
        {
            e1.L_attack.SetActive(true);
        }

        if(e1.sprite.flipX)
        {
            e1.R_attack.SetActive(true);
        }

        e1.anim.SetBool("isAttack", true);
    }

    public override void Execute()
    {
        if(e1.anim.GetCurrentAnimatorStateInfo(0).IsName("E1_Attack") 
            && e1.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            stateMachine.ChangeState(new E1_Chase(stateMachine, e1));
        }
    }

    public override void Exit()
    {
        e1.anim.SetBool("isAttack", false);
        e1.L_attack.gameObject.SetActive(false);
        e1.R_attack.gameObject.SetActive(false);
        e1.canAttack = true;
        e1.attackCooldown = 3f;
    }
}

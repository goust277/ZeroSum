using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Attack : BaseState
{
    private Mission_melee mm;

    public MM_Attack(StateMachine stateMachine, Mission_melee monster) : base(stateMachine)
    {
        this.mm = monster;
    }

    public override void Enter()
    {
        //m1.PlayAttackSound();
        mm.anim.SetBool("isAttack", true);
        if (mm.transform.position.x >= mm.player.position.x)
        {
            mm.sprite.flipX = true;
        }

        else if (mm.transform.position.x < mm.player.position.x)
        {
            mm.sprite.flipX = false;
        }
    }

    public override void Execute()
    {
        if(mm.anim.GetCurrentAnimatorStateInfo(0).IsName("M1_attack")
            && mm.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f)
        {
            stateMachine.ChangeState(new MM_Chase(stateMachine, mm));
        }
    }

    public override void Exit()
    {
        mm.attack.gameObject.SetActive(false);
        mm.canAttack = true;
        mm.attackCooldown = 1.5f;
        mm.anim.SetBool("isAttack", false);
    }
}

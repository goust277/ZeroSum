using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1_Die : BaseState
{
    private Melee1 m1;

    public M1_Die(StateMachine stateMachine, Melee1 monster) : base(stateMachine)
    {
        this.m1 = monster;
    }

    public override void Enter()
    {
        m1.isDie = true;

        if ((m1.player.position.x - m1.transform.position.x) >= 0.2f)
        {
            m1.sprite.flipX = false;
        }

        if ((m1.player.position.x - m1.transform.position.x) <= 0.2f)
        {
            m1.sprite.flipX = true;
        }
        //m1.SpawnHitEffect();
        m1.anim.SetTrigger("isDie");
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = m1.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("M1_die") && stateInfo.normalizedTime >= 0.9f)
        {
            m1.gameObject.SetActive(false);
        }
    }
}

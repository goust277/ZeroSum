using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Die : BaseState
{
    private Mission_melee mm;

    public MM_Die(StateMachine stateMachine, Mission_melee monster) : base(stateMachine)
    {
        this.mm = monster;
    }

    public override void Enter()
    {
        mm.isDie = true;

        if ((mm.player.position.x - mm.transform.position.x) >= 0.2f)
        {
            mm.sprite.flipX = false;
        }

        if ((mm.player.position.x - mm.transform.position.x) <= 0.2f)
        {
            mm.sprite.flipX = true;
        }
        //m1.SpawnHitEffect();
        mm.anim.SetTrigger("isDie");
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = mm.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("M1_die") && stateInfo.normalizedTime >= 0.9f)
        {
            mm.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout_Die : BaseState
{
    private Scout scout;
    public Scout_Die(StateMachine stateMachine, Scout monster) : base(stateMachine)
    {
        this.scout = monster;
    }

    public override void Enter()
    {
        scout.isDie = true;
        if ((scout.player.position.x - scout.transform.position.x) >= 0.2f)
        {
            scout.sprite.flipX = true;
        }

        if ((scout.player.position.x - scout.transform.position.x) <= 0.2f)
        {
            scout.sprite.flipX = false;
        }
        scout.anim.SetTrigger("isDie");
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = scout.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Scout_die") && stateInfo.normalizedTime >= 0.98f)
        {
            scout.gameObject.SetActive(false);
        }
    }
    public override void Exit()
    {

    }
}

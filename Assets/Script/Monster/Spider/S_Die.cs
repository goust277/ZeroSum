using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Die : BaseState
{
    private Spider s;
    public S_Die(StateMachine stateMachine, Spider monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        s.gameObject.SetActive(false);

        //s.anim.SetTrigger("isDie");

        //if ((s.player.position.x - s.transform.position.x) >= 0.2f)
        //{
        //    s.sprite.flipX = true;
        //}

        //if ((s.player.position.x - s.transform.position.x) <= 0.2f)
        //{
        //    s.sprite.flipX = false;
        //}
    }

    public override void Execute()
    {
        //AnimatorStateInfo stateInfo = s.anim.GetCurrentAnimatorStateInfo(0);

        //if (stateInfo.IsName("S_Die") && stateInfo.normalizedTime >= 0.98f)
        //{
        //    s.gameObject.SetActive(false);
        //}
    }
}

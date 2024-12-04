using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class L_Die : BaseState
{
    private Long l;

    public L_Die(StateMachine stateMachine, Long monster) : base(stateMachine)
    {
        this.l = monster;
    }

    public override void Enter()
    {
        l.anim.SetTrigger("isDie");
        l.sprite.flipX = l.player.position.x > l.transform.position.x;
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = l.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("L_Die") && stateInfo.normalizedTime >= 0.98f)
        {
            l.gameObject.SetActive(false);
        }
    }
    public override void Exit()
    {

    }
}
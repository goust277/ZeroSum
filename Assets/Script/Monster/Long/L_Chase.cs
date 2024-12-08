using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Chase : BaseState
{
    private Long l;

    public L_Chase(StateMachine stateMachine, Long monster) : base(stateMachine)
    {
        this.l = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        l.anim.SetBool("isMove", true);
        l.currentTargetPosition = l.transform.position;
    }

    public override void Execute()
    {
        l.DistanceFromPlayer();

        if (l.attackRange > Mathf.Abs(l.player.position.x - l.transform.position.x))
        {
            if (l.CanEnterAttackState())
            {
                stateMachine.ChangeState(new L_Ready(stateMachine, l));
                return;
            }
        }
    }
    public override void Exit()
    {
        l.anim.SetBool("isMove", false);
    }
}

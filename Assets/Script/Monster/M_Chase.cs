using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Chase : BaseState
{
    private Melee m;

    public M_Chase(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        m.anim.SetTrigger("isRun");
    }

    public override void Execute()
    {
        if((m.player.position.x - m.transform.position.x) >= 0.2f)
        {
            m.sprite.flipX = false;
        }
        
        if((m.player.position.x - m.transform.position.x) <= 0.2f)
        {
            m.sprite.flipX = true;
        }

        if(m.attackRange > Mathf.Abs(m.player.position.x - m.transform.position.x))
        {
            if(m.CanEnterAttackState())
            {
                stateMachine.ChangeState(new M_Ready(stateMachine, m));
                return;
            }
        }

        Vector3 targetPosition = new Vector3(m.player.position.x, m.transform.position.y, 0);
        m.transform.position = Vector3.MoveTowards(m.transform.position, targetPosition, m.moveSpeed * Time.deltaTime);
    }
    public override void Exit()
    {
        m.anim.ResetTrigger("isRun");
    }
}
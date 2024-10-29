using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class M_Patrol : BaseState
{
    private Melee m;
    private Vector3 currentTarget;

    public M_Patrol(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
        currentTarget = new Vector3(monster.spawnPoint.x - monster.patrolRange, monster.transform.position.y, monster.transform.position.z);
    }

    public override void Enter()
    {
        Debug.Log("ผ๘ย๛ ป๓ลย");
        m.anim.SetTrigger("isRun");
        currentTarget = (m.transform.position.x < m.spawnPoint.x)
            ? new Vector3(m.spawnPoint.x + m.patrolRange, m.transform.position.y, m.transform.position.z) 
            : new Vector3(m.spawnPoint.x - m.patrolRange, m.transform.position.y, m.transform.position.z);
    }

    public override void Execute()
    {
        if (m.IsPlayerInRange())
        {
            stateMachine.ChangeState(new M_Chase(stateMachine, m));
            return;
        }

        if(m.transform.position.x < currentTarget.x)
        {
            m.sprite.flipX = false;
        }

        if (m.transform.position.x > currentTarget.x)
        {
            m.sprite.flipX = true;
        }

        m.transform.position = Vector3.MoveTowards(m.transform.position, currentTarget, m.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(m.transform.position, currentTarget) < 0.1f)
        {
            stateMachine.ChangeState(new M_Idle(stateMachine, m));
        }
    }
    public override void Exit()
    {
        m.anim.ResetTrigger("isRun");
    }
}

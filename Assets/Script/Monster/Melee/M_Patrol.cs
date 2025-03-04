using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class M_Patrol : BaseState
{
    private Melee m;

    public M_Patrol(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
        SetNextTarget();
    }

    public override void Enter()
    {
        Debug.Log("순찰 상태");
        m.anim.SetBool("isWalk", true);
        SetNextTarget();
    }

    public override void Execute()
    {

        if (m.isPlayerInRange)
        {
            stateMachine.ChangeState(new M_Chase(stateMachine, m));
            return;
        }

        if(m.transform.position.x < m.currentTarget.x)
        {
            m.anim.SetFloat("Dir", 1);
        }

        if (m.transform.position.x > m.currentTarget.x)
        {
            m.anim.SetFloat("Dir", -1);
        }

        m.transform.position = Vector3.MoveTowards(m.transform.position, m.currentTarget, m.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(m.transform.position, m.currentTarget) < 0.1f)
        {
            stateMachine.ChangeState(new M_Idle(stateMachine, m));
        }
    }
    public override void Exit()
    {
        m.anim.SetBool("isWalk", false);
    }

    // 목표 지점 설정
    private void SetNextTarget()
    {
        m.currentTarget = (m.transform.position.x < m.spawnPoint.x)
            ? new Vector3(m.spawnPoint.x + m.patrolRange, m.transform.position.y, m.transform.position.z)
            : new Vector3(m.spawnPoint.x - m.patrolRange, m.transform.position.y, m.transform.position.z);
    }
}

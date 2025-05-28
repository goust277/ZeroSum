using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1_Patrol : BaseState
{
    private Melee1 m1;

    public M1_Patrol(StateMachine stateMachine, Melee1 monster) : base(stateMachine)
    {
        this.m1 = monster;
        SetNextTarget();
    }

    public override void Enter()
    {
        Debug.Log("ผ๘ย๛ ป๓ลย");
        m1.PlayMoveSound(1.0f);
        m1.anim.SetBool("isWalk", true);
        m1.seeMark = false;
        SetNextTarget();
    }

    public override void Execute()
    {
        if (m1.isPlayerInRange)
        {
            stateMachine.ChangeState(new M1_Chase(stateMachine, m1));
            return;
        }

        if (m1.turn)
        {
            SetNextTarget();
            m1.turn = false;
        }

        if (m1.transform.position.x < m1.currentTarget.x)
        {
            m1.sprite.flipX = false;
        }

        if (m1.transform.position.x > m1.currentTarget.x)
        {
            m1.sprite.flipX = true;
        }

        m1.transform.position = Vector3.MoveTowards(m1.transform.position, m1.currentTarget, m1.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(m1.transform.position, m1.currentTarget) < 0.1f)
        {
            stateMachine.ChangeState(new M1_Idle(stateMachine, m1));
        }
    }
    public override void Exit()
    {
        m1.StopMoveSound();
        m1.anim.SetBool("isWalk", false);
    }

    private void SetNextTarget()
    {
        m1.currentTarget = (m1.transform.position.x < m1.spawnPoint.x)
            ? new Vector3(m1.spawnPoint.x + m1.patrolRange, m1.transform.position.y, m1.transform.position.z)
            : new Vector3(m1.spawnPoint.x - m1.patrolRange, m1.transform.position.y, m1.transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Patrol : BaseState
{
    private Tanker tanker;

    public T_Patrol(StateMachine stateMachine, Tanker monster) : base(stateMachine)
    {
        this.tanker = monster;
        SetNextTarget();
    }

    public override void Enter()
    {
        Debug.Log("순찰 상태");
        tanker.anim.SetBool("isWalk", true);
        SetNextTarget();
    }

    public override void Execute()
    {
        if (tanker.isPlayerInRange)
        {
            stateMachine.ChangeState(new T_Chase(stateMachine, tanker));
            return;
        }

        if (tanker.transform.position.x < tanker.currentTarget.x)
        {
            tanker.sprite.flipX = true;
            tanker.detect.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if (tanker.transform.position.x > tanker.currentTarget.x)
        {
            tanker.sprite.flipX = false;
            tanker.detect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }


        if (tanker.turn)
        {
            SetNextTarget();
            tanker.turn = false;
        }

        tanker.transform.position = Vector3.MoveTowards(tanker.transform.position, tanker.currentTarget, tanker.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(tanker.transform.position, tanker.currentTarget) < 0.1f)
        {
            stateMachine.ChangeState(new T_Idle(stateMachine, tanker));
        }
    }
    public override void Exit()
    {
        tanker.anim.SetBool("isWalk", false);
    }

    // 목표 지점 설정
    private void SetNextTarget()
    {
        tanker.currentTarget = (tanker.transform.position.x < tanker.spawnPoint.x)
            ? new Vector3(tanker.spawnPoint.x + tanker.patrolRange, tanker.transform.position.y, tanker.transform.position.z)
            : new Vector3(tanker.spawnPoint.x - tanker.patrolRange, tanker.transform.position.y, tanker.transform.position.z);
    }
}

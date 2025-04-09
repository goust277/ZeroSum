using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Scout_Patrol : BaseState
{
    private Scout scout;

    public Scout_Patrol(StateMachine stateMachine, Scout monster) : base(stateMachine)
    {
        this.scout = monster;
        SetNextTarget();
    }

    public override void Enter()
    {
        Debug.Log("순찰 상태");
        scout.anim.SetBool("isWalk", true);
        SetNextTarget();
    }

    public override void Execute()
    {
        if (scout.isPlayerInRange)
        {
            stateMachine.ChangeState(new Scout_Chase(stateMachine, scout));
            return;
        }

        if (scout.transform.position.x < scout.currentTarget.x)
        {
            scout.sprite.flipX = true;
            scout.detect.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if (scout.transform.position.x > scout.currentTarget.x)
        {
            scout.sprite.flipX = false;
            scout.detect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }


        if (scout.turn)
        {
            SetNextTarget();
            scout.turn = false;
        }

        scout.transform.position = Vector3.MoveTowards(scout.transform.position, scout.currentTarget, scout.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(scout.transform.position, scout.currentTarget) < 0.1f)
        {
            stateMachine.ChangeState(new Scout_Idle(stateMachine, scout));
        }
    }
    public override void Exit()
    {
        scout.anim.SetBool("isWalk", false);
    }

    // 목표 지점 설정
    private void SetNextTarget()
    {
        scout.currentTarget = (scout.transform.position.x < scout.spawnPoint.x)
            ? new Vector3(scout.spawnPoint.x + scout.patrolRange, scout.transform.position.y, scout.transform.position.z)
            : new Vector3(scout.spawnPoint.x - scout.patrolRange, scout.transform.position.y, scout.transform.position.z);
    }
}

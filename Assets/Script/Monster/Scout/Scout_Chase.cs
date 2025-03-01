using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout_Chase : BaseState
{
    private Scout scout;

    public Scout_Chase(StateMachine stateMachine, Scout monster) : base(stateMachine)
    {
        this.scout = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        scout.anim.SetBool("isWalk", true);
    }

    public override void Execute()
    {
        if (!scout.isPlayerInRange)
        {
            stateMachine.ChangeState(new Scout_Patrol(stateMachine, scout));
            return;
        }

        if ((scout.player.position.x - scout.transform.position.x) >= 0.2f)
        {
            scout.sprite.flipX = true;
            scout.detect.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if ((scout.player.position.x - scout.transform.position.x) <= 0.2f)
        {
            scout.sprite.flipX = false;
            scout.detect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (scout.attackRange > Mathf.Abs(scout.player.position.x - scout.transform.position.x))
        {
            if (scout.CanEnterAttackState())
            {
                stateMachine.ChangeState(new Scout_Ready(stateMachine, scout));
                return;
            }
        }

        Vector3 targetPosition = new Vector3(scout.player.position.x, scout.transform.position.y, 0);
        scout.transform.position = Vector3.MoveTowards(scout.transform.position, targetPosition, scout.moveSpeed * Time.deltaTime);
    }
    public override void Exit()
    {
        scout.anim.SetBool("isWalk", false);
    }
}

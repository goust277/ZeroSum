using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Patrol : BaseState
{
    private Long l;
    private Vector3 currentTarget;

    public L_Patrol(StateMachine stateMachine, Long monster) : base(stateMachine)
    {
        this.l = monster;
        currentTarget = new Vector3(monster.spawnPoint.x - monster.patrolRange, monster.transform.position.y, monster.transform.position.z);
    }

    public override void Enter()
    {
        Debug.Log("ผ๘ย๛ ป๓ลย");
        l.anim.SetBool("isMove", true);
        currentTarget = (l.transform.position.x < l.spawnPoint.x)
            ? new Vector3(l.spawnPoint.x + l.patrolRange, l.transform.position.y, l.transform.position.z)
            : new Vector3(l.spawnPoint.x - l.patrolRange, l.transform.position.y, l.transform.position.z);
    }

    public override void Execute()
    {
        if (l.isPlayerInRange)
        {
            stateMachine.ChangeState(new L_Chase(stateMachine, l));
            return;
        }

        if (l.transform.position.x < currentTarget.x)
        {
            //l.anim.SetFloat("Dir", 1);
            l.sprite.flipX = true;
        }

        if (l.transform.position.x > currentTarget.x)
        {
            //l.anim.SetFloat("Dir", -1);
            l.sprite.flipX = false;
        }

        l.transform.position = Vector3.MoveTowards(l.transform.position, currentTarget, l.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(l.transform.position, currentTarget) < 0.1f)
        {
            stateMachine.ChangeState(new L_Idle(stateMachine, l));
        }
    }
    public override void Exit()
    {
        l.anim.SetBool("isMove", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Patrol : BaseState
{
    private Spider s;

    public S_Patrol(StateMachine stateMachine, Spider monster) : base(stateMachine)
    {
        this.s = monster;
        SetNextTarget();
    }

    public override void Enter()
    {
        Debug.Log("ผ๘ย๛ ป๓ลย");
        s.PlayMoveSound(1.0f);
        s.anim.SetBool("isWalk", true);
        SetNextTarget();
    }

    public override void Execute()
    {
        if (s.isPlayerInRange)
        {
            stateMachine.ChangeState(new S_Chase(stateMachine, s));
            return;
        }

        if (s.transform.position.x < s.currentTarget.x)
        {
            s.sprite.flipX = true;
        }

        if (s.transform.position.x > s.currentTarget.x)
        {
            s.sprite.flipX = false;
        }

        s.transform.position = Vector3.MoveTowards(s.transform.position, s .currentTarget, s.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(s.transform.position, s.currentTarget) < 0.1f)
        {
            stateMachine.ChangeState(new S_Idle(stateMachine, s));
        }
    }
    public override void Exit()
    {
        s.StopMoveSound();
        s.anim.SetBool("isWalk", false);
    }

    private void SetNextTarget()
    { 
        s.currentTarget = (s.transform.position.x < s.spawnPoint.x)
            ? new Vector3(s.spawnPoint.x + s.patrolRange, s.transform.position.y, s.transform.position.z)
            : new Vector3(s.spawnPoint.x - s.patrolRange, s.transform.position.y, s.transform.position.z);
    }
}

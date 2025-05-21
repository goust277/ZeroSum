using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_Patrol : BaseState
{
    private Summoner summoner;

    public Summoner_Patrol(StateMachine stateMachine, Summoner monster) : base(stateMachine)
    {
        this.summoner = monster;
        SetNextTarget();
    }

    public override void Enter()
    {
        Debug.Log("¼øÂû »óÅÂ");
        summoner.PlayMoveSound(1.0f);
        summoner.anim.SetBool("isWalk", true);
        SetNextTarget();
    }

    public override void Execute()
    {

        if (summoner.isPlayerInRange)
        {
            stateMachine.ChangeState(new Summoner_Chase(stateMachine, summoner));
            return;
        }

        if (summoner.transform.position.x < summoner.currentTarget.x)
        {
            summoner.sprite.flipX = true;
            summoner.detect.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if (summoner.transform.position.x > summoner.currentTarget.x)
        {
            summoner.sprite.flipX = false;
            summoner.detect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (summoner.turn)
        {
            SetNextTarget();
            summoner.turn = false;
        }

        summoner.transform.position = Vector3.MoveTowards(summoner.transform.position, summoner.currentTarget, summoner.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(summoner.transform.position, summoner.currentTarget) < 0.1f)
        {
            stateMachine.ChangeState(new Summoner_Idle(stateMachine, summoner));
        }
    }
    public override void Exit()
    {
        summoner.anim.SetBool("isWalk", false);
    }

    private void SetNextTarget()
    {
        summoner.currentTarget = (summoner.transform.position.x < summoner.spawnPoint.x)
            ? new Vector3(summoner.spawnPoint.x + summoner.patrolRange, summoner.transform.position.y, summoner.transform.position.z)
            : new Vector3(summoner.spawnPoint.x - summoner.patrolRange, summoner.transform.position.y, summoner.transform.position.z);
    }
}

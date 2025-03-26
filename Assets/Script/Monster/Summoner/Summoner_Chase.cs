using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_Chase : BaseState
{
    private Summoner summoner;
    public Summoner_Chase(StateMachine stateMachine, Summoner monster) : base(stateMachine)
    {
        this.summoner = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        summoner.anim.SetBool("isChase", true);
    }

    public override void Execute()
    {
        if (!summoner.isPlayerInRange)
        {
            stateMachine.ChangeState(new Summoner_Patrol(stateMachine, summoner));
            return;
        }

        if (summoner.transform.position.x < summoner.player.position.x)
        {
            summoner.sprite.flipX = true;
        }

        if (summoner.transform.position.x > summoner.player.position.x)
        {
            summoner.sprite.flipX = false;
        }

        if (summoner.attackRange > Mathf.Abs(summoner.player.position.x - summoner.transform.position.x))
        {
            if (summoner.CanEnterAttackState())
            {
                stateMachine.ChangeState(new Summoner_Attack(stateMachine, summoner));
                return;
            }
        }
    }

    public override void Exit()
    {
        summoner.anim.SetBool("isChase", false);
    }
}

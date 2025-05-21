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
        if (Mathf.Abs(summoner.player.transform.position.x - summoner.transform.position.x) >= 9f
            || Mathf.Abs(summoner.player.transform.position.y - summoner.transform.position.y) >= 2f)
        {
            stateMachine.ChangeState(new Summoner_Idle(stateMachine, summoner));
            return;
        }

        if (summoner.transform.position.x < summoner.player.position.x)
        {
            summoner.sprite.flipX = true;
            summoner.detect.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if (summoner.transform.position.x > summoner.player.position.x)
        {
            summoner.sprite.flipX = false;
            summoner.detect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (summoner.attackRange > Mathf.Abs(summoner.player.position.x - summoner.transform.position.x))
        {
            if (summoner.CanEnterAttackState())
            {
                if (summoner.canLAttack)
                {
                    summoner.canLAttack = false;
                    stateMachine.ChangeState(new Summoner_L_atk(stateMachine, summoner));
                }
                else
                {
                    summoner.canLAttack = true;
                    stateMachine.ChangeState(new Summoner_Attack(stateMachine, summoner));
                }
                return;
            }
        }
    }

    public override void Exit()
    {
        summoner.anim.SetBool("isChase", false);
    }
}

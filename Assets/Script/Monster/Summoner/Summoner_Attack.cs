using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_Attack : BaseState
{
    private Summoner summoner;

    public Summoner_Attack(StateMachine stateMachine, Summoner monster) : base(stateMachine)
    {
        this.summoner = monster;
    }

    public override void Enter()
    {
        summoner.PlayAttackSound();
        summoner.anim.SetBool("isAttack", true);
        if (summoner.transform.position.x >= summoner.player.position.x)
        {
            summoner.sprite.flipX = false;
        }

        else if (summoner.transform.position.x < summoner.player.position.x)
        {
            summoner.sprite.flipX = true;
        }
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = summoner.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Summoner_attack") && stateInfo.normalizedTime >= 0.80f)
        {
            stateMachine.ChangeState(new Summoner_Chase(stateMachine, summoner));
            return;
        }
    }

    public override void Exit()
    {
        summoner.canAttack = true;
        summoner.attackCooldown = 2f;
        summoner.seeMark = true;
        summoner.anim.SetBool("isAttack", false);
    }
}

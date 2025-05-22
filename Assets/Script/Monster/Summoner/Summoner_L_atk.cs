using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_L_atk : BaseState
{
    private Summoner summoner;

    public Summoner_L_atk(StateMachine stateMachine, Summoner monster) : base(stateMachine)
    {
        this.summoner = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격 상태");

        summoner.anim.SetBool("isLAttack", true);
    }

    public override void Execute()
    {
        if (summoner.anim.GetCurrentAnimatorStateInfo(0).IsName("Summoner_L_attack"))
        {
            if (summoner.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                stateMachine.ChangeState(new Summoner_Chase(stateMachine, summoner));
                return;
            }
        }
    }
    public override void Exit()
    {
        summoner.canAttack = true;
        summoner.attackCooldown = 3f;
        summoner.seeMark = true;
        summoner.anim.SetBool("isLAttack", false);
    }
}

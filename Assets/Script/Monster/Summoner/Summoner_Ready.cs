using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_Ready : BaseState
{
    private Summoner summoner;

    public Summoner_Ready(StateMachine stateMachine, Summoner monster) : base(stateMachine)
    {
        this.summoner = monster;
    }

    public override void Enter()
    {
        summoner.rb.velocity = Vector2.zero;
        summoner.anim.SetBool("isReady", true);
    }

    public override void Execute()
    {
        AnimatorStateInfo stateInfo = summoner.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("S_ready") && stateInfo.normalizedTime >= 0.75f)
        {
            stateMachine.ChangeState(new Summoner_Attack(stateMachine, summoner));
            return;
        }
    }

    public override void Exit()
    {
        summoner.anim.SetBool("isReady", false);
    }
}

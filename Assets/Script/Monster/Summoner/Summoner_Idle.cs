using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_Idle : BaseState
{
    private Summoner summoner;
    private float timer = 0f;

    public Summoner_Idle(StateMachine stateMachine, Summoner monster) : base(stateMachine)
    {
        this.summoner = monster;
    }

    public override void Enter()
    {
        Debug.Log("대기 상태");
        timer = 3f;
        summoner.StopMoveSound();
        summoner.anim.SetBool("isIdle", true);
    }

    public override void Execute()
    {
        if(isFreeze())
        {
            return;
        }
        
        timer -= Time.deltaTime;

        if (summoner.isPlayerInRange)
        {
            stateMachine.ChangeState(new Summoner_Chase(stateMachine, summoner));
            return;
        }

        if (timer <= 0f)
        {
            stateMachine.ChangeState(new Summoner_Patrol(stateMachine, summoner));
        }
    }
    public override void Exit()
    {
        summoner.anim.SetBool("isIdle", false);
    }

    bool isFreeze()
    {
        return (summoner.rb.constraints & RigidbodyConstraints2D.FreezePositionX) == RigidbodyConstraints2D.FreezePositionX;
    }
}

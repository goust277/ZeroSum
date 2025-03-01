using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout_Attack : BaseState
{
    private Scout scout;

    public Scout_Attack(StateMachine stateMachine, Scout monster) : base(stateMachine)
    {
        this.scout = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격 상태");

        scout.anim.SetBool("isAttack", true);

        scout.fireCount = 0; // 발사 횟수 초기화
    }

    public override void Execute()
    {
        if (scout.fireCount >= scout.maxFireCount)
        {
            stateMachine.ChangeState(new Scout_Chase(stateMachine, scout));
            return;
        }
    }
    public override void Exit()
    {
        scout.anim.SetBool("isAttack", false);
        scout.canAttack = true;
        scout.canShot = false;
        scout.attackCooldown = 3f;
    }
}

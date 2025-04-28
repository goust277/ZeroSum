using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Attack : BaseState
{
    private Tanker tanker;
    private float fireTimer = 0f;

    public T_Attack(StateMachine stateMachine, Tanker monster) : base(stateMachine)
    {
        this.tanker = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격 상태");

        tanker.anim.SetBool("isAttack", true);

        tanker.fireCount = 0;
        fireTimer = 0f;
    }

    public override void Execute()
    {
        if (!tanker.canShot)
            return;

        fireTimer += Time.deltaTime;

        if (tanker.fireCount < tanker.maxFireCount)
        {
            if (fireTimer >= tanker.fireInterval)
            {
                tanker.FireBullet();
                tanker.fireCount++;
                fireTimer = 0f;
            }
        }
        else
        {
            if (tanker.anim.GetCurrentAnimatorStateInfo(0).IsName("T_attack_end") &&
                tanker.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                stateMachine.ChangeState(new T_Chase(stateMachine, tanker));
            }
        }
    }
    public override void Exit()
    {
        tanker.anim.SetBool("isAttack", false);
        tanker.canAttack = true;
        tanker.canShot = false;
        tanker.attackCooldown = 3f;
    }
}

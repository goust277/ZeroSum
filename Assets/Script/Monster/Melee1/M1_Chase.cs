using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1_Chase : BaseState
{
    private Melee1 m1;
    private float timer = 1.0f;

    public M1_Chase(StateMachine stateMachine, Melee1 monster) : base(stateMachine)
    {
        this.m1 = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        m1.PlayMoveSound(1.4f);
        m1.moveSpeed = 3f;
        if (!m1.seeMark)
        {
            m1.seeMark = true;
            m1.mark.gameObject.SetActive(true);
        }
        m1.anim.SetBool("isRun", true);
    }

    public override void Execute()
    {
        if (m1.turn)
        {
            stateMachine.ChangeState(new M1_Idle(stateMachine, m1));
            m1.turn = false;
            m1.wait_T = 1f;
            m1.canAttack = true;
            m1.attackCooldown = 1.5f;
            return;
        }

        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            m1.mark.gameObject.SetActive(false);
        }

        if (m1.transform.position.x < m1.player.position.x)
        {
            m1.sprite.flipX = false;
        }

        if (m1.transform.position.x > m1.player.position.x)
        {
            m1.sprite.flipX = true;
        }

        if (Mathf.Abs(m1.player.transform.position.y - m1.transform.position.y) >= 2f)
        {
            stateMachine.ChangeState(new M1_Patrol(stateMachine, m1));
            return;
        }

        if (m1.attackRange > Mathf.Abs(m1.player.position.x - m1.transform.position.x))
        {
            if (m1.CanEnterAttackState())
            {
                stateMachine.ChangeState(new M1_Attack(stateMachine, m1));
                return;
            }
        }
        AnimatorStateInfo stateInfo = m1.anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("M1_run"))
        {
            Vector3 targetPosition = new Vector3(m1.player.position.x, m1.transform.position.y, 0);
            m1.transform.position = Vector3.MoveTowards(m1.transform.position, targetPosition, m1.moveSpeed * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        m1.StopMoveSound();
        m1.anim.SetBool("isRun", false);
        timer = 1.0f;
        m1.moveSpeed = 2;
    }
}

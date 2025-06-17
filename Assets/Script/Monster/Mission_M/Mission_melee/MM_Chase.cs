using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Chase : BaseState
{
    private Mission_melee mm;
    private float timer = 1.0f;

    public MM_Chase(StateMachine stateMachine, Mission_melee monster) : base(stateMachine)
    {
        this.mm = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        mm.PlayMoveSound(1.4f);
        mm.moveSpeed = 3f;
        if (!mm.seeMark)
        {
            mm.seeMark = true;
            mm.mark.gameObject.SetActive(true);
        }
        mm.anim.SetBool("isRun", true);
    }

    public override void Execute()
    {
        if (mm.turn)
        {
            stateMachine.ChangeState(new MM_Idle(stateMachine, mm));
            mm.turn = false;
            mm.wait_T = 1f;
            mm.canAttack = true;
            mm.attackCooldown = 1.5f;
            return;
        }

        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            mm.mark.gameObject.SetActive(false);
        }

        if (mm.transform.position.x < mm.player.position.x)
        {
            mm.sprite.flipX = false;
        }

        if (mm.transform.position.x > mm.player.position.x)
        {
            mm.sprite.flipX = true;
        }

        if (mm.attackRange > Mathf.Abs(mm.player.position.x - mm.transform.position.x))
        {
            if (mm.CanEnterAttackState())
            {
                stateMachine.ChangeState(new MM_Attack(stateMachine, mm));
                return;
            }
        }
        AnimatorStateInfo stateInfo = mm.anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("M1_run"))
        {
            Vector3 targetPosition = new Vector3(mm.player.position.x, mm.transform.position.y, 0);
            mm.transform.position = Vector3.MoveTowards(mm.transform.position, targetPosition, mm.moveSpeed * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        mm.StopMoveSound();
        mm.anim.SetBool("isRun", false);
        timer = 1.0f;
        mm.moveSpeed = 2;
    }
}

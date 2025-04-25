using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class T_Chase : BaseState
{
    Tanker tanker;

    public T_Chase(StateMachine stateMachine, Tanker monster) : base(stateMachine)
    {
        this.tanker = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        tanker.anim.SetBool("isWalk", true);
        tanker.moveSpeed = 2.5f;
    }

    public override void Execute()
    {
        if (tanker.anim.GetCurrentAnimatorStateInfo(0).IsName("T_attack_end")
            && tanker.anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.85f)
        {
            return;
        }

        if (Mathf.Abs(tanker.player.transform.position.x - tanker.transform.position.x) >= 9f
            || Mathf.Abs(tanker.player.transform.position.y - tanker.transform.position.y) >= 2f)
        {
            stateMachine.ChangeState(new T_Patrol(stateMachine, tanker));
            return;
        }

        if ((tanker.player.position.x - tanker.transform.position.x) >= 0.2f)
        {
            tanker.sprite.flipX = true;
            tanker.detect.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if ((tanker.player.position.x - tanker.transform.position.x) <= 0.2f)
        {
            tanker.sprite.flipX = false;
            tanker.detect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (tanker.attackRange > Mathf.Abs(tanker.player.position.x - tanker.transform.position.x))
        {
            if (tanker.CanEnterAttackState())
            {
                stateMachine.ChangeState(new T_Ready(stateMachine, tanker));
                return;
            }
        }
        //float offsetX = tanker.sprite.flipX ? -3f : 3f;

        Vector3 targetPosition = new Vector3(tanker.player.position.x , tanker.transform.position.y, 0);
        tanker.transform.position = Vector3.MoveTowards(tanker.transform.position, targetPosition, tanker.moveSpeed * Time.deltaTime);
    }
    public override void Exit()
    {
        tanker.anim.SetBool("isWalk", false);
        tanker.moveSpeed = 2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_Chase : BaseState
{
    private Elite1 e1;

    public E1_Chase(StateMachine stateMachine, Elite1 monster) : base(stateMachine)
    {
        this.e1 = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        e1.anim.SetBool("isWalk", true);
    }

    public override void Execute()
    {
        if ((e1.player.position.x - e1.transform.position.x) >= 0.2f)
        {
            e1.sprite.flipX = false;
        }

        if ((e1.player.position.x - e1.transform.position.x) <= 0.2f)
        {
            e1.sprite.flipX = true;
        }

        if (e1.attackRange > Mathf.Abs(e1.player.position.x - e1.transform.position.x))
        {
            if (e1.CanEnterAttackState())
            {
                stateMachine.ChangeState(new E1_Attack(stateMachine, e1));
                return;
            }
        }

        Vector3 targetPosition = new Vector3(e1.player.position.x, e1.transform.position.y, 0);
        e1.transform.position = Vector3.MoveTowards(e1.transform.position, targetPosition, e1.moveSpeed * Time.deltaTime);
    }
    public override void Exit()
    {
        e1.anim.SetBool("isWalk", false);
    }
}

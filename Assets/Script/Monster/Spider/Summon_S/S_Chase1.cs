using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Chase1 : BaseState
{
    private Summon_S s;
    public S_Chase1(StateMachine stateMachine, Summon_S monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        s.anim.SetBool("isWalk", true);
    }

    public override void Execute()
    {
        if (s.transform.position.x < s.currentTarget.x)
        {
            s.sprite.flipX = true;
        }

        if (s.transform.position.x > s.currentTarget.x)
        {
            s.sprite.flipX = false;
        }

        if (s.attackRange > Mathf.Abs(s.player.position.x - s.transform.position.x))
        {
            if (s.CanEnterAttackState())
            {
                stateMachine.ChangeState(new S_Ready1(stateMachine, s));
                return;
            }
        }
        AnimatorStateInfo stateInfo = s.anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("S_walk"))
        {
            Vector3 targetPosition = new Vector3(s.player.position.x, s.transform.position.y, 0);
            s.transform.position = Vector3.MoveTowards(s.transform.position, targetPosition, s.moveSpeed * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        s.anim.SetBool("isWalk", false);
    }
}

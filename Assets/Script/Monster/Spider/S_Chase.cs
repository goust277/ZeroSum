using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Chase : BaseState
{
    private Spider s;
    private float timer = 1f;
    public S_Chase(StateMachine stateMachine, Spider monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        if(!s.seeMark)
        s.mark.gameObject.SetActive(true);
        s.anim.SetBool("isWalk", true);
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            s.mark.gameObject.SetActive(false);
        }

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
                stateMachine.ChangeState(new S_Ready(stateMachine, s));
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

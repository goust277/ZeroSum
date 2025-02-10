using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_Special_Attack : BaseState
{
    private Elite1 e1;

    public E1_Special_Attack(StateMachine stateMachine, Elite1 monster) : base(stateMachine)
    {
        this.e1 = monster;
    }

    public override void Enter()
    {
        e1.anim.SetTrigger("isSpecial_A");
        //플레이어와 거리 확인해서 패턴 바꾸거나 벽으로 이동
    }

    public override void Execute()
    {
        if (e1.anim.GetCurrentAnimatorStateInfo(0).IsName("E1_Special_Attack")
    && e1.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f)
        {
            stateMachine.ChangeState(new E1_Chase(stateMachine, e1));
        }
    }

    public override void Exit()
    {
        e1.test = false;
        e1.attackCooldown = 2.5f;
    }

    public void collider()
    {
        e1.S_attack.gameObject.SetActive(true);
    }
}

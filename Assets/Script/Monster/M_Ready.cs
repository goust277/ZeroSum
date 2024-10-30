using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class M_Ready : BaseState
{
    private Melee m;
    private Vector3 targetPosition;

    public M_Ready(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격준비 상태");

        m.isDashing = true;
        m.anim.SetTrigger("isReady");

        if (!m.sprite.flipX)
        {
            //오른쪽
            targetPosition = new Vector3(m.transform.position.x + m.dashRange, m.transform.position.y, 0);
        }

        if (m.sprite.flipX)
        {
            //왼쪽
            targetPosition = new Vector3(m.transform.position.x - m.dashRange, m.transform.position.y, 0);
        }
    }

    public override void Execute()
    {
        if(m.touchPlayer)
        {
            stateMachine.ChangeState(new M_Attack(stateMachine, m));
            m.touchPlayer = false;
            return;
        }
        
        if (Mathf.Abs(m.transform.position.x) >= Mathf.Abs(targetPosition.x))
        {
            stateMachine.ChangeState(new M_Attack(stateMachine, m));
            return;
        }

        m.sprite.color = Color.red;
        m.transform.position = Vector3.MoveTowards(m.transform.position, targetPosition, m.moveSpeed * 2f * Time.deltaTime);
    }

    public override void Exit()
    {
        m.isDashing = false;
        m.anim.ResetTrigger("isReady");
        m.sprite.color = Color.white;
    }
}

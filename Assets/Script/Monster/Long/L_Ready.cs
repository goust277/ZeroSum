using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Ready : BaseState
{
    private Long l;

    public L_Ready(StateMachine stateMachine, Long monster) : base(stateMachine)
    {
        this.l = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격준비 상태");

        l.anim.SetBool("isAttack", true);

        if (l.laser != null)
        {
            l.laser.enabled = true;
            l.laser.positionCount = 2;
        }
    }

    public override void Execute()
    {
        // 레이저 시작 위치를 flipX에 따라 설정
        SpriteRenderer spriteRenderer = l.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            l.laserStart = spriteRenderer.flipX ? l.rightFirePoint.position : l.leftFirePoint.position; 
        }

        // 레이저 끝 위치는 플레이어 위치
        l.laserEnd = l.player.position;

        // 레이저 업데이트
        if (l.laser != null)
        {
            l.laser.SetPosition(0, l.laserStart);
            l.laser.SetPosition(1, l.laserEnd);
        }

        if(l.canShot)
        {
            stateMachine.ChangeState(new L_Attack(stateMachine, l));
            return;
        }
    }

    public override void Exit()
    {
        // 레이저 비활성화
        if (l.laser != null)
        {
            l.laser.positionCount = 0; // 포인트 카운트를 0으로 설정
            l.laser.enabled = false; // 비활성화
        }
    }
}

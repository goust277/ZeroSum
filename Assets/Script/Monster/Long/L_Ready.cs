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
        Debug.Log("�����غ� ����");

        l.anim.SetBool("isAttack", true);

        if (l.laser != null)
        {
            l.laser.enabled = true;
            l.laser.positionCount = 2;
        }
    }

    public override void Execute()
    {
        // ������ ���� ��ġ�� flipX�� ���� ����
        SpriteRenderer spriteRenderer = l.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            l.laserStart = spriteRenderer.flipX ? l.rightFirePoint.position : l.leftFirePoint.position; 
        }

        // ������ �� ��ġ�� �÷��̾� ��ġ
        l.laserEnd = l.player.position;

        // ������ ������Ʈ
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
        // ������ ��Ȱ��ȭ
        if (l.laser != null)
        {
            l.laser.positionCount = 0; // ����Ʈ ī��Ʈ�� 0���� ����
            l.laser.enabled = false; // ��Ȱ��ȭ
        }
    }
}

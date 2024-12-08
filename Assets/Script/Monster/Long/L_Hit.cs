using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Hit : BaseState
{
    private Long l;
    private float blinkDuration = 0.5f; // �����Ÿ��� �� �ð�
    private float blinkInterval = 0.1f; // �����Ÿ��� ����
    private float elapsedBlinkTime = 0f; // �����Ÿ� ���� �ð�
    private float intervalTimer = 0f; // ���� Ÿ�̸�
    private bool isBlinking = false;

    public L_Hit(StateMachine stateMachine, Long monster) : base(stateMachine)
    {
        this.l = monster;
    }

    public override void Enter()
    {
        l.isHit = true;
        elapsedBlinkTime = 0f;
        intervalTimer = 0f;
        isBlinking = true;

        SetSpriteAlpha(0.5f);
    }

    public override void Execute()
    {
        if (isBlinking)
        {
            elapsedBlinkTime += Time.deltaTime;
            intervalTimer += Time.deltaTime;

            if (intervalTimer >= blinkInterval)
            {
                ToggleSpriteAlpha();
                intervalTimer = 0f; // ���� Ÿ�̸� �ʱ�ȭ
            }

            if (elapsedBlinkTime >= blinkDuration)
            {
                // �����Ÿ� ����
                isBlinking = false;
                SetSpriteAlpha(1f);
                stateMachine.ChangeState(new L_Chase(stateMachine, l));
            }
        }
    }
    public override void Exit()
    {
        isBlinking = false;
        SetSpriteAlpha(1f); // ���� ���� �� ���İ� ����
        l.isHit = false;
        l.attackCooldown = 3f;
        l.canAttack = true;
    }

    // ��������Ʈ�� ���İ��� �����ϴ� �޼���
    private void SetSpriteAlpha(float alpha)
    {
        if (l.sprite != null)
        {
            Color color = l.sprite.color;
            color.a = alpha;
            l.sprite.color = color;
        }
    }

    // ��������Ʈ ���İ��� ����ϴ� �޼���
    private void ToggleSpriteAlpha()
    {
        if (l.sprite != null)
        {
            Color color = l.sprite.color;
            color.a = (color.a == 1f) ? 0.5f : 1f; // 1f�� 0.5f ���̸� ���
            l.sprite.color = color;
        }
    }
}

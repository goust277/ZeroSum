using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Hit : BaseState
{
    private Melee m;
    private float blinkDuration = 0.5f; // 깜빡거리는 총 시간
    private float blinkInterval = 0.1f; // 깜빡거리는 간격
    public float elapsedBlinkTime = 0f; // 깜빡거림 지속 시간
    private float intervalTimer = 0f; // 간격 타이머
    private bool isBlinking = false;

    public M_Hit(StateMachine stateMachine, Melee monster) : base(stateMachine)
    {
        this.m = monster;
    }

    public override void Enter()
    {
        m.isHit = true;
        m.rb.velocity = Vector2.zero;
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
                intervalTimer = 0f; // 간격 타이머 초기화
            }

            if (elapsedBlinkTime >= blinkDuration)
            {
                // 깜빡거림 종료
                isBlinking = false;
                SetSpriteAlpha(1f);
                stateMachine.ChangeState(new M_Chase(stateMachine, m));
            }
        }
    }
    public override void Exit()
    {
        isBlinking = false;
        SetSpriteAlpha(1f); // 상태 종료 시 알파값 복원
        m.isHit = false;
        m.attackCooldown = 3f;
        m.canAttack = true;
    }

    // 스프라이트의 알파값을 설정하는 메서드
    private void SetSpriteAlpha(float alpha)
    {
        if (m.sprite != null)
        {
            Color color = m.sprite.color;
            color.a = alpha;
            m.sprite.color = color;
        }
    }

    // 스프라이트 알파값을 토글하는 메서드
    private void ToggleSpriteAlpha()
    {
        if (m.sprite != null)
        {
            Color color = m.sprite.color;
            color.a = (color.a == 1f) ? 0.5f : 1f; // 1f와 0.5f 사이를 토글
            m.sprite.color = color;
        }
    }
}

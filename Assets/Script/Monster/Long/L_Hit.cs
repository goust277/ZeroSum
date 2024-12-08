using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Hit : BaseState
{
    private Long l;
    private float blinkDuration = 0.5f; // 깜빡거리는 총 시간
    private float blinkInterval = 0.1f; // 깜빡거리는 간격
    private float elapsedBlinkTime = 0f; // 깜빡거림 지속 시간
    private float intervalTimer = 0f; // 간격 타이머
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
                intervalTimer = 0f; // 간격 타이머 초기화
            }

            if (elapsedBlinkTime >= blinkDuration)
            {
                // 깜빡거림 종료
                isBlinking = false;
                SetSpriteAlpha(1f);
                stateMachine.ChangeState(new L_Chase(stateMachine, l));
            }
        }
    }
    public override void Exit()
    {
        isBlinking = false;
        SetSpriteAlpha(1f); // 상태 종료 시 알파값 복원
        l.isHit = false;
        l.attackCooldown = 3f;
        l.canAttack = true;
    }

    // 스프라이트의 알파값을 설정하는 메서드
    private void SetSpriteAlpha(float alpha)
    {
        if (l.sprite != null)
        {
            Color color = l.sprite.color;
            color.a = alpha;
            l.sprite.color = color;
        }
    }

    // 스프라이트 알파값을 토글하는 메서드
    private void ToggleSpriteAlpha()
    {
        if (l.sprite != null)
        {
            Color color = l.sprite.color;
            color.a = (color.a == 1f) ? 0.5f : 1f; // 1f와 0.5f 사이를 토글
            l.sprite.color = color;
        }
    }
}

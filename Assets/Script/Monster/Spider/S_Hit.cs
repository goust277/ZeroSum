using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Hit : BaseState
{
    private Spider s;
    private float blinkDuration = 0.5f;
    private float blinkInterval = 0.1f;
    public float elapsedBlinkTime = 0f;
    private float intervalTimer = 0f;
    private bool isBlinking = false;

    public S_Hit(StateMachine stateMachine, Spider monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        s.isHit = true;
        s.rb.velocity = Vector2.zero;
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
                intervalTimer = 0f;
            }

            if (elapsedBlinkTime >= blinkDuration)
            {
                isBlinking = false;
                SetSpriteAlpha(1f);
                stateMachine.ChangeState(new S_Chase(stateMachine, s));
            }
        }
    }
    public override void Exit()
    {
        isBlinking = false;
        s.isHit = false;
        s.attackCooldown = 3f;
        s.canAttack = true;
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (s.sprite != null)
        {
            Color color = s.sprite.color;
            color.a = alpha;
            s.sprite.color = color;
        }
    }

    private void ToggleSpriteAlpha()
    {
        if (s.sprite != null)
        {
            Color color = s.sprite.color;
            color.a = (color.a == 1f) ? 0.5f : 1f;
            s.sprite.color = color;
        }
    }
}

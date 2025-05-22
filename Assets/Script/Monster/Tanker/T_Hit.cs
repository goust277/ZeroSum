using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Hit : BaseState
{
    Tanker tanker;

    private float blinkDuration = 0.5f;
    private float blinkInterval = 0.1f;
    public float elapsedBlinkTime = 0f;
    private float intervalTimer = 0f;
    private bool isBlinking = false;

    public T_Hit(StateMachine stateMachine, Tanker monster) : base(stateMachine)
    {
        this.tanker = monster;
    }

    public override void Enter()
    {
        tanker.PlayDamagedSound();
        tanker.isHit = true;
        tanker.rb.velocity = Vector2.zero;
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
                stateMachine.ChangeState(new T_Chase(stateMachine, tanker));
            }
        }
    }
    public override void Exit()
    {
        isBlinking = false;
        tanker.isHit = false;
        tanker.attackCooldown = 3f;
        tanker.canAttack = true;
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (tanker.sprite != null)
        {
            Color color = tanker.sprite.color;
            color.a = alpha;
            tanker.sprite.color = color;
        }
    }

    private void ToggleSpriteAlpha()
    {
        if (tanker.sprite != null)
        {
            Color color = tanker.sprite.color;
            color.a = (color.a == 1f) ? 0.5f : 1f;
            tanker.sprite.color = color;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Hit : BaseState
{
    private Melee m;
    private float blinkDuration = 0.5f;
    private float blinkInterval = 0.1f;
    public float elapsedBlinkTime = 0f;
    private float intervalTimer = 0f; 
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
                intervalTimer = 0f;
            }

            if (elapsedBlinkTime >= blinkDuration)
            {
                isBlinking = false;
                SetSpriteAlpha(1f);
                stateMachine.ChangeState(new M_Chase(stateMachine, m));
            }
        }
    }
    public override void Exit()
    {
        isBlinking = false;
        SetSpriteAlpha(1f);
        m.isHit = false;
        m.attackCooldown = 3f;
        m.canAttack = true;
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (m.sprite != null)
        {
            Color color = m.sprite.color;
            color.a = alpha;
            m.sprite.color = color;
        }
    }

    private void ToggleSpriteAlpha()
    {
        if (m.sprite != null)
        {
            Color color = m.sprite.color;
            color.a = (color.a == 1f) ? 0.5f : 1f;
            m.sprite.color = color;
        }
    }
}

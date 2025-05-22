using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_Hit : BaseState
{
    private Summoner summoner;
    private float blinkDuration = 0.5f;
    private float blinkInterval = 0.1f;
    public float elapsedBlinkTime = 0f;
    private float intervalTimer = 0f;
    private bool isBlinking = false;

    public Summoner_Hit(StateMachine stateMachine, Summoner monster) : base(stateMachine)
    {
        this.summoner = monster;
    }

    public override void Enter()
    {
        summoner.isHit = true;
        summoner.invincibility = true;
        summoner.rb.velocity = Vector2.zero;
        elapsedBlinkTime = 0f;
        intervalTimer = 0f;
        isBlinking = true;
        summoner.anim.SetTrigger("isHit");

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
                stateMachine.ChangeState(new Summoner_Chase(stateMachine, summoner));
            }
        }
    }
    public override void Exit()
    {
        isBlinking = false;
        summoner.invincibility = false;
        summoner.attackCooldown = 3f;
        summoner.canAttack = true;
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (summoner.sprite != null)
        {
            Color color = summoner.sprite.color;
            color.a = alpha;
            summoner.sprite.color = color;
        }
    }

    private void ToggleSpriteAlpha()
    {
        if (summoner.sprite != null)
        {
            Color color = summoner.sprite.color;
            color.a = (color.a == 1f) ? 0.5f : 1f;
            summoner.sprite.color = color;
        }
    }
}

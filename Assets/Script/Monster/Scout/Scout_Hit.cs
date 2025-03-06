using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout_Hit : BaseState
{
    private Scout scout;
    private float blinkDuration = 0.5f;
    private float blinkInterval = 0.1f; 
    public float elapsedBlinkTime = 0f; 
    private float intervalTimer = 0f; 
    private bool isBlinking = false;

    public Scout_Hit(StateMachine stateMachine, Scout monster) : base(stateMachine)
    {
        this.scout = monster;
    }

    public override void Enter()
    {
        scout.isHit = true;
        scout.rb.velocity = Vector2.zero;
        elapsedBlinkTime = 0f;
        intervalTimer = 0f;
        isBlinking = true;
    }

    public override void Execute()
    {
        if (isBlinking)
        {
            elapsedBlinkTime += Time.deltaTime;
            intervalTimer += Time.deltaTime;

            if (intervalTimer >= blinkInterval)
            {
                intervalTimer = 0f; 
            }

            if (elapsedBlinkTime >= blinkDuration)
            {
                isBlinking = false;
                stateMachine.ChangeState(new Scout_Chase(stateMachine, scout));
            }
        }
    }
    public override void Exit()
    {
        isBlinking = false;
        scout.isHit = false;
        scout.attackCooldown = 3f;
        scout.canAttack = true;
    }
}

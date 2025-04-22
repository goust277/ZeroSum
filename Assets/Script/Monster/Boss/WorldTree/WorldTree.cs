using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class WorldTree : MonoBehaviour, IDamageAble
{
    public Animator anim;
    public bool leftArmDestroyed, rightArmDestroyed, headExposed;
    public int health = 10;

    public float leftAttackCooldown = 5f, rightAttackCooldown = 5f, seedCooldown = 7f;
    private float leftTimer, rightTimer, seedTimer;
    private float leftRegenTimer = -1f, rightRegenTimer = -1f;
    private float regenDelay = 40f;

    public BaseState nextQueuedState = null;

    public HeadExposed headExposedState;
    public WorldTree_Idle idleState;
    public SeedDrop seedDropToLeft, seedDropToRight;
    public LeftArm leftAttackState;
    public RightArm rightAttackState;
    public Recovery recoveryState;

    private StateMachine stateMachine;

    void Start()
    {
        stateMachine = new StateMachine();

        idleState = new WorldTree_Idle(stateMachine, this);
        leftAttackState = new LeftArm(stateMachine, this);
        rightAttackState = new RightArm(stateMachine, this);
        seedDropToLeft = new SeedDrop(stateMachine, this, true);
        seedDropToRight = new SeedDrop(stateMachine, this, false);
        headExposedState = new HeadExposed(stateMachine, this);
        recoveryState = new Recovery(stateMachine, this);

        stateMachine.Initialize(idleState);
        leftTimer = leftAttackCooldown;
        rightTimer = rightAttackCooldown;
        seedTimer = seedCooldown;
    }

    void Update()
    {
        stateMachine.currentState.Execute();

        leftTimer -= Time.deltaTime;
        rightTimer -= Time.deltaTime;
        seedTimer -= Time.deltaTime;

        if (leftArmDestroyed) { leftRegenTimer -= Time.deltaTime; if (leftRegenTimer <= 0) leftArmDestroyed = false; }
        if (rightArmDestroyed) { rightRegenTimer -= Time.deltaTime; if (rightRegenTimer <= 0) rightArmDestroyed = false; }
    }

    public bool CanUseLeftArm() => !leftArmDestroyed && leftTimer <= 0f;
    public bool CanUseRightArm() => !rightArmDestroyed && rightTimer <= 0f;
    public bool CanDropSeed() => seedTimer <= 0f;

    public void ResetLeftTimer() => leftTimer = leftAttackCooldown;
    public void ResetRightTimer() => rightTimer = rightAttackCooldown;
    public void ResetSeedTimer() => seedTimer = seedCooldown;

    public void DestroyLeftArm() { leftArmDestroyed = true; leftRegenTimer = regenDelay; }
    public void DestroyRightArm() { rightArmDestroyed = true; rightRegenTimer = regenDelay; }

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Debug.Log("º¸½º »ç¸Á");
        }
    }

    public void ExposeHead()
    {
        headExposed = true;
        stateMachine.ChangeState(headExposedState);
    }

    public void Recover()
    {
        headExposed = false;
        leftArmDestroyed = false;
        rightArmDestroyed = false;
        stateMachine.ChangeState(recoveryState);
    }
}


using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class WorldTree : MonoBehaviour, IDamageAble
{
    [Header("보스 기본 설정")]
    public int health = 100;
    private float phaseValue => health / 100f;

    public Animator anim;

    [Header("타이머 및 FSM")]
    public float warningTimer;
    public float exposedDuration = 10f;
    public float finalBurstDuration = 15f;

    private float exposedTimer;
    private float finalBurstTimer;

    public bool leftArmDestroyed;
    public bool rightArmDestroyed;
    public bool headExposed;
    public bool isDying;

    [Header("팔 관련")]
    public float regenDelay = 40f;
    private float leftRegenTimer = -1f;
    private float rightRegenTimer = -1f;

    public R_Damage rightArmDamage;
    public L_Damage leftArmDamage;
    public Collider2D Head;
    public Collider2D Left_atk;
    public Collider2D Right_atk;
    public GameObject MiddleArm;
    public GameObject laser;
    private float BulletSpeed = 10f;
    public GameObject BulletPrefab;
    public Transform fPoint;

    [Header("상태 참조")]
    public BaseState pattern1;
    public BaseState pattern2;
    public PatternPause patternPause;

    private StateMachine stateMachine;

    public WorldTree_Idle idleState;
    public HeadExposed headExposedState;
    public Recovery recoveryState;
    public FinalBurst finalBurstState;
    public WorldTree_Die dieState;

    public LeftArm patternA;
    public RightArm patternB;
    public MiddleArm patternC;

    public List<BaseState> finalBurst = new();
    public int burstIndex = 0;

    void Start()
    {
        stateMachine = new StateMachine();

        // 상태 인스턴스 초기화
        idleState = new WorldTree_Idle(stateMachine, this);
        patternPause = new PatternPause(stateMachine, this);
        headExposedState = new HeadExposed(stateMachine, this);
        recoveryState = new Recovery(stateMachine, this);
        finalBurstState = new FinalBurst(stateMachine, this);
        dieState = new WorldTree_Die(stateMachine, this);

        patternA = new LeftArm(stateMachine, this);
        patternB = new RightArm(stateMachine, this);
        patternC = new MiddleArm(stateMachine, this);

        stateMachine.Initialize(idleState);
    }

    void Update()
    {
        stateMachine.currentState.Execute();

        if (!headExposed && !isDying)
        {
            if (leftArmDestroyed)
            {
                leftRegenTimer -= Time.deltaTime;
                if (leftRegenTimer <= 0f)
                {
                    leftArmDestroyed = false;
                    leftArmDamage.ResetArm();
                }
            }

            if (rightArmDestroyed)
            {
                rightRegenTimer -= Time.deltaTime;
                if (rightRegenTimer <= 0f)
                {
                    rightArmDestroyed = false;
                    rightArmDamage.ResetArm();
                }
            }

            // 머리 노출 조건
            if (leftArmDestroyed && rightArmDestroyed && !headExposed)
            {
                ExposeHead();
            }
        }

        if (isDying && !headExposed)
        {
            finalBurstTimer -= Time.deltaTime;
            if (finalBurstTimer <= 0f)
            {
                stateMachine.ChangeState(dieState);
            }
        }
    }

    public void Damage(int atk)
    {
        if (isDying) return;

        health -= atk;
        if (health <= 0)
        {
            if (headExposed)
            {
                isDying = true;
            }
            else
            {
                isDying = true;
                finalBurstTimer = finalBurstDuration;
                stateMachine.ChangeState(finalBurstState);
            }
        }
    }

    public void ExposeHead()
    {
        headExposed = true;
        exposedTimer = exposedDuration;
        stateMachine.ChangeState(headExposedState);
    }

    public void DestroyLeftArm()
    {
        if (!leftArmDestroyed)
        {
            leftArmDestroyed = true;
            var renderer = Left_atk.GetComponent<SpriteRenderer>();
            Color c = renderer.color;
            c.a = 0.4f;
            renderer.color = c;
            leftArmDamage.GetComponent<Collider2D>().enabled = false;
            leftRegenTimer = regenDelay;
        }
    }

    public void DestroyRightArm()
    {
        if (!rightArmDestroyed)
        {
            rightArmDestroyed = true;
            var renderer = Right_atk.GetComponent<SpriteRenderer>();
            Color c = renderer.color;
            c.a = 0.4f;
            renderer.color = c;
            rightArmDamage.GetComponent<Collider2D>().enabled = false;
            rightRegenTimer = regenDelay;
        }
    }

    public void Recover()
    {
        leftArmDestroyed = false;
        rightArmDestroyed = false;
        Left_atk.enabled = true;
        Right_atk.enabled = true;
        leftArmDamage.ResetArm();
        rightArmDamage.ResetArm();
        stateMachine.ChangeState(recoveryState);
    }

    public void GetRandomPattern()
    {
        bool boostProjectile = phaseValue <= 0.3f && phaseValue > 0.1f;

        List<BaseState> pattern = new();

        if (!leftArmDestroyed) pattern.Add(patternA);
        if (!rightArmDestroyed) pattern.Add(patternB);
        pattern.Add(patternC); // 중간 팔은 항상 가능

        // 투사체 강화 플래그 세팅
        patternC.boosted = boostProjectile;

        if (pattern.Count == 0)
        {
            pattern1 = idleState;
            pattern2 = null;
            return;
        }

        if (phaseValue > 0.7f)
        {
            // 100~70% 구간 → 단일 패턴 실행
            pattern1 = pattern[Random.Range(0, pattern.Count)];
            pattern2 = null;
        }
        else
        {
            // 69~10% 구간 → 두 개 랜덤 실행
            if (pattern.Count == 1)
            {
                pattern1 = pattern[0];
                pattern2 = null;
            }
            else
            {
                int first = Random.Range(0, pattern.Count);
                int second;
                do { second = Random.Range(0, pattern.Count); } while (second == first);

                pattern1 = pattern[first];
                pattern2 = pattern[second];
            }
        }
    }

    public void PrepareFinalBurst()
    {
        finalBurst.Clear();

        if (!leftArmDestroyed) finalBurst.Add(patternA);
        if (!rightArmDestroyed) finalBurst.Add(patternB);
        finalBurst.Add(patternC);

        for (int i = 0; i < finalBurst.Count; i++)
        {
            int j = Random.Range(i, finalBurst.Count);
            (finalBurst[i], finalBurst[j]) = (finalBurst[j], finalBurst[i]);
        }

        burstIndex = 0;
    }

    private BaseState GetPatternByIndex(int i, bool boost)
    {
        if (i == 0) return patternA;
        if (i == 1) return patternB;
        if (i == 2) { patternC.boosted = boost; return patternC; }
        return idleState;
    }

    private int[] GetTwoRandomIndexes(int count)
    {
        int first = Random.Range(0, count);
        int second;
        do { second = Random.Range(0, count); } while (second == first);
        return new int[] { first, second };
    }

    private void Fire()
    {
        if (BulletPrefab != null)
        {
            Vector2 dir = Vector2.left;

            GameObject bullet = GameObject.Instantiate(BulletPrefab, fPoint.position, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = dir * BulletSpeed; // 발사 속도 설정
            }
        }
    }
}
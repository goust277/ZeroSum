using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite1 : MonoBehaviour, IDetectable, IDamageAble
{
    [Header("Animation")]
    public Animator anim;
    public SpriteRenderer sprite;

    [Header("Patrol Settings")]
    public float patrolRange = 10f;
    public float moveSpeed = 2f;
    private Vector3 spawnPosition;

    public Vector3 spawnPoint => spawnPosition;

    [Header("Detection Settings")]
    public Transform player;
    public bool isPlayerInRange;

    [Header("Combat Settings")]
    public int health = 100;
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 3f;
    public float dashRange = 1.5f;
    public bool isDashing;
    public bool canAttack = true;
    public bool touchPlayer;
    private bool isCooldownComplete;
    public bool isHit;
    public Rigidbody2D rb;
    public GameObject L_attack;
    public GameObject R_attack;
    public GameObject S_attack;
    private StateMachine stateMachine;
    public bool isblock;
    public bool canSpecial = true;
    public bool test;

    void Start()
    {
        spawnPosition = transform.position;
        stateMachine = new StateMachine();

        // 필요한 상태 생성 시 컴포넌트를 전달
        var idleState = new E1_Idle(stateMachine, this);
        var readyStade = new E1_Ready(stateMachine, this);
        var attackState = new E1_Attack(stateMachine, this);
        var Special_attackState = new E1_Special_Attack(stateMachine, this);
        //var patrolState = new E1_Patrol(stateMachine, this);
        var chaseState = new E1_Chase(stateMachine, this);
        var hitState = new E1_Hit(stateMachine, this);
        var dieState = new E1_Die(stateMachine, this);

        // 상태 초기화
        stateMachine.Initialize(chaseState);
    }

    void Update()
    {
        stateMachine.currentState.Execute();

        if (!isCooldownComplete && canAttack)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                isCooldownComplete = true;
                canAttack = false;
            }
        }

        if(test)
        {
            stateMachine.ChangeState(new E1_Special_Attack(stateMachine, this));
        }
    }

    public bool CanEnterAttackState()
    {
        if (isCooldownComplete)
        {
            isCooldownComplete = false;
            return true;
        }
        return false;
    }

    public void SetPlayerInRange(bool inRange)
    {
        isPlayerInRange = inRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageAble damageable = other.GetComponent<IDamageAble>();
            if (damageable != null)
            {
                damageable.Damage(attackDamage);
            }
        }
    }

    public void Damage(int atk)
    {
        if (isHit || isblock)
        {
            return;
        }

        health -= atk;

        if (health <= 0)
        {
            stateMachine.ChangeState(new E1_Die(stateMachine, this));
        }
        else if (75 >= health && health > 50 && !isHit && canSpecial)
        {
            isblock = true;
            canSpecial = false;
            stateMachine.ChangeState(new E1_Special_Attack(stateMachine, this));
        }
        else if(50 >= health && health > 25 && !isHit && canSpecial)
        {
            isblock = true;
            canSpecial = false;
            stateMachine.ChangeState(new E1_Special_Attack(stateMachine, this));
        }
        else if (health > 0 && !isHit)
        {
            stateMachine.ChangeState(new E1_Hit(stateMachine, this));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Melee : MonoBehaviour, IDetectable, IDamageAble
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
    private StateMachine stateMachine;

    void Start()
    {
        spawnPosition = transform.position;
        stateMachine = new StateMachine();

        // 필요한 상태 생성 시 컴포넌트를 전달
        var idleState = new M_Idle(stateMachine, this);
        var readyStade = new M_Ready(stateMachine, this);
        var attackState = new M_Attack(stateMachine, this);
        var patrolState = new M_Patrol(stateMachine, this);
        var chaseState = new M_Chase(stateMachine, this);
        var hitState = new M_Hit(stateMachine, this);
        var dieState = new M_Die(stateMachine, this);

        // 상태 초기화
        stateMachine.Initialize(idleState);
    }

    void Update()
    {
        stateMachine.currentState.Execute();

        if(!isCooldownComplete && canAttack)
        {
            attackCooldown -= Time.deltaTime;
            if(attackCooldown <= 0)
            {
                isCooldownComplete = true;
                canAttack = false;
            }
        }
    }

    public bool CanEnterAttackState()
    {
        if(isCooldownComplete)
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
            damageable?.Damage(attackDamage);
        }
    }

    public void Damage(int atk)
    {
        if(isHit)
        {
            return;
        }

        health -= atk;

        if (health <= 0)
        {
            stateMachine.ChangeState(new M_Die(stateMachine, this));
        }
        else if (health > 0 && !isHit)
        {
            stateMachine.ChangeState(new M_Hit(stateMachine, this));
        }
    }
}
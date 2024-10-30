using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
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
    private bool isPlayerInRange;

    [Header("Combat Settings")]
    public float health = 100f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 3f;
    public float dashRange = 3f;
    public bool isDashing;
    public bool canAttack = true;
    public bool touchPlayer;
    private bool isCooldownComplete;

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

    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            touchPlayer = true;
        }
    }
}

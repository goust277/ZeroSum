using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;
using TMPro;

public class Mission_melee : BaseAudioMonster, IDetectable, IDamageAble
{
    [Header("Animation")]
    public Animator anim;
    public SpriteRenderer sprite;

    [Header("Patrol Settings")]
    public float patrolRange = 10f;
    public float moveSpeed = 2f;
    private Vector3 spawnPosition;
    public Vector3 currentTarget;
    public bool turn;

    public Vector3 spawnPoint => spawnPosition;

    [Header("Detection Settings")]
    public Transform player;
    public bool isPlayerInRange;
    public GameObject detect;
    public float wait_T;

    [Header("Combat Settings")]
    public int health = 2;
    public int attackDamage = 1;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public bool canAttack = true;
    public GameObject mark;
    public bool seeMark;
    private bool isCooldownComplete;
    public bool isHit;
    public bool isDie;
    public Rigidbody2D rb;
    public GameObject attack;
    private StateMachine stateMachine;

    void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        spawnPosition = transform.position;
        stateMachine = new StateMachine();

        // 필요한 상태 생성 시 컴포넌트를 전달
        var idleState = new MM_Idle(stateMachine, this);
        var attackState = new MM_Attack(stateMachine, this);
        var chaseState = new MM_Chase(stateMachine, this);
        var hitState = new MM_Hit(stateMachine, this);
        var dieState = new MM_Die(stateMachine, this);

        //상태 초기화
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

        if (wait_T >= 0)
        {
            wait_T -= Time.deltaTime;
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


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (this.CompareTag("Monster") && other.collider.CompareTag("Wall"))
        {
            turn = true;
        }

        if (other.collider.CompareTag("MovingBlock"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && this.CompareTag("MonsterAtk"))
        {
            IDamageAble damageable = other.GetComponent<IDamageAble>();
            damageable?.Damage(attackDamage);
        }

        if (this.CompareTag("Monster") && (other.CompareTag("MovingBlock") || other.CompareTag("Ev")))
        {
            turn = true;
        }
    }

    void TakeDamage()
    {
        stateMachine.ChangeState(new MM_Die(stateMachine, this));
    }

    public void Damage(int atk)
    {
        if (!isHit)
        {
            if (isDie)
            {
                return;
            }

             health--;

            if (health <= 0)
            {
                stateMachine.ChangeState(new MM_Die(stateMachine, this));
            }
            else
            {
                stateMachine.ChangeState(new MM_Hit(stateMachine, this));
            }
        }
    }

    // 목표 반대로 변경
    public void FlipTarget()
    {
        // 현재 이동 방향 확인
        float moveDirection = Mathf.Sign(currentTarget.x - transform.position.x);

        // 반대 방향으로 목표 지점 설정
        currentTarget = new Vector3(transform.position.x - (moveDirection * patrolRange), transform.position.y, transform.position.z);

        // 현재 위치를 강제로 재설정하여 즉시 반영
        transform.position += new Vector3(moveDirection * -0.1f, 0, 0);
    }

    private void attack_col()
    {
        attack.gameObject.SetActive(true);
    }
}

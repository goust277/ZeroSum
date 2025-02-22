using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

public class Long : MonoBehaviour, IDetectable, IDamageAble
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
    public float smoothTime = 0.3f; // �ε巯�� ������ ����
    public float minDistance = 1.5f;
    public float maxDistance = 3.0f;
    public float height = 2.5f;
    private Vector3 currentVelocity = Vector3.zero;
    public Vector3 currentTargetPosition;

    [Header("Combat Settings")]
    public int health = 100;
    public int attackDamage = 10;
    public float attackRange = 3.0f;
    public float attackCooldown = 3f;
    public bool canAttack = true;
    public bool canShot = false;
    private bool isCooldownComplete;
    public bool isHit;
    public Rigidbody2D rb;
    private StateMachine stateMachine;
    // ������ �� �߻�ü ���� ����
    public LineRenderer laser;              // ������ ����
    public Transform leftFirePoint;         // ���� �߻� ��ġ
    public Transform rightFirePoint;        // ������ �߻� ��ġ
    public GameObject BulletPrefab;     // �߻�ü ������
    public float BulletSpeed = 10f;     // �߻�ü �ӵ�
    public Vector3 laserStart;              // ������ ���� ��ġ
    public Vector3 laserEnd;                // ������ �� ��ġ

    void Start()
    {
        if (laser != null)
        {
            laser.enabled = false;
        }

        spawnPosition = transform.position;
        stateMachine = new StateMachine();

        // �ʿ��� ���� ���� �� ������Ʈ�� ����
        var idleState = new L_Idle(stateMachine, this);
        var readyStade = new L_Ready(stateMachine, this);
        var attackState = new L_Attack(stateMachine, this);
        var patrolState = new L_Patrol(stateMachine, this);
        var chaseState = new L_Chase(stateMachine, this);
        var hitState = new L_Hit(stateMachine, this);
        var dieState = new L_Die(stateMachine, this);

        // ���� �ʱ�ȭ
        stateMachine.Initialize(idleState);
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

    public void DistanceFromPlayer()
    {
        float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);
        sprite.flipX = player.position.x > transform.position.x;

        if (distanceToPlayer < minDistance)
        {
            // �÷��̾ �ʹ� ������ �ٰ����� �ݴ� �������� �Ÿ� ����
            Vector3 Dir = (transform.position - player.position).normalized * maxDistance;

            // ��ǥ ��ġ�� ���������� ����
            currentTargetPosition = Vector3.Lerp(currentTargetPosition, transform.position + Dir, Time.deltaTime * 5f);
        }

        else if (distanceToPlayer > maxDistance)
        {
            currentTargetPosition = Vector3.Lerp(currentTargetPosition, player.position, Time.deltaTime * 2f);
        }
        else
        {
            currentTargetPosition = transform.position;
        }

        currentTargetPosition.y = player.position.y + height;
        transform.position = Vector3.SmoothDamp(transform.position, currentTargetPosition, ref currentVelocity, smoothTime);
    }

    void CanShot()
    {
        canShot = true;
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
        if (isHit)
        {
            return;
        }

        health -= atk;

        if (health <= 0)
        {
            stateMachine.ChangeState(new L_Die(stateMachine, this));
        }
        else if (health > 0 && !isHit)
        {
            stateMachine.ChangeState(new L_Hit(stateMachine, this));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanker : BaseAudioMonster, IDetectable, IDamageAble
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
    public float minDistance = 1.5f;
    public float maxDistance = 3.0f;
    public GameObject detect;

    [Header("Combat Settings")]
    public int health = 2;
    public int attackDamage = 1;
    public float attackRange = 6f;
    public float attackCooldown = 1f;
    public bool canAttack = true;
    public bool canShot = false;
    public bool canMove = true;
    private bool isCooldownComplete;
    public bool isHit;
    public bool isDie;
    public bool seeMark;
    public GameObject mark;
    public Rigidbody2D rb;
    private StateMachine stateMachine;
    public Transform leftFirePoint;         // ���� �߻� ��ġ
    public Transform rightFirePoint;        // ������ �߻� ��ġ
    public GameObject BulletPrefab;     // �߻�ü ������
    public float BulletSpeed = 10f;     // �߻�ü �ӵ�
    public int fireCount = 0;       // �߻� Ƚ��
    public int maxFireCount = 10;    // �ִ� �߻� Ƚ��
    public float fireInterval = 0.1f;
    private Transform fPoint;
    //public GameObject hitPrefab;

    //[Header("HP�� UI")]
    //[SerializeField] private Image hpBar;
    //[SerializeField] private GameObject DamageValuePrefab;
    //[SerializeField] private Transform canvasTransform;

    void Start()
    {
        spawnPosition = transform.position;
        stateMachine = new StateMachine();

        // �ʿ��� ���� ���� �� ������Ʈ�� ����
        var idleState = new T_Idle(stateMachine, this);
        var readyStade = new T_Ready(stateMachine, this);
        var attackState = new T_Attack(stateMachine, this);
        var patrolState = new T_Patrol(stateMachine, this);
        var chaseState = new T_Chase(stateMachine, this);
        var hitState = new T_Hit(stateMachine, this);
        var dieState = new T_Die(stateMachine, this);

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && this.CompareTag("MonsterAtk"))
        {
            IDamageAble damageable = other.GetComponent<IDamageAble>();
            damageable?.Damage(attackDamage);
        }

        if (other.CompareTag("MovingBlock") || other.CompareTag("Ev"))
        {
            turn = true;
        }
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

    private void VisualDamage(int value)
    {
        //Debug.Log("VisualDamage");
        //Vector3 offsetFix = new Vector3(-1.0f, -1.0f, 0.0f);
        //Vector3 offset = gameObject.transform.position; // z���� -0.1�� ����
        //GameObject newText = Instantiate(DamageValuePrefab, offset + offsetFix, Quaternion.identity);
        ////Debug.Log($"�ڡڡڡڡ� New Damage Text instantiated at: {newText.transform.position}");


        //if (canvasTransform == null)
        //{
        //    Debug.LogError("Canvas Transform is not assigned!");
        //    return;
        //}

        //newText.transform.SetParent(canvasTransform, false);
        ////Debug.Log($"�� ��Parent set to: {newText.transform.parent.name}");
        ////TextMeshProUGUI textComponent = newText.GetComponentInChildren<TextMeshProUGUI>();
        //TextMeshProUGUI textComponent = newText.GetComponent<TextMeshProUGUI>();
        //if (textComponent == null)
        //{
        //    Debug.LogError("TextMeshProUGUI component not found in prefab!");
        //    return;
        //}

        //textComponent.text = value.ToString();
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
                stateMachine.ChangeState(new T_Die(stateMachine, this));
            }
            else
            {
                stateMachine.ChangeState(new T_Hit(stateMachine, this));
            }
        }

        if (atk == 100)
        {
            health = 0;
            isDie = true;
            stateMachine.ChangeState(new T_Die(stateMachine, this));
        }
        //HP �� ǥ��
        //if (hpBar != null)
        //{
        //    hpBar.fillAmount = Mathf.Clamp(health, 0, 100) / 100f; //0~1 ���̷� Ŭ����
        //}
        //VisualDamage(atk);
    }

    void TakeDamage()
    {
        stateMachine.ChangeState(new T_Die(stateMachine, this));
    }


    // ��ǥ �ݴ�� ����
    public void FlipTarget()
    {
        // ���� �̵� ���� Ȯ��
        float moveDirection = Mathf.Sign(currentTarget.x - transform.position.x);

        // �ݴ� �������� ��ǥ ���� ����
        currentTarget = new Vector3(transform.position.x - (moveDirection * patrolRange), transform.position.y, transform.position.z);

        // ���� ��ġ�� ������ �缳���Ͽ� ��� �ݿ�
        transform.position += new Vector3(moveDirection * -0.1f, 0, 0);
    }

    private void EnableFire()
    {
        canShot = true;
    }


    private void Move_controll()
    {
        if (canMove)
        {
            canMove = false;
        }
        else if(!canMove)
        {
            canMove = true;
        }
    }

    public void FireBullet()
    {
        if (BulletPrefab == null) return;

        fPoint = sprite.flipX ? rightFirePoint : leftFirePoint;
        Vector2 baseDir = sprite.flipX ? Vector2.right : Vector2.left;

        float spreadAngle = 20f;
        float randomAngle = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);

        Vector2 shootDir = Quaternion.Euler(0, 0, randomAngle * (sprite.flipX ? -1 : 1)) * baseDir;

        GameObject bullet = Instantiate(BulletPrefab, fPoint.position, Quaternion.identity);

        if (fPoint == rightFirePoint)
            bullet.GetComponent<SpriteRenderer>().flipX = true;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootDir.normalized * BulletSpeed;
        }
    }

    //public void SpawnHitEffect()
    //{
    //    Transform spawnPoint;
    //    bool flipX;
    //    float xOffset = 1f;

    //    if ((player.position.x - transform.position.x) >= 0.2f)
    //    {
    //        spawnPoint = leftFirePoint;
    //        flipX = false;
    //    }
    //    else
    //    {
    //        spawnPoint = rightFirePoint;
    //        flipX = true;
    //    }

    //    if (hitPrefab != null)
    //    {
    //        Vector3 spawnPosition = spawnPoint.position + new Vector3(flipX ? xOffset : -xOffset, 0, 0);

    //        GameObject hitInstance = Instantiate(hitPrefab, spawnPosition, Quaternion.identity);

    //        SpriteRenderer hitSprite = hitInstance.GetComponent<SpriteRenderer>();
    //        if (hitSprite != null)
    //        {
    //            hitSprite.flipX = flipX;
    //        }
    //    }
    //}
}

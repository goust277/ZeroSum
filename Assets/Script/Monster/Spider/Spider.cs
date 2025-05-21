using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;
using TMPro;

public class Spider : BaseAudioMonster, IDetectable, IDamageAble
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

    [Header("Combat Settings")]
    public int health = 100;
    public int attackDamage = 1;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public float dashRange = 3f;
    public bool canAttack = true;
    private bool isCooldownComplete;
    public bool isHit;
    public bool isDie;
    public Rigidbody2D rb;
    public GameObject attack;
    private StateMachine stateMachine;

    [Header("HP�� UI")]
    [SerializeField] private Image hpBar;
    [SerializeField] private GameObject DamageValuePrefab;
    [SerializeField] private Transform canvasTransform;

    void Start()
    {
        spawnPosition = transform.position;
        stateMachine = new StateMachine();

        // �ʿ��� ���� ���� �� ������Ʈ�� ����
        var idleState = new S_Idle(stateMachine, this);
        var readyStade = new S_Ready(stateMachine, this);
        var attackState = new S_Attack(stateMachine, this);
        var patrolState = new S_Patrol(stateMachine, this);
        var chaseState = new S_Chase(stateMachine, this);
        var hitState = new S_Hit(stateMachine, this);
        var dieState = new S_Die(stateMachine, this);

        //���� �ʱ�ȭ
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


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (this.CompareTag("Monster") && other.collider.CompareTag("Wall"))
        {
            turn = true;
        }
    }
    private void VisualDamage(int value)
    {
        Debug.Log("VisualDamage");
        Vector3 offsetFix = new Vector3(-1.0f, -1.0f, 0.0f);
        Vector3 offset = gameObject.transform.position; // z���� -0.1�� ����
        GameObject newText = Instantiate(DamageValuePrefab, offset + offsetFix, Quaternion.identity);
        //Debug.Log($"�ڡڡڡڡ� New Damage Text instantiated at: {newText.transform.position}");


        if (canvasTransform == null)
        {
            Debug.LogError("Canvas Transform is not assigned!");
            return;
        }

        newText.transform.SetParent(canvasTransform, false);
        //Debug.Log($"�� ��Parent set to: {newText.transform.parent.name}");
        //TextMeshProUGUI textComponent = newText.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI textComponent = newText.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in prefab!");
            return;
        }

        textComponent.text = value.ToString();
    }

    public void Damage(int atk)
    {
        if (!isHit)
        {
            if (isDie)
            {
                return;
            }

            if(atk != 10)
            health--;

            else
            {
                stateMachine.ChangeState(new S_Die(stateMachine, this));
                return;
            }

            if (health <= 0)
            {
                stateMachine.ChangeState(new S_Die(stateMachine, this));
            }
            else
            {
                stateMachine.ChangeState(new S_Hit(stateMachine, this));
            }
        }


        //HP �� ǥ��
        if (hpBar != null)
        {
            hpBar.fillAmount = Mathf.Clamp(health, 0, 100) / 100f; //0~1 ���̷� Ŭ����
        }
        //VisualDamage(atk);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;
using TMPro;

public class Summoner : MonoBehaviour , IDetectable, IDamageAble
{
    [Header("Animation")]
    public Animator anim;
    public SpriteRenderer sprite;

    [Header("Patrol Settings")]
    public float patrolRange = 3f;
    public float moveSpeed = 2f;
    private Vector3 spawnPosition;
    public Vector3 currentTarget;

    public Vector3 spawnPoint => spawnPosition;

    [Header("Detection Settings")]
    public Transform player;
    public bool isPlayerInRange;
    public GameObject detect;

    [Header("Combat Settings")]
    public int health = 100;
    public int attackDamage = 10;
    public float attackRange = 5f;
    public float attackCooldown = 0f;
    public bool canAttack = true;
    private bool isCooldownComplete;
    public bool isHit;
    public bool isDie;
    public Rigidbody2D rb;
    public GameObject spider;
    public float spawnRangeX = 1.5f;    // X 좌표 랜덤 범위
    public int summonCount = 3;       // 한 번에 소환할 몬스터 개수
    private StateMachine stateMachine;

    [Header("HP바 UI")]
    [SerializeField] private Image hpBar;
    [SerializeField] private GameObject DamageValuePrefab;
    [SerializeField] private Transform canvasTransform;

    void Start()
    {
        spawnPosition = transform.position;
        stateMachine = new StateMachine();

        // 필요한 상태 생성 시 컴포넌트를 전달
        var idleState = new Summoner_Idle(stateMachine, this);
        var readyStade = new Summoner_Ready(stateMachine, this);
        var attackState = new Summoner_Attack(stateMachine, this);
        var patrolState = new Summoner_Patrol(stateMachine, this);
        var chaseState = new Summoner_Chase(stateMachine, this);
        var hitState = new Summoner_Hit(stateMachine, this);
        var dieState = new Summoner_Die(stateMachine, this);

        //상태 초기화
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
    }

    private void VisualDamage(int value)
    {
        Debug.Log("VisualDamage");
        Vector3 offsetFix = new Vector3(-1.0f, -1.0f, 0.0f);
        Vector3 offset = gameObject.transform.position; // z축을 -0.1로 설정
        GameObject newText = Instantiate(DamageValuePrefab, offset + offsetFix, Quaternion.identity);
        //Debug.Log($"★★★★★ New Damage Text instantiated at: {newText.transform.position}");


        if (canvasTransform == null)
        {
            Debug.LogError("Canvas Transform is not assigned!");
            return;
        }

        newText.transform.SetParent(canvasTransform, false);
        //Debug.Log($"★ ★Parent set to: {newText.transform.parent.name}");
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

            health--;

            if (health <= 0)
            {
                stateMachine.ChangeState(new Summoner_Die(stateMachine, this));
            }
            else
            {
                stateMachine.ChangeState(new Summoner_Hit(stateMachine, this));
            }
        }


        //HP 바 표기
        if (hpBar != null)
        {
            hpBar.fillAmount = Mathf.Clamp(health, 0, 100) / 100f; //0~1 사이로 클램프
        }
        VisualDamage(atk);
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

    public void SummonMonsters()
    {
        if (spider == null)
        {
            Debug.LogError("소환할 몬스터 프리팹이 설정되지 않았습니다!");
            return;
        }

        for (int i = 0; i < summonCount; i++)
        {
            float step = (spawnRangeX * 2) / (summonCount - 1); // 총 거리 나누기 등분수
            float xOffset = -spawnRangeX + (step * i); // 왼쪽 끝에서 시작해서 step씩 증가

            Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
            Instantiate(spider, spawnPosition, Quaternion.identity);
        }
    }
}

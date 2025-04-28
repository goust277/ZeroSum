using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator animator;

    [Header("데미지")]
    [SerializeField] private int damage;
    [SerializeField] private float time;
    private float curTime;

    [Header("움직이는 속도")]
    [SerializeField] private float moveSpeed;

    [Header("폭발 범위")]
    [SerializeField] private GameObject col; 

    [HideInInspector] public bool isMove;

    [SerializeField] private Vector3 knockbackDirection;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        isMove = false;
    }

    private void Update()
    {
        if (isMove)
        {
            rb.velocity = knockbackDirection * moveSpeed;
        }
        else
            rb.velocity = Vector3.zero;
    }

    public void IsBomb()
    {
        col.SetActive(true);
    }

    public void IsEnable()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            IDamageAble damageable = collision.GetComponent<IDamageAble>();
            if (damageable != null)
            {
                if (curTime < time)
                {
                    curTime += Time.deltaTime;
                }
                else if (curTime >= time) 
                {
                    damageable.Damage(damage);
                    curTime = 0;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            knockbackDirection.x *= -1;
        }
    }

    public void TakeDamage(Vector3 attackerPosition)
    {
        knockbackDirection = (transform.position - attackerPosition).normalized;

        knockbackDirection.y = 0;

        animator.SetTrigger("Hit");
    }

    public void IsMove()
    {
        isMove = true;
    }

    public void IsStop()
    {
        isMove = false;
    }
}

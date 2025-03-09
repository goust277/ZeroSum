using Com.LuisPedroFonseca.ProCamera2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwordAttack : PlayerAttackState
{
    [SerializeField] private float delay;
    [SerializeField] private float atkCoolTime;
    
    [HideInInspector] public bool isParryingReady;

    private bool isAtkReady;

    public event Action OnSwordAttack;

    private void Start()
    {
        isAtkReady = true;
        delay = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAtkReady)
        {
            delay += Time.deltaTime;
            if (atkCoolTime <= delay)
            {
                isAtkReady = true;
            }
        }
        else
        {
            if (delay != 0f)
            {
                delay = 0f;
            }
        }
    }
    public void OnAttack()
    {
        if (isAtkReady)
        {
            OnSwordAttack?.Invoke();
        }

    }

    public void Parrying()
    {
        delay = attackDelay;
        isAttack = false;
        isAtkReady = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive") || collision.CompareTag("Monster"))
        {
            IDamageAble damageable = collision.GetComponent<IDamageAble>();
            if (damageable != null)
            {
                damageable.Damage(damage);
            }

            Bomb bomb = collision.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.TakeDamage(transform.position);
            }
        }
    }
}

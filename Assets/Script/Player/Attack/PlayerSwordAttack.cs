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


    private void Start()
    {
        isAtkReady = true;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    public void OnAttack(InputAction.CallbackContext context)
    {

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

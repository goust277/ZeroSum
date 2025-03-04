using Com.LuisPedroFonseca.ProCamera2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttack : PlayerAttackState
{
    [SerializeField] private float delay;
    [SerializeField] private float atkCoolTime;
    [SerializeField] private GameObject[] col;
    [HideInInspector] public bool isParryingReady;

    private bool isAtkReady;
    [SerializeField] private int combo = 1;
    private Animator animator;


    private void Start()
    {
        isAtkReady = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (delay >= 0)
        {
            delay -= Time.deltaTime;
            isAttack = true;
        }
        else
        {
            isAttack = false;
            combo = 1;
        }
        if (delay <= atkCoolTime)
        {
            isAtkReady = true;
        }

    }

    public void ComboAttack()
    {

        if (isAtkReady)
        {
            switch (combo)
            {
                case 1:
                    {
                        Attack1();
                        break;
                    }
            }
        }

    }

    private void Attack1()
    {
        delay = attackDelay;
        combo++;
        isAttack = false;
        isAtkReady = false;
    }

    public void Parrying()
    {
        delay = attackDelay;
        isAttack = false;
        isAtkReady = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            IDamageAble damageable = collision.GetComponent<IDamageAble>();
            if (damageable != null)
            {
                damageable.Damage(damage);


                var shakePreset = ProCamera2DShake.Instance.ShakePresets[0];

                ProCamera2DShake.Instance.Shake(shakePreset);
            }

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttack : PlayerAttackState
{
    [SerializeField] private float delay;
    [SerializeField] private float atkCoolTime;
    [SerializeField] private GameObject[] col;

    private bool isAtkReady;
    private int combo = 1;
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
                case 2: 
                    {
                        Attack2();
                        break;
                    }
                case 3:
                    {
                        Attack3();
                        break;
                    }

            }
        }

    }

    private void Attack1()
    {
        delay = attackDelay;
        animator.SetTrigger("Attack1");
        combo++;
        isAttack = false;
        isAtkReady = false;
    }

    private void Attack2()
    {
        delay = attackDelay;
        animator.SetTrigger("Attack2");
        combo++;
        isAttack = false;
        isAtkReady = false;
    }

    private void Attack3()
    {
        delay = attackDelay;
        animator.SetTrigger("Attack3");
        combo = 1;
        isAttack = false;
        isAtkReady = false;
    }

    private void AtkColSetTrue()
    {
        switch (combo)
        {
            case 1:
                {
                    col[0].SetActive(true);
                    break;
                }
            case 2:
                {
                    break;
                }
            case 3:
                {
                    break;
                }

        }
    }
    private void AtkColSetFalse()
    {
        switch (combo)
        {
            case 1:
                {
                    col[0].SetActive(false);
                    break;
                }
            case 2:
                {
                    break;
                }
            case 3:
                {
                    break;
                }

        }
    }
}

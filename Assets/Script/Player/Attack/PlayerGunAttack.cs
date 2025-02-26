using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAttack : PlayerAttackState
{
    [SerializeField] private float delay;

    [SerializeField] private bool isAtkReady;

    [Header("ÃÑ¾Ë ÇÁ¸®ÆÕ")]
    [SerializeField] private GameObject bullet;

    private float delayTime;
    private Animator animator;
    private PlayerMovement playerMovement;
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        isAtkReady = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAtkReady)
        {
            if (delayTime >= 0)
            {
                delayTime -= Time.deltaTime;
            }
            else
            {
                isAtkReady = true;
            }
        }

    }

    public void GunAttack()
    {
        if(isAtkReady)
        {
            if (!playerMovement.isDown)
            {
                animator.SetTrigger("GunAttackStart");
                Attack();
            }
        }
        else
        {
            animator.SetTrigger("GunAttack");
            Attack();
            Debug.Log("Attack");
        }
        if (playerMovement.isDown)
        {
            Debug.Log("AttackDown");
            animator.SetTrigger("GunAttack");
            Attack();
        }
    }

    private void Attack()
    {
        delayTime = delay;
        isAtkReady = false;


        //GameObject fireBullet = Instantiate(bullet, transform.position, transform.rotation);

        
    }
}

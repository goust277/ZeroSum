using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAttack : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float atkCoolTime;

    [SerializeField] private bool isAtkReady;
    private float delayTime;
    private Animator animator;
    private PlayerMovement playerMovement;

    //UI
    private Ver01_DungeonStatManager dungeonStatManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        //UI
        dungeonStatManager = GetComponent<Ver01_DungeonStatManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isAtkReady)
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
        if(!isAtkReady)
        {
            if (playerMovement.isDown)
            {
                Debug.Log("AttackDown");
                animator.SetTrigger("GunAttack");
                Attack();
            }
                
            else
            {
                animator.SetTrigger("GunAttackStart");
                Attack();
                isAtkReady = true;
            }
        }
        else
        {
            animator.SetTrigger("GunAttack");
            Attack();
            Debug.Log("Attack");
        }
    }

    private void Attack()
    {
        delayTime = delay;

        //UI & 불렛생성
        dungeonStatManager.ShotGun();
    }
}

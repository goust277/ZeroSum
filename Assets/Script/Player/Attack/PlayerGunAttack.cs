using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAttack : PlayerAttackState
{
    [SerializeField] private float delay;
    [SerializeField] private float atkCoolTime;

    [HideInInspector] public bool isAtkEnd;
    private float delayTime;
    private PlayerMovement playerMovement;

    public event Action OnGunAttack;
    public event Action OnFirstGunAttack;

    [Header("ÃÑ¾Ë ÇÁ¸®ÆÕ")]
    [SerializeField] private GameObject bullet;
    //UI
    //private Ver01_DungeonStatManager dungeonStatManager;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        bullet.GetComponent<PlayerBullet>().damage = this.damage;
        isAtkEnd = false;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void GunAttack()
    {
        if (!isAtkEnd)
        {
            if (!playerMovement.isDown)
            {
                OnFirstGunAttack?.Invoke();
                Attack();
            }
        }
        else
        {
            OnGunAttack?.Invoke();
            Attack();
            Debug.Log("Attack");
        }
        if (playerMovement.isDown)
        {
            Debug.Log("AttackDown");
            OnGunAttack?.Invoke();
            Attack();
        }
    }

    private void Attack()
    {
        delayTime = delay;
        isAtkEnd = true;
    }


}

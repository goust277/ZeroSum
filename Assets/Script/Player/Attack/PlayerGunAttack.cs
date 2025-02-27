using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAttack : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float atkCoolTime;

    private bool isAtkReady;
    private float delayTime;
    private PlayerMovement playerMovement;

    public event Action OnGunAttack;
    public event Action OnFirstGunAttack;
    //UI
    private Ver01_DungeonStatManager dungeonStatManager;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        isAtkReady = true;

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
        if (isAtkReady)
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
        isAtkReady = false;

        //UI & 불렛생성
        //dungeonStatManager.ShotGun();
    }
}

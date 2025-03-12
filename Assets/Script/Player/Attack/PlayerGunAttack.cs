using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunAttack : PlayerAttackState
{
    [SerializeField] private int poolSize = 20;

    private List<GameObject> bulletPool;

    [SerializeField] private float delay;
    [SerializeField] private float atkCoolTime;
    [SerializeField] private float bulletSpeed;
    private float delayTime;
    private PlayerMovement playerMovement;

    public event Action OnGunAttack;
    public event Action OnFirstGunAttack;

    [Header("총알 프리팹")]
    [SerializeField] private GameObject bullet;

    [Header("총알 발사 위치")]
    [SerializeField] private Transform standAtk;
    [SerializeField] private Transform downAtk;

    [Header("총알 부모")]
    [SerializeField] private Transform bulletParent;

    private GameObject childBullet;

    private bool isAtkReady;
    //UI
    //private Ver01_DungeonStatManager dungeonStatManager;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        bullet.GetComponent<PlayerBullet>().damage = this.damage;
        bullet.GetComponent<PlayerBullet>().speed = this.bulletSpeed;
        isAttack = false;
        InitializeBulletPool();
        isAtkReady = true;
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

    public void GunAttack()
    {
        if (!isAttack)
        {
            if (!playerMovement.isDown)
            {
                OnFirstGunAttack?.Invoke();
                Attack();
                Debug.Log("FirstAttack");
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
        isAttack = true;

        GameObject bullets;
        Vector3 spawnPosition = playerMovement.isDown ? downAtk.position : standAtk.position;

        bullets = GetBulletFromPool();
        bullets.transform.position = spawnPosition;
        bullets.transform.rotation = Quaternion.identity;

        PlayerBullet playerBullet = bullets.GetComponent<PlayerBullet>();
        if (playerBullet != null)
        {
            playerBullet.SetDriection(transform.right);
        }
    }

    private void InitializeBulletPool()
    {
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newBullet = Instantiate(bullet, bulletParent);
            newBullet.SetActive(false);
            bulletPool.Add(newBullet);
        }
    }

    private GameObject GetBulletFromPool()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // 모든 총알이 사용 중이면 새로 생성
        GameObject newBullet = Instantiate(bullet, bulletParent);
        bulletPool.Add(newBullet);
        return newBullet;
    }
}

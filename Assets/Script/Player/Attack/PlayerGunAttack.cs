using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAttack : PlayerAttackState
{
    [SerializeField] private int poolSize = 20;

    private List<GameObject> bulletPool;

    [SerializeField] private float delay;
    [SerializeField] private float atkCoolTime;
    [SerializeField] private float bulletSpeed;

    [HideInInspector] public bool isAtkEnd;
    private float delayTime;
    private PlayerMovement playerMovement;

    public event Action OnGunAttack;
    public event Action OnFirstGunAttack;

    [Header("�Ѿ� ������")]
    [SerializeField] private GameObject bullet;

    [Header("�Ѿ� �߻� ��ġ")]
    [SerializeField] private Transform standAtk;
    [SerializeField] private Transform downAtk;

    [Header("�Ѿ� �θ�")]
    [SerializeField] private Transform bulletParent;

    private GameObject childBullet;
    //UI
    //private Ver01_DungeonStatManager dungeonStatManager;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        bullet.GetComponent<PlayerBullet>().damage = this.damage;
        bullet.GetComponent<PlayerBullet>().speed = this.bulletSpeed;
        isAtkEnd = false;
        InitializeBulletPool();
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
        Debug.Log("Attack");
        delayTime = delay;
        isAtkEnd = true;

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

        // ��� �Ѿ��� ��� ���̸� ���� ����
        GameObject newBullet = Instantiate(bullet, bulletParent);
        bulletPool.Add(newBullet);
        return newBullet;
    }
}

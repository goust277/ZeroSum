using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [HideInInspector] public static BulletPool Instance;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private List<GameObject> bulletPool;

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet()
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
        GameObject newBullet = Instantiate(bulletPrefab, transform);
        bulletPool.Add(newBullet);
        return newBullet;
    }
}

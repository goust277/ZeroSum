using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterDoors : MonoBehaviour
{
    [Header("스폰시간")]
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    private float spawnTime;
    private float time;
    private bool isSpawnReady;

    [Header("스폰 몬스터")]
    [SerializeField] private GameObject monster;
    private GameObject target;


    void Start()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        time = 0;
        isSpawnReady = true;
        target = GameObject.Find("Monsters");
    }

    
    void Update()
    {
        if (time >= spawnTime) 
        {
            if (isSpawnReady)
                spawnMonster();
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    private void spawnMonster()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        isSpawnReady = false;

        GameObject _monster = Instantiate(monster, transform.position, Quaternion.identity, target.transform);
    }
}

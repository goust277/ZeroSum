using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;

public class Monster_Spawner : MonoBehaviour
{
    [Header("몬스터 프리팹 리스트")]
    public List<GameObject> monsterPrefabs;

    [Header("소환 간격 (초)")]
    public float spawnInterval = 5f;

    [Header("최대 몬스터 수")]
    public int maxMonsterCount = 5; // 이 스포너가 유지할 최대 몬스터 수

    private List<GameObject> spawnedMonsters = new List<GameObject>(); // 이 스포너가 소환한 몬스터 목록

    private static List<Monster_Spawner> allSpawners = new List<Monster_Spawner>();
    private static float lastSpawnTime = 0f;

    private void Start()
    {
        allSpawners.Add(this);
        StartCoroutine(SpawnMonsterRoutine());
    }

    private IEnumerator SpawnMonsterRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 모든 스포너가 동시에 소환하지 않도록 제어
            if (Time.time - lastSpawnTime < spawnInterval * 0.8f)
                continue;

            // 자신이 소환한 몬스터 개수만 확인
            CleanUpMonsterList();
            if (spawnedMonsters.Count < maxMonsterCount)
            {
                SpawnMonster();
                lastSpawnTime = Time.time; // 마지막 소환 시간 갱신
            }
        }
    }

    private void SpawnMonster()
    {
        int index = Random.Range(0, monsterPrefabs.Count);
        GameObject monster = Instantiate(monsterPrefabs[index], transform.position, Quaternion.identity);
        monster.name = monsterPrefabs[index].name;

        Melee meleeComponent = monster.GetComponent<Melee>();
        if (meleeComponent != null)
        {
            meleeComponent.player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 할당
        }

        spawnedMonsters.Add(monster);
    }

    // 몬스터가 제거되었을 경우 리스트에서 삭제
    public void RemoveMonsterFromList(GameObject monster)
    {
        spawnedMonsters.Remove(monster);
    }

    // 몬스터 리스트에서 이미 제거된 오브젝트 정리
    private void CleanUpMonsterList()
    {
        spawnedMonsters.RemoveAll(monster => monster == null);
    }

    private void OnDestroy()
    {
        allSpawners.Remove(this);
    }
}

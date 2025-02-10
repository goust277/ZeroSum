using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;

public class Monster_Spawner : MonoBehaviour
{
    [Header("���� ������ ����Ʈ")]
    public List<GameObject> monsterPrefabs;

    [Header("��ȯ ���� (��)")]
    public float spawnInterval = 5f;

    [Header("�ִ� ���� ��")]
    public int maxMonsterCount = 5; // �� �����ʰ� ������ �ִ� ���� ��

    private List<GameObject> spawnedMonsters = new List<GameObject>(); // �� �����ʰ� ��ȯ�� ���� ���

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

            // ��� �����ʰ� ���ÿ� ��ȯ���� �ʵ��� ����
            if (Time.time - lastSpawnTime < spawnInterval * 0.8f)
                continue;

            // �ڽ��� ��ȯ�� ���� ������ Ȯ��
            CleanUpMonsterList();
            if (spawnedMonsters.Count < maxMonsterCount)
            {
                SpawnMonster();
                lastSpawnTime = Time.time; // ������ ��ȯ �ð� ����
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
            meleeComponent.player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� �Ҵ�
        }

        spawnedMonsters.Add(monster);
    }

    // ���Ͱ� ���ŵǾ��� ��� ����Ʈ���� ����
    public void RemoveMonsterFromList(GameObject monster)
    {
        spawnedMonsters.Remove(monster);
    }

    // ���� ����Ʈ���� �̹� ���ŵ� ������Ʈ ����
    private void CleanUpMonsterList()
    {
        spawnedMonsters.RemoveAll(monster => monster == null);
    }

    private void OnDestroy()
    {
        allSpawners.Remove(this);
    }
}

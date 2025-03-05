using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDoor : MonoBehaviour
{
    [Header("몬스터 프리팹 리스트")]
    [SerializeField] private List<GameObject> monsterPrefabs;

    private List<GameObject> spawnedMonsters = new List<GameObject>();

    private static List<Monster_Spawner> allSpawners = new List<Monster_Spawner>();
    private static float lastSpawnTime = 0f;

    private bool isSpawnReady;
    private Animator animator;

    private GameObject doorController;
    private Transform parent;
    private void Start()
    {
        isSpawnReady = true;
        animator = GetComponent<Animator>();
        doorController = GameObject.Find("DoorManager");

        doorController.GetComponent<DoorController>().AddToList(gameObject);
        parent = GameObject.Find("MonsterManager").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetOpen()
    {
        animator.SetTrigger("Open");
    }
    public void SpawnMonster()
    {
        if (isSpawnReady)
        {
            int index = Random.Range(0, monsterPrefabs.Count);
            GameObject monster = Instantiate(monsterPrefabs[index], transform.position, Quaternion.identity, parent);
            monster.name = monsterPrefabs[index].name;

            Melee meleeComponent = monster.GetComponent<Melee>();
            if (meleeComponent != null)
            {
                meleeComponent.player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 할당
            }

            spawnedMonsters.Add(monster);
        }
    }
}

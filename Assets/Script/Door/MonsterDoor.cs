using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDoor : MonoBehaviour
{
    [Header("몬스터 프리팹 리스트")]
    [SerializeField] private List<GameObject> monsterPrefabs;

    [SerializeField] private Transform player;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    private static List<Monster_Spawner> allSpawners = new List<Monster_Spawner>();
    private static float lastSpawnTime = 0f;

    private bool isSpawnReady;
    private Animator animator;

    private GameObject doorController;
    private Transform parent;
    private void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

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
    private void SpawnMonster()
    {
        if (monsterPrefabs == null || monsterPrefabs.Count == 0) return; // 리스트가 비어 있으면 리턴

        int index = Random.Range(0, monsterPrefabs.Count);
        GameObject monster = Instantiate(monsterPrefabs[index], transform.position, Quaternion.identity);
        monster.name = monsterPrefabs[index].name;

        AssignPlayerToMonster(monster);

        spawnedMonsters.Add(monster);
    }

    private void AssignPlayerToMonster(GameObject monster)
    {
        if (player == null) return; // 플레이어가 없으면 할당 불가능하므로 리턴

        Melee meleeComponent = monster.GetComponent<Melee>();
        if (meleeComponent != null)
            meleeComponent.player = player;

        Scout scoutComponent = monster.GetComponent<Scout>();
        if (scoutComponent != null)
            scoutComponent.player = player;

        Spider spiderComponent = monster.GetComponent<Spider>();
        if (spiderComponent != null)
            spiderComponent.player = player;

        Summoner summonerComponent = monster.GetComponent<Summoner>();
        if (summonerComponent != null)
            summonerComponent.player = player;
    }
}

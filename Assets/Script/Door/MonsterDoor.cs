using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDoor : MonoBehaviour
{
    [Header("몬스터 프리팹 리스트")]
    [SerializeField] private List<GameObject> monsterPrefabsTypeA;
    [SerializeField] private List<GameObject> monsterPrefabsTypeB;
    [SerializeField] private List<GameObject> monsterPrefabsTypeC;
    [SerializeField] private Transform player;

    [Header("SpawnCoolTime")]
    [SerializeField] private float spawnCoolTime;
    [SerializeField] private float curCoolTime;

    [SerializeField] private float spawnTime;
    private float curSpawnTime = 0f;

    [SerializeField] private List<List<GameObject>> monsterPrefabs = new List<List<GameObject>>();
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    private static List<Monster_Spawner> allSpawners = new List<Monster_Spawner>();
    private static float lastSpawnTime = 0f;

    private bool isSpawnReady;
    private Animator animator;

    private Transform parent;
    private void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        monsterPrefabs.Add(monsterPrefabsTypeA);
        monsterPrefabs.Add(monsterPrefabsTypeB);

        if (monsterPrefabsTypeC.Count != 0)
        {
            monsterPrefabs.Add(monsterPrefabsTypeC);
        }

        curCoolTime = spawnCoolTime;

        isSpawnReady = false;
        animator = GetComponent<Animator>();
        parent = GameObject.Find("MonsterManager").transform;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawnReady)
        {
            if (spawnedMonsters.Count == 0 && curCoolTime >= spawnCoolTime)
            {
                SetOpen();
                curSpawnTime = 0;
            }
        }

        if (curCoolTime < spawnCoolTime && spawnedMonsters.Count == 0)
        {
            curCoolTime += Time.deltaTime;
        }

        if (spawnedMonsters.Count != 0)
        {
            spawnedMonsters.RemoveAll(obj => obj != null && !obj.activeSelf);
        }
        //spawnedMonsters.RemoveAll(item => item == null);

        if (curSpawnTime > 0)
        {
            curSpawnTime -= Time.deltaTime;
        }
    }

    public void SetOpen()
    {
        animator.SetBool("Open", true);
    }
    private void SpawnMonster()
    {
        if (monsterPrefabs == null || monsterPrefabs.Count == 0) return; // 리스트가 비어 있으면 리턴

        if (animator.GetBool("Open"))
            animator.SetBool("Open", false);
        int spawnType = Random.Range(0, monsterPrefabs.Count);
        List<GameObject> selectedTypeList = monsterPrefabs[spawnType];

        for(int index = 0; index < selectedTypeList.Count; index++) 
        {
            GameObject monster = Instantiate(selectedTypeList[index], transform.position, Quaternion.identity);
            monster.name = selectedTypeList[index].name;

            AssignPlayerToMonster(monster);

            spawnedMonsters.Add(monster);

        }

    }

    private void AssignPlayerToMonster(GameObject monster)
    {
        if (player == null) return; // 플레이어가 없으면 할당 불가능하므로 리턴

        Melee meleeComponent = monster.GetComponent<Melee>();
        if (meleeComponent != null)
            meleeComponent.player = player;

        Melee1 melee1Component = monster.GetComponent<Melee1>();
        if (melee1Component != null)
            melee1Component.player = player;

        Scout scoutComponent = monster.GetComponent<Scout>();
        if (scoutComponent != null)
            scoutComponent.player = player;

        Spider spiderComponent = monster.GetComponent<Spider>();
        if (spiderComponent != null)
            spiderComponent.player = player;

        Summoner summonerComponent = monster.GetComponent<Summoner>();
        if (summonerComponent != null)
            summonerComponent.player = player;

        Tanker tankerComponent = monster.GetComponent<Tanker>();
        if(tankerComponent != null)
            tankerComponent.player = player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isSpawnReady = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isSpawnReady = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("몬스터 문 리스트")]
    public List<GameObject> monsterDoors;

    [Header("오픈할 문 갯수")]
    [SerializeField] private int openDoorCount;

    [Header("스폰 주기")]
    [SerializeField] private float spawnTime;
    private float curSpawnTime;

    private bool isFirstSpawn;
    [SerializeField] private int curOpenDoor;
    void Start()
    {
        isFirstSpawn = true;
        curSpawnTime = spawnTime;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (curSpawnTime >= spawnTime)
        {
            if (isFirstSpawn)
            {
                curOpenDoor = Mathf.RoundToInt(monsterDoors.Count / 2);
                isFirstSpawn = false;
            }
            else
            {
                if (curOpenDoor != openDoorCount)
                {
                    curOpenDoor = openDoorCount;
                }
            }


            List<GameObject> doors = GetRandomElements(monsterDoors, curOpenDoor);
            OpenDoor(doors);
            curSpawnTime = 0f;
        }
        else
            curSpawnTime += Time.deltaTime;
    }

    List<T> GetRandomElements<T>(List<T> sourceList, int count)
    {
        // 원본 리스트를 복사하여 작업
        List<T> tempList = new List<T>(sourceList);
        List<T> result = new List<T>();

        for (int i = 0; i < count; i++)
        {
            if (tempList.Count == 0) break;

            // 랜덤 인덱스 생성
            int randomIndex = Random.Range(0, tempList.Count);

            // 랜덤 요소 추가 및 제거
            result.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }

        return result;
    }

    private void OpenDoor(List<GameObject> doors)
    {
        for(int i = 0; i < doors.Count; i++)
        {
            doors[i].GetComponent<MonsterDoor>().SetOpen();
        }
    }

    public void AddToList(GameObject obj)
    {
        if (!monsterDoors.Contains(obj))
        {
            monsterDoors.Add(obj);
        }
        else
        {

        }
    }
}

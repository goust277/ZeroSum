using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("���� �� ����Ʈ")]
    public List<GameObject> monsterDoors;

    [Header("������ �� ����")]
    [SerializeField] private int openDoorCount;

    [Header("���� �ֱ�")]
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
        // ���� ����Ʈ�� �����Ͽ� �۾�
        List<T> tempList = new List<T>(sourceList);
        List<T> result = new List<T>();

        for (int i = 0; i < count; i++)
        {
            if (tempList.Count == 0) break;

            // ���� �ε��� ����
            int randomIndex = Random.Range(0, tempList.Count);

            // ���� ��� �߰� �� ����
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

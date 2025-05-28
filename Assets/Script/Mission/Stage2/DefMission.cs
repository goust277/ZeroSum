using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DefMission : MonoBehaviour
{
    [Header("Monsters")]
    [SerializeField] private GameObject[] _monsters;
    [SerializeField] private bool isClearMonster = false;

    [SerializeField] private SceneLoadSetting missionSetting;

    [SerializeField] private float missionTime = 0f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    private bool isExe = false;
    public float curMissionTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (missionSetting.isMissionStart && !missionSetting.isMissionClear)
        {
            if (!isExe)
            {
                animator.SetTrigger("Exe");
                isExe = true;
            }
            if (curMissionTime >= missionTime)
            {
                missionSetting.isMissionClear = true;
                animator.SetTrigger("End");
                return;
            }

            curMissionTime += Time.deltaTime;

        }

        if (missionSetting.isMissionClear && !isClearMonster)
        {
            isClearMonster = true;

            DestroyAllMonster();
            
        }
    }

    private void DestroyAllMonster()
    {
        //foreach(var mon in _monsters)
        //{
        //    mon.SetActive(false);
        //    Debug.Log("���� ��Ȱ��ȭ");
        //}

        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        //foreach (GameObject monster in monsters)
        //{
        //    IDamageAble damageable = monster.GetComponent<IDamageAble>();

        //    if (damageable != null)
        //    {
        //        damageable.Damage(100);
        //    }
        //}

        //foreach (var mon in _monsters)
        //{
        //    mon.SetActive(true);
        //    Debug.Log("���� Ȱ��ȭ");
        //}

        // 2. ���� ��Ȱ��ȭ
        foreach (var mon in _monsters)
        {
            if (mon != null) mon.SetActive(false);
        }

        // 3. �±� ��� ���� �˻� (����Ʈ ����)
        List<GameObject> monsters = new List<GameObject>();
        monsters.AddRange(GameObject.FindGameObjectsWithTag("Monster"));

        // 4. ������ ó�� (���� ó�� ����)
        foreach (GameObject monster in monsters.ToArray()) // �迭�� ��ȯ
        {
            try
            {
                var damageable = monster?.GetComponent<IDamageAble>();
                damageable?.Damage(100);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"������ ó�� ����: {e.Message}");
            }
        }

        // 5. ���� ��Ȱ��ȭ
        foreach (var mon in _monsters)
        {
            if (mon != null) mon.SetActive(true);
        }
    }
}

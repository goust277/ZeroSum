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
        //    Debug.Log("몬스터 비활성화");
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
        //    Debug.Log("몬스터 활성화");
        //}

        // 2. 몬스터 비활성화
        foreach (var mon in _monsters)
        {
            if (mon != null) mon.SetActive(false);
        }

        // 3. 태그 기반 몬스터 검색 (리스트 갱신)
        List<GameObject> monsters = new List<GameObject>();
        monsters.AddRange(GameObject.FindGameObjectsWithTag("Monster"));

        // 4. 데미지 처리 (예외 처리 포함)
        foreach (GameObject monster in monsters.ToArray()) // 배열로 변환
        {
            try
            {
                var damageable = monster?.GetComponent<IDamageAble>();
                damageable?.Damage(100);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"데미지 처리 오류: {e.Message}");
            }
        }

        // 5. 몬스터 재활성화
        foreach (var mon in _monsters)
        {
            if (mon != null) mon.SetActive(true);
        }
    }
}

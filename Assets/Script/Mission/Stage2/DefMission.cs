using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefMission : MonoBehaviour
{
    [Header("Monsters")]
    [SerializeField] private GameObject[] _monsters;
    private bool isClearMonster = false;

    [SerializeField] private SceneLoadSetting missionSetting;

    [SerializeField] private float missionTime = 0f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    private bool isExe = false;
    private float curMissionTime = 0f;

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
        foreach(var mon in _monsters)
        {
            mon.SetActive(false);
        }

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            IDamageAble damageable = monster.GetComponent<IDamageAble>();
            if (damageable != null)
            {
                damageable.Damage(100);
                //damageable.Damage(1);
            }
        }

        foreach (var mon in _monsters)
        {
            mon.SetActive(true);
        }
    }
}

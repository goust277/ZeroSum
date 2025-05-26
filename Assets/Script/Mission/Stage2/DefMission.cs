using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefMission : MonoBehaviour
{


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
    }

    private void DestroyAllMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            IDamageAble damageable = monster.GetComponent<IDamageAble>();
            if (damageable != null)
            {
                damageable.Damage(1);
                damageable.Damage(1);
                damageable.Damage(1);
                damageable.Damage(1);
                damageable.Damage(1);
                damageable.Damage(1);
            }
        }
    }
}

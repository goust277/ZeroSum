using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1_Num3 : CutSceneBase
{
    [Header("Direct Resources")]
    [SerializeField] private Mission mission;
    [SerializeField] private Image missionCheck;
 
    private bool isClear;

    void Update()
    {
        if (mission.isClear)
        {
            if (!isClear)
            {
                isClear = true;
                missionCheck.enabled = true;
                dialogs[1].SetActive(false); //문 알림끄고
                StartCoroutine(Num3Scene());
            }
        }
    }

    private IEnumerator Num3Scene()
    {
        inputManager.SetActive(false); //입력못받게하고

        yield return new WaitForSeconds(0.5f);
        yield return ShowDialog(0, 2.0f); //유저가 대사하고

        inputManager.SetActive(true);
        dialogs[2].SetActive(true); //문 알림끄고
        trigger.enabled=true;//엘베 콜라이더 이제 켜줌
    }
}

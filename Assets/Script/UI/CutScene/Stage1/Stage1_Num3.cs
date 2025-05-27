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
                dialogs[1].SetActive(false); //�� �˸�����
                StartCoroutine(Num3Scene());
            }
        }
    }

    private IEnumerator Num3Scene()
    {
        inputManager.SetActive(false); //�Է¸��ް��ϰ�

        yield return new WaitForSeconds(0.5f);
        yield return ShowDialog(0, 2.0f); //������ ����ϰ�

        inputManager.SetActive(true);
        dialogs[2].SetActive(true); //�� �˸�����
        trigger.enabled=true;//���� �ݶ��̴� ���� ����
    }
}

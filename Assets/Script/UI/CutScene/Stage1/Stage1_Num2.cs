using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;


public class Stage1_Num2 : CutSceneBase
{
    [SerializeField] private TextMeshProUGUI keyTexts;
    private InputAction interactAction; //0

    private new void Start()
    {
        base.Start();

        PlayerInput playerInput = inputManager.GetComponent<PlayerInput>();

        interactAction = playerInput.actions["F"];

        keyTexts.text = "[" + interactAction.bindings[0].ToDisplayString();
        keyTexts.text += "] �� ���� �� ����";

        StartCoroutine(Num2Scene());
    }

    private IEnumerator Num2Scene()
    {
        inputManager.SetActive(false); //�Է¸��ް��ϰ�
                                       
        yield return new WaitForSeconds(0.5f);

        yield return ShowDialog(0, 2.0f); //������ ����ϰ�
        yield return StartCoroutine(MoveUIVerticallyDown(down, 180.0f));

        trigger.enabled = true; //�� �ݶ��̴� ���� ����
        inputManager.SetActive(true);
        dialogs[1].SetActive(true);
    }
}

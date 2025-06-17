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
        keyTexts.text += "] 를 눌러 문 입장";

        StartCoroutine(Num2Scene());
    }

    private IEnumerator Num2Scene()
    {
        inputManager.SetActive(false); //입력못받게하고
                                       
        yield return new WaitForSeconds(0.5f);

        yield return ShowDialog(0, 2.0f); //유저가 대사하고
        yield return StartCoroutine(MoveUIVerticallyDown(down, 180.0f));

        //trigger.enabled = true; //문 콜라이더 이제 켜줌
        //inputManager.SetActive(true);
        dialogs[1].SetActive(true); //보급 안내문 띄움

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F)); //F 받으면 다음꺼
        dialogs[2].SetActive(true); //체크표시
        yield return new WaitForSeconds(1.0f);
        dialogs[1].SetActive(false); //보급 안내문 띄움

        trigger.enabled = true; //문 콜라이더 이제 켜줌
        inputManager.SetActive(true);
        dialogs[3].SetActive(true); //보급 안내문 띄움
    }
}

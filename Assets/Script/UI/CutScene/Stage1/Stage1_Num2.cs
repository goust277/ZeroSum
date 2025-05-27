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

        trigger.enabled = true; //문 콜라이더 이제 켜줌
        inputManager.SetActive(true);
        dialogs[1].SetActive(true);
    }
}

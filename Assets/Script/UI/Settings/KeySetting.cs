using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용

public class KeySetting : MonoBehaviour
{
    [SerializeField] public PlayerInput playerInput; // PlayerInput 컴포넌트
    [SerializeField] private InputAction attackAction; // 변경할 입력 액션
    public string actionName;

    [Header("ActionBtns")]
    [SerializeField] private Button defaultAtkBtn; // UI 버튼
    [SerializeField] private Button parryingBtn;
    [SerializeField] private Button gunAtkBtn;

    private Dictionary<Button, string> buttonActionMap; // 버튼과 액션 이름을 매핑하는 딕셔너리

    private void Start()
    {

        // 버튼과 액션 이름 매핑
        buttonActionMap = new Dictionary<Button, string>
        {
            { defaultAtkBtn, "SwordAttack" },
            { parryingBtn, "Dash" },
            { gunAtkBtn, "GunAttack" }
        };

        //attackAction = playerInput.actions["Attack"]; // "Attack" 액션 가져오기
        // 버튼 이벤트 추가
        foreach (var entry in buttonActionMap)
        {
            entry.Key.onClick.AddListener(() => StartRebinding(entry.Key, entry.Value));
        }

        // UI 업데이트 (초기 키 표시)
        UpdateAllButtonTexts();
    }

    public void StartRebinding(Button rebindButton, string actionName)
    {
        InputAction action = playerInput.actions[actionName];

        if (action == null)
        {
            Debug.LogError($"❌ 오류: '{actionName}' 액션을 찾을 수 없습니다!");
            return;
        }

        TMP_Text buttonText = rebindButton.GetComponentInChildren<TMP_Text>();
        if (buttonText == null)
        {
            Debug.LogError($"❌ 오류: 버튼 {rebindButton.name} 안에 TMP_Text 컴포넌트가 없습니다!");
            return;
        }

        buttonText.text = "Press Any Key...";
        action.Disable();

        action.PerformInteractiveRebinding()
            .OnComplete(operation =>
            {
                action.Enable();
                UpdateButtonText(rebindButton, actionName); // UI 업데이트
                operation.Dispose();
            })
            .Start();
    }

    private void UpdateAllButtonTexts()
    {
        foreach (var entry in buttonActionMap)
        {
            UpdateButtonText(entry.Key, entry.Value);
        }
    }

    private void UpdateButtonText(Button button, string actionName)
    {
        InputAction action = playerInput.actions[actionName];
        if (action == null)
        {
            Debug.LogError($"❌ 오류: '{actionName}' 액션을 찾을 수 없습니다!");
            return;
        }

        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText == null)
        {
            Debug.LogError($"❌ 오류: 버튼 {button.name} 안에 TMP_Text 컴포넌트가 없습니다!");
            return;
        }

        buttonText.text = action.bindings[0].ToDisplayString();
    }

}

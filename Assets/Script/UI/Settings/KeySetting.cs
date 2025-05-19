using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용

public class KeySetting : MonoBehaviour
{
    [SerializeField] public PlayerInput playerInput; // PlayerInput 컴포넌트
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private InputAction attackAction; // 변경할 입력 액션
    public string actionName;

    [Header("ActionBtns")]
    [SerializeField] private Button defaultAtkBtn; // UI 버튼
    [SerializeField] private Button parryingBtn;
    [SerializeField] private Button gunAtkBtn;

    private Dictionary<Button, string> buttonActionMap; // button-action Mapping
    private Dictionary<string, string> tempMap = new Dictionary<string, string>(); // temp save before Applyyyyyyyyyyyyyy
                    //<액션이름, 바뀌는키> 일케 저장할거임

    [Header("saveBtn")]
    [SerializeField] private Button resetBtn;
    [SerializeField] private Button saveBtn;

    private Dictionary<string, string> defaultMap = new Dictionary<string, string>
    {
        { "SwordAttack", "<Keyboard>/a" },
        { "Dash", "<Keyboard>/x" },
        { "GunAttack", "<Keyboard>/c" }
    };


    private void Start()
    {
        // 버튼과 액션 이름 매핑
        buttonActionMap = new Dictionary<Button, string>
        {
            { defaultAtkBtn, "SwordAttack" },
            { parryingBtn, "Dash" },
            { gunAtkBtn, "GunAttack" }
        };

        //attackAction = playerInput.actions["SwordAttack"]; //액션 가져오기
        // 버튼 이벤트 추가
        foreach (var entry in buttonActionMap)
        {
            entry.Key.onClick.AddListener(() => StartRebinding(entry.Key, entry.Value));
        }

        resetBtn.onClick.AddListener(ResetBinding);
        saveBtn.onClick.AddListener(SaveRebinding);

        UpdateButtonTexts();
    }

    public void StartRebinding(Button rebindButton, string actionName)
    {
        audioSource.Play();
        InputAction action = playerInput.actions[actionName];

        if (action == null)
        {
            Debug.Log($"★☆★: '{actionName}' 액션을 찾을 수 없습니다!");
            return;
        }

        TMP_Text buttonText = rebindButton.GetComponentInChildren<TMP_Text>(); //버튼 내부 텍스트 받아옴
  
        buttonText.text = "Press Key";
        action.Disable();

        action.PerformInteractiveRebinding()
            .OnComplete(operation => //람다 ->. 완성되면
            {
                action.Enable(); // 액션 활성화 시키는데
                UpdateButtonText(rebindButton, actionName); // UI 업데이트
                operation.Dispose(); //
            })
            .Start();
    }

    public void ResetBinding()
    {
        audioSource.Play();
        foreach (var entry in defaultMap)
        {
            InputAction action = playerInput.actions[entry.Key];
            action.ApplyBindingOverride(0, entry.Value); // 기본 키로 덮어쓰기
        }
        tempMap.Clear();
        UpdateButtonTexts();

        TMP_Text buttonText = saveBtn.GetComponentInChildren<TMP_Text>();
        buttonText.text = "저장";
    }

    public void SaveRebinding()
    {
        audioSource.Play();
        foreach (var entry in tempMap)
        {
            string actionName = entry.Value;
            InputAction action = playerInput.actions[actionName];
            if (action == null) continue;

            // 바인딩된 키가 존재하는지 확인
            if (action.bindings.Count > 0)
            {
                // 첫 번째 바인딩(키보드 기준) 수정
                string newBindingPath = action.bindings[0].effectivePath;
                action.ApplyBindingOverride(0, newBindingPath);
            }
        }

        tempMap.Clear(); // 저장 후 임시 저장소 비우기

        TMP_Text buttonText = resetBtn.GetComponentInChildren<TMP_Text>();
        buttonText.text = "초기화";

        buttonText = saveBtn.GetComponentInChildren<TMP_Text>();
        buttonText.text = "저장 완료";
    }

    private void UpdateButtonTexts()
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
            Debug.Log("★☆★" + actionName + "cant find action");
            return;
        }

        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = action.bindings[0].ToDisplayString(); //액션 바인딩 된 키(바뀐거) 버튼 위에다가 바꿔줌
    }

}

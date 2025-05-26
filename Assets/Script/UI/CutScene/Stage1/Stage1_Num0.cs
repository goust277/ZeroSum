using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Stage1_Num0 : CutSceneBase
{
    [Header("Moving Img")]
    [SerializeField] private GameObject movingImg;
    [SerializeField] private Image[] keyChecks;
    [SerializeField] private TextMeshProUGUI[] keyTexts;

    private bool isComplete = false;
    private bool isLeftKeyDown = false;
    private bool isRightKeyDown = false;
    private bool isJumpKeyDown = false;

    private InputAction moveAction;
    private InputAction jumpAction;

    private new void Start()
    {
        base.Start();

        PlayerInput playerInput = inputManager.GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        keyTexts[2].text = jumpAction.bindings[0].ToDisplayString();

        inputManager.SetActive(false);

        StartCoroutine(Num0Scene());
    }

    private void Update()
    {
        if (!isComplete)
        {
            ScanInputValue();
        }
    }

    private IEnumerator Num0Scene()
    {
        proCamera2D.RemoveAllCameraTargets();
        up.SetActive(true);
        StartCoroutine(MoveUIVerticallyDown(up, 150.0f));


        yield return new WaitForSeconds(1.0f);
        yield return ShowDialog(0, 6.0f); //2

        movingImg.SetActive(true);
        inputManager.SetActive(true);
        yield return StartCoroutine(ScanInputValue());
        FinishSequence();
    }

    private IEnumerator ScanInputValue()
    {
        while (!isComplete)
        {
            Vector2 moveValue = moveAction.ReadValue<Vector2>();

            // 좌우 입력
            if (moveValue.x < -0.5f)
            {
                isLeftKeyDown = true;
                keyChecks[0].enabled = true;
            }
            if (moveValue.x > 0.5f)
            {
                isRightKeyDown = true;
                keyChecks[1].enabled = true;
            }
            // 점프 입력
            if (jumpAction.WasPressedThisFrame())
            {
                isJumpKeyDown = true;
                keyChecks[2].enabled = true;
            }

            if (isLeftKeyDown && isRightKeyDown && isJumpKeyDown)
                isComplete = true;

            yield return null;
        }
    }

    private void FinishSequence()
    {
        proCamera2D.AddCameraTarget(playerTarget, 1f, 1f, 0f, new Vector2(0f, 2f));

        isComplete = true;
        movingImg.SetActive(false);
        trigger.isTrigger = true;
    }

}

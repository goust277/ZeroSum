using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;


public class Stage1_Num1 : CutSceneBase
{

    [Header("BattleKey Img")]
    [SerializeField] private GameObject[] keyImg;
    [SerializeField] private Image[] keyChecks;
    [SerializeField] private TextMeshProUGUI[] keyTexts;
    [SerializeField] private Transform movesTransform;

    [Header("summoner")]
    [SerializeField] private GameObject summoner;


    private bool isComplete = false;
    private bool isParringKeyDown = false;
    private bool isDownKeyDown = false;
    private bool isGunAtkKeyDown = false;
    private bool isSwordAtkKeyDown = false;

    private int completeCnt = 0;

    private InputAction parringAction; //0
    private InputAction downAction; //1
    private InputAction gunAction; //2 
    private InputAction swordAtkAction; //3 

    private new void Start()
    {
        base.Start();

        PlayerInput playerInput = inputManager.GetComponent<PlayerInput>();

        parringAction = playerInput.actions["Dash"];
        downAction = playerInput.actions["Down"];
        gunAction = playerInput.actions["GunAttack"];
        swordAtkAction = playerInput.actions["SwordAttack"];

        keyTexts[0].text = parringAction.bindings[0].ToDisplayString();
        keyTexts[1].text = downAction.bindings[0].ToDisplayString();
        keyTexts[2].text = gunAction.bindings[0].ToDisplayString();
        keyTexts[3].text = swordAtkAction.bindings[0].ToDisplayString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            trigger.enabled = false;

            inputManager.SetActive(false); //�Է¸��ް��ϰ�
            proCamera2D.RemoveAllCameraTargets(); //ī�޶� �����ϰ� 
            player.GetComponent<PlayerAnimation>().enabled = false; // �ִϤӲ�
            StartCoroutine(Num1Scene());
        }
    }

    private IEnumerator Num1Scene()
    {
        //���� �ɾ���� ī�޶� �����س���
        MoveAndZoomTo((Vector2)cutsceneTarget[0].position, 3.5f, 2.0f);
        yield return MovePlayerTo(movesTransform, 3.0f);

        //ī�޶� �����ʰ��ٰ�
        MoveAndZoomTo((Vector2)cutsceneTarget[1].position, 3.5f, 2.0f);
        yield return new WaitForSeconds(2.0f);

        //������ ����ϰ�
        yield return ShowDialog(0, 2.0f); //2

        //����ȯ

        //�ǵ��ƿ���
        MoveAndZoomTo((Vector2)cutsceneTarget[0].position, 3.5f, 2.0f);
        yield return new WaitForSeconds(2.0f);

        //��ó��
        player.GetComponent<PlayerAnimation>().enabled = true;
        StartCoroutine(MoveUIVerticallyUp(up, 150.0f));
        GameStateManager.Instance.StartMoveUIDown(); //UI ��������
        proCamera2D.AddCameraTarget(playerTarget, 1f, 1f, 0f, new Vector2(0f, 2f)); //�÷��̾� �������ϰ�
        yield return new WaitForSeconds(1.0f);
        inputManager.SetActive(true);

        //Ű �Է¹޴°� ǥ��
        StartCoroutine(ScanInputValue());
    }

    private IEnumerator ScanInputValue()
    {
        while (!isComplete)
        {
            HandleInputStep();
            yield return null;
        }
    }

    private void HandleInputStep()
    {
        switch (completeCnt)
        {
            case 0:
                keyImg[0].SetActive(true);
                if (parringAction.WasPressedThisFrame())
                {
                    isParringKeyDown = true;
                    keyChecks[0].enabled = true;
                    StartCoroutine(AdvanceStep(0));
                }
                break;

            case 1:
                keyImg[1].SetActive(true);
                if (downAction.WasPressedThisFrame())
                {
                    isDownKeyDown = true;
                    keyChecks[1].enabled = true;
                    StartCoroutine(AdvanceStep(1));
                }
                break;

            case 2:
                keyImg[2].SetActive(true);
                if (gunAction.WasPressedThisFrame())
                {
                    isGunAtkKeyDown = true;
                    keyChecks[2].enabled = true;
                    StartCoroutine(AdvanceStep(2));
                }
                break;

            case 3:
                keyImg[3].SetActive(true);
                if (swordAtkAction.WasPressedThisFrame())
                {
                    isSwordAtkKeyDown = true;
                    keyChecks[3].enabled = true;
                    StartCoroutine(AdvanceStep(3));
                }
                break;

            case 4:
                isComplete = true;
                break;
        }
    }

    private IEnumerator AdvanceStep(int index)
    {
        yield return new WaitForSeconds(1.5f);
        keyImg[index].SetActive(false);
        completeCnt++;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Stage1_Num4 : CutSceneBase
{
    [SerializeField] public BoxCollider2D ev;

    [Header("Fadeout Light")]
    [SerializeField] private Light2D light2D;
    [SerializeField] private float fadeDuration = 5f;
    [SerializeField] private Transform move;

    [Header("Before LastCutScene")]
    [SerializeField] private GameObject missionUI;
    [SerializeField] private MissionDoorManager tutorialMissionManager;
    [SerializeField] private MissionDoorManager realMissionDoorManager;

    [Header("Tutorial End")]
    [SerializeField] private GameObject cutSceneTrigger;
    [SerializeField] private Rigidbody2D[] monster;
    private GameObject skipper;

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        skipper = GameObject.Find("SkipDetect");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            trigger.enabled = false;
            ev.enabled = false;
            hasPlayed = true;

            // �÷��̾� ����ٴϴ� �� ����
            proCamera2D.RemoveAllCameraTargets();

            GameStateManager.Instance.StartMoveUIUp(); //UI�ö󰡱�
            up.SetActive(true);
            down.SetActive(true);
            StartCoroutine(MoveUIVerticallyDown(up, 100.0f)); //������ ��������
            StartCoroutine(MoveUIVerticallyUp(down, 100.0f)); //�Ʒ����� �ö����

            StartCoroutine(Num4Scene());
            StartCoroutine(GrowAndFade());
        }
    }
    private IEnumerator Num4Scene()
    {
        StartCoroutine(MoveUIVerticallyUp(missionUI, 180.0f)); //uiâ �ø�
        yield return new WaitForSeconds(0.5f);
        inputManager.SetActive(false);
        MoveAndZoomTo((Vector2)cutsceneTarget[0].position, 3.5f, 2.0f); //ī�޶����
        yield return MovePlayerTo(move, 2.0f); //�÷��̾������

        yield return ShowDialog(0, 6.0f); //������ ����ϰ�

        dialogs[1].SetActive(true); //�� �˸�����
        MoveAndZoomTo((Vector2)cutsceneTarget[1].position, 7.46f, 2.0f); //�Ʒ���
        yield return new WaitForSeconds(2.5f);
        MoveAndZoomTo((Vector2)cutsceneTarget[2].position, 7.46f, 4.5f); //����
        yield return new WaitForSeconds(4.7f);
        MoveAndZoomTo((Vector2)cutsceneTarget[3].position, 6.1f, 2.0f); //����
        yield return new WaitForSeconds(3.2f);

        trigger.enabled = true;//���� �ݶ��̴� ���� ����
        StartCoroutine(MoveUIVerticallyDown(missionUI, 180.0f)); //uiâ �ø�
        OnTutorialEnd();
    }

    private IEnumerator GrowAndFade()
    {
        float timer = 0f;
        float startFalloff = 1f;
        float endFalloff = 0.2f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            light2D.pointLightOuterRadius = Mathf.Lerp(0f, 10f, t);
            light2D.falloffIntensity = Mathf.Lerp(startFalloff, endFalloff, t);
            // intensity�� �ǵ帮�� ����
            yield return null;
        }

        light2D.enabled = false;
        SwapMissionUI();
    }

    public void SwapMissionUI()
    {
        tutorialMissionManager.enabled = false;
        realMissionDoorManager.enabled = true;
    }

    public void MonsterEnable()
    {
        monster[0].constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        monster[1].constraints &= ~RigidbodyConstraints2D.FreezePositionX;
    }

    private void OnTutorialEnd()
    {
        skipper.GetComponent<TutorialSkipper>().ConnectPause();
        skipper.SetActive(false);

        ev.enabled = true;
        EndCutScene();
        MonsterEnable();
        cutSceneTrigger.SetActive(false);
    }
}

using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using System.Collections;
using UnityEngine;

public class Stage2_Num3 : CutSceneBase
{
    [SerializeField] private GameObject npc;
    [SerializeField] private Transform[] movesTransform;
    [SerializeField] private MovingBlock[] evs;
    [SerializeField] private MonsterDoor[] mDoors;
    [SerializeField] private SceneLoadSetting sceneLoadSetting;
    private bool isPlay = false;
    private Animator npcAnimator;

    [Header("Shutter")]
    [SerializeField] private GameObject[] shutters;
    private Animator[] shuttersAnimators = new Animator[2];

    [Header("Error Ani")]
    [SerializeField] private GameObject error9;
    private new void Start()
    {
        base.Start();
        npcAnimator = npc.GetComponent<Animator>();
        trigger.enabled = false;

        shuttersAnimators[0] = shutters[0].GetComponent<Animator>(); //left
        shuttersAnimators[1] = shutters[1].GetComponent<Animator>(); //right
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlay) {
            if (sceneLoadSetting.isMissionClear)
            {
                StartClearCutScene();
            }
        }
    }

    private void StartClearCutScene()
    {
        isPlay = true;

        if (inputManager != null)
            inputManager.SetActive(false);

        GameStateManager.Instance.StartMoveUIUp();
        StartCoroutine(Num3Scene());
    }

    private IEnumerator Num3Scene()
    {
        mDoors[0].enabled = false;
        mDoors[1].enabled = false;
        shutters[0].GetComponent<Collider2D>().enabled = true;
        shutters[1].GetComponent<Collider2D>().enabled = true;

        error9.SetActive(false);

        evs[1].enabled = true;
        StartCoroutine(MoveUIVerticallyDown(down,130.0f));
        StartCoroutine(MoveUIVerticallyUp(up, 100.0f));
        yield return new WaitForSeconds(0.5f);

        npc.GetComponent<NPC_Hp>().enabled = false;
        StartCoroutine(MovePlayerTo(movesTransform[0], 5.0f));
        yield return new WaitForSeconds(1.0f);

        yield return ShowDialog(0, 2.0f); //1

        npcAnimator.SetBool("Mission", false);

        yield return ShowDialog(1, 16.0f); //2

        //너는 어쩔생각이지?
        playerTarget.rotation = Quaternion.Euler(0, 0, 0);
        yield return ShowDialog(2, 2.0f); //3

        //나?
        yield return ShowDialog(3, 5.0f); //4

        //위험?
        yield return ShowDialog(4, 2.0f); //5

        yield return ShowDialog(5, 8.0f); //4

        //부서지는 소리, 화면 흔들림
        npcAnimator.SetBool("Down2", true);
        yield return new WaitForSeconds(1.0f);
        dialogs[6].SetActive(true);
        yield return new WaitForSeconds(5.0f);
        //엘리베이터 가서 서기
        StartCoroutine(MovePlayerTo(movesTransform[1], 3.0f));
        MoveAndZoomTo((Vector2)cutsceneTarget[1].position, 3.6f, 2.0f);

        yield return new WaitForSeconds(3.0f);
        dialogs[6].SetActive(false);

        //데리러오겠다
        yield return ShowDialog(7, 3.0f); //7

        yield return ShowDialog(8, 3.0f); //7
        // 셔터 내려오기

        shuttersAnimators[0].Play("Shutter_Down");
        shuttersAnimators[1].Play("Shutter_Down");

        shutters[0].GetComponent<Collider2D>().offset = new Vector2(-0.9f, -2.25f);
        shutters[1].GetComponent<Collider2D>().offset = new Vector2(0.9f, -2.25f);

        // 그믐 주저앉기
        npcAnimator.enabled = false;
        npcAnimator.StopPlayback();

        //엘리베이터 동작
        evs[0].enabled= true;
        trigger.enabled = true;

        MoveAndZoomTo((Vector2)cutsceneTarget[2].position, 3.0f, 1.0f);
        yield return ShowDialog(9, 3.0f); //7


        //줌아웃
        EndCutScene();
    }
}

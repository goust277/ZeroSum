using System.Collections;
using UnityEngine;

public class Stage2_Num3 : CutSceneBase
{
    [SerializeField] private GameObject npc;
    [SerializeField] private Transform[] movesTransform;
    [SerializeField] private MovingBlock[] evs;
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
        error9.SetActive(false);

        evs[1].enabled = true;
        StartCoroutine(MoveUIVerticallyDown(down,130.0f));
        StartCoroutine(MoveUIVerticallyUp(up, 100.0f));
        StartCoroutine(MovePlayerTo(movesTransform[0], 5.0f));
        yield return new WaitForSeconds(1.0f);

        yield return ShowDialog(0, 2.0f); //1
        yield return ShowDialog(1, 6.0f); //2
        yield return ShowDialog(2, 2.0f); //3
        yield return ShowDialog(3, 3.0f); //4

        yield return ShowDialog(4, 2.0f); //5
        //ø§∏Æ∫£¿Ã≈Õ ∞°º≠ º≠±‚
        StartCoroutine(MovePlayerTo(movesTransform[1], 3.0f));
        yield return new WaitForSeconds(2.0f);
        yield return ShowDialog(5, 2.0f); //6
        yield return ShowDialog(6, 3.0f); //7
        // º≈≈Õ ≥ª∑¡ø¿±‚

        shuttersAnimators[0].Play("Shutter_Down");
        shuttersAnimators[1].Play("Shutter_Down");

        shutters[0].GetComponent<Collider2D>().offset = new Vector2(-0.9f, -2.25f);
        shutters[1].GetComponent<Collider2D>().offset = new Vector2(0.9f, -2.25f);

        shutters[0].GetComponent<Collider2D>().enabled = true;
        shutters[1].GetComponent<Collider2D>().enabled = true;

        // ±◊π  ¡÷¿˙æ…±‚
        npcAnimator.SetBool("Down", true);
        yield return new WaitForSeconds(0.49f); // øπ: 0.5√ 
        npcAnimator.enabled = false;
        npcAnimator.StopPlayback();

        //ø§∏Æ∫£¿Ã≈Õ µø¿€
        evs[0].enabled= true;
        trigger.enabled = true;

        //¡‹æ∆øÙ
        EndCutScene();
    }
}

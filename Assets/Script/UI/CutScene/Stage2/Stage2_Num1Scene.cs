using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage2_Num1Scene : CutSceneBase
{
    [SerializeField] private MovingBlock[] evs;
    [SerializeField] private Transform move;
    [SerializeField] private Vector3 npcmove;

    [SerializeField] private GameObject fade;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject scout;
    [SerializeField] private GameObject scout_die;
    [SerializeField] private AudioSource battleAudio;
    [SerializeField] private AudioSource runAudio;

    [Header("Error Ani")]
    [SerializeField] private GameObject error9;
    [SerializeField] private Image error9img;
    [SerializeField] private TextMeshProUGUI error9txt;

    private Animator npcAnimator;
    private bool isAnyBlockTriggered = false;


    private new void Start()
    {
        base.Start();
        npcAnimator = npc.GetComponent<Animator>();
        //npc.GetComponent<NPCController>().enabled = false;
    }

    private void Update()
    {

        if (isAnyBlockTriggered) return; // 이미 감지했으면 다시 확인하지 않음

        foreach (var block in evs)
        {
            if (block != null && block.inPlayer)
            {
                Debug.Log("💡 플레이어가 블록에 닿음!");
                isAnyBlockTriggered = true;

                OnBlockTriggered(); // 원하는 동작 호출
                break; // 하나만 감지하면 됨
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            trigger.enabled= false;
            evs[0].enabled = false;
            evs[1].enabled = false;

            GameStateManager.Instance.StartMoveUIUp(); //UI올라가기
            proCamera2D.RemoveAllCameraTargets();

            StartCoroutine(From1To3());
        }
    }

    private void OnBlockTriggered()
    {
        
        up.SetActive(true);
        down.SetActive(true);
        StartCoroutine(MoveUIVerticallyDown(up, 100.0f)); //위에서 내려오기
        StartCoroutine(MoveUIVerticallyUp(down, 100.0f)); //아래에서 올라오기

        if (inputManager != null) //입력못받게하고
            inputManager.SetActive(false);
    }


    private void KillScout()
    {
        scout.transform.position = new Vector3(8.0f, npcmove.y, 0.0f);
        scout.GetComponent<Scout>().Damage(1);
        scout_die.SetActive(true);
    }

    private IEnumerator From1To3(){
        MoveAndZoomTo((Vector2)cutsceneTarget[0].position, 4.0f, 2.0f);

        yield return MovePlayerTo(move, 2.0f);

        yield return ShowDialog(0, 6.5f);

        MoveAndZoomTo((Vector2)cutsceneTarget[1].position, 4.0f, 2.0f);
        yield return new WaitForSeconds(3.0f);
        playerTarget.rotation = Quaternion.Euler(0, 0, 0);

        dialogs[1].SetActive(true);
        yield return new WaitForSeconds(2.0f);
        //6
        dialogs[2].SetActive(true);
        yield return new WaitForSeconds(2.0f);
        dialogs[1].SetActive(false);
        dialogs[2].SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            runAudio.PlayOneShot(runAudio.clip);
            yield return new WaitForSeconds(0.2f);
        }

        SpriteRenderer sr = npc.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 4;
        npc.transform.position = npcmove;
        yield return new WaitForSeconds(1.0f);

        //NPC IDLE 뒤집기
        dialogs[3].SetActive(true);
        //7~8
        npc.transform.rotation = Quaternion.Euler(0, 0, 0); //우
        yield return new WaitForSeconds(2.0f);
        npc.transform.rotation = Quaternion.Euler(0, 180, 0); //좌
        yield return new WaitForSeconds(3.0f);
        dialogs[3].SetActive(false);
        dialogs[4].SetActive(true);
        //두리번두리번
        npc.transform.rotation = Quaternion.Euler(0, 0, 0); //우
        yield return new WaitForSeconds(1.0f);
        npc.transform.rotation = Quaternion.Euler(0, 180, 0); //좌
        yield return new WaitForSeconds(1.0f);
        dialogs[4].SetActive(false);
        npc.transform.rotation = Quaternion.Euler(0, 0, 0);
        // 9
        yield return new WaitForSeconds(1.0f);
        npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        StartCoroutine(ErrorBlink());
        // 10
        yield return ShowDialog(5, 3.0f);

        // 11
        dialogs[6].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        dialogs[7].SetActive(true); //12
        yield return new WaitForSeconds(2.0f);
        dialogs[7].SetActive(false);
        dialogs[6].SetActive(false);

        //13
        yield return ShowDialog(8, 2.0f);

        //14
        yield return ShowDialog(9, 4.0f);

        //15
        yield return ShowDialog(10, 2.0f);

        //16
        dialogs[11].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        dialogs[12].SetActive(true);
        yield return new WaitForSeconds(4.0f);
        dialogs[11].SetActive(false);
        dialogs[12].SetActive(false);

        yield return ShowDialog(13, 2.0f);

        //정찰자  걸어와야함
        MoveAndZoomTo((Vector2)cutsceneTarget[2].position, 5.12f, 2.0f);
        yield return new WaitForSeconds(1.8f);
        scout.SetActive(true);

        dialogs[14].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        dialogs[15].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        npc.transform.rotation = Quaternion.Euler(0, 0, 0); //우
        yield return new WaitForSeconds(1.0f);
        dialogs[14].SetActive(false);
        dialogs[15].SetActive(false);

        //숙이기
        npcAnimator.SetBool("Down", true);
        yield return new WaitForSeconds(0.49f); // 예: 0.5초
        npcAnimator.enabled = false;
        npcAnimator.StopPlayback();
        //쏘기 모션
        fade.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        battleAudio.PlayOneShot(battleAudio.clip);
        yield return new WaitForSeconds(0.3f);
        battleAudio.PlayOneShot(battleAudio.clip);
        yield return new WaitForSeconds(0.5f);
        //scout.GetComponent<Scout>().Damage(2);
        //scout.SetActive(false);
        battleAudio.PlayOneShot(battleAudio.clip);
        KillScout();
        MoveAndZoomTo((Vector2)cutsceneTarget[3].position, 4.0f, 1.0f);
        yield return new WaitForSeconds(1.0f);
        fade.SetActive(false);
        //숙인거 다시 일어서기
        yield return new WaitForSeconds(0.5f);
        npcAnimator.enabled = true;
        npcAnimator.SetBool("Down", false);
        npcAnimator.SetBool("Up", true);
        yield return new WaitForSeconds(0.5f);
        npcAnimator.SetBool("Up", false);

        //21
        yield return ShowDialog(16, 3.0f);
        yield return new WaitForSeconds(1.0f);
        yield return ShowDialog(17, 3.0f);

        npc.GetComponent<NPCController>().enabled = true;
        yield return new WaitForSeconds(1.0f);
        MoveAndZoomTo((Vector2)cutsceneTarget[4].position, 7.9f, 2.0f);
        yield return new WaitForSeconds(2.5f);
        MoveAndZoomTo((Vector2)cutsceneTarget[5].position, 7.9f, 5.0f);
        yield return new WaitForSeconds(6.0f);
        scout_die.SetActive(true);
        EndCutScene();
        evs[0].enabled = true;
        evs[1].enabled = true;

        error9.SetActive(false);
    }

    IEnumerator ErrorBlink()
    {
        Color32 fromColor = new Color32(255, 0, 0, 255);
        Color32 toColor = new Color32(118, 0, 0, 255);
        float duration = 1f;

        error9.SetActive(true);
        float endAlpha = 70f / 255f;
        Color originalColor = error9img.color;
        float elapsed = 0f;
        
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / 1f);

            float newAlpha = Mathf.Lerp( 0f, endAlpha, t);
            error9img.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            yield return null;
        }

        while (true)
        {
            // 1. from → to
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                error9txt.color = Color32.Lerp(fromColor, toColor, t);
                yield return null;
            }

            // 2. to → from
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                error9txt.color = Color32.Lerp(toColor, fromColor, t);
                yield return null;
            }
        }
    }
}

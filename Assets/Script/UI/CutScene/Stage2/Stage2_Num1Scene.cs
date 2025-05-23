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
    [SerializeField] private AudioSource battleAudio;
    [SerializeField] private AudioSource runAudio;

    [Header("Error Ani")]
    [SerializeField] private GameObject error9;
    [SerializeField] private Image error9img;
    [SerializeField] private TextMeshProUGUI error9txt;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            trigger.enabled= false;
            evs[0].enabled = false;
            evs[1].enabled = false;
            StartCutScene();
            StartCoroutine(From1To3());
        }
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
            yield return new WaitForSeconds(0.3f);
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
        yield return new WaitForSeconds(4.0f);
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

        //쏘기 모션
        yield return new WaitForSeconds(1.0f);
        fade.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        battleAudio.PlayOneShot(battleAudio.clip);
        yield return new WaitForSeconds(0.3f);
        battleAudio.PlayOneShot(battleAudio.clip);
        yield return new WaitForSeconds(0.5f);
        scout.GetComponent<Scout>().Damage(2);
        scout.SetActive(false);
        battleAudio.PlayOneShot(battleAudio.clip);
        MoveAndZoomTo((Vector2)cutsceneTarget[3].position, 4.0f, 1.0f);
        yield return new WaitForSeconds(1.0f);
        fade.SetActive(false);

        //숙인거 다시 일어서기
        yield return new WaitForSeconds(1.0f);

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
        EndCutScene();
        evs[0].enabled = true;
        evs[1].enabled = true;
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

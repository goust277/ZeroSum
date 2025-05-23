using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Stage2_Num2 : CutSceneBase
{
    [SerializeField] private MovingBlock[] evs;
    [SerializeField] private GameObject npc;
    [SerializeField] private Transform[] npcmoves;

    [Header("Error Ani")]
    [SerializeField] private GameObject error9;
    [SerializeField] private Image error9img;
    [SerializeField] private TextMeshProUGUI error9txt;

    private Transform npcTarget;
    private Animator npcAnimator;

    private new void Start()
    {
        base.Start();
        npcAnimator = npc.GetComponent<Animator>();
        npcTarget = npc.transform;
        //npc.GetComponent<NPCController>().enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            trigger.enabled = false;
            
            //evs[1].enabled = false;
            npc.GetComponent<NPCController>().enabled = false;
            StartCutScene();
            StartCoroutine(Num2Scene());
        }
    }

    private IEnumerator Num2Scene()
    {
        MoveAndZoomTo((Vector2)cutsceneTarget[0].position, 5.5f, 2.0f);
        yield return new WaitForSeconds(1.0f);

        npc.transform.position = new Vector3(npcmoves[0].position.x, npcmoves[0].position.y, npcmoves[0].position.z);
        yield return MoveNpcTo(npcmoves[1], "Walk");
        yield return ShowDialog(0, 2.0f); //1
        StartCoroutine(MoveNpcTo(npcmoves[2], "Run"));
        MoveAndZoomTo((Vector2)cutsceneTarget[1].position, 3.5f, 3.0f);
        yield return MovePlayerTo(npcmoves[3], 5.0f);
        //2,3
        yield return ShowDialog(1, 4.0f);
        //터치나 타자 효과음 > 잠시잠잠했다가 오류뜨는게 나아보이는데
        error9.SetActive(false);
        yield return new WaitForSeconds(2.0f);

        yield return ShowDialog(2, 2.0f); //4
        //5
        StartCoroutine(ErrorBlink());
        evs[0].enabled = false;
        yield return new WaitForSeconds(2.0f);

        MoveAndZoomTo((Vector2)cutsceneTarget[2].position, 4.0f, 2.0f);
        yield return new WaitForSeconds(3.0f);
        MoveAndZoomTo((Vector2)cutsceneTarget[3].position, 4.0f, 2.0f);
        yield return new WaitForSeconds(3.0f);

        MoveAndZoomTo((Vector2)cutsceneTarget[1].position, 4.0f, 2.0f);
        yield return new WaitForSeconds(3.0f);

        yield return ShowDialog(3, 3.0f); //6
        yield return ShowDialog(4, 2.0f); //7
        yield return ShowDialog(5, 1.5f); //8
        yield return ShowDialog(6, 3.0f); //7

        EndCutScene();
    }

    private IEnumerator MoveNpcTo(Transform targetPoint, string motionName)
    {
        float moveSpeed = 1.5f;
        if (motionName == "Run")
        {
            moveSpeed = 2f;
        }

        float startTime = Time.time;

        // y, z 고정한 목표 위치
        Vector3 targetPos = new Vector3(targetPoint.position.x, npcTarget.position.y, npcTarget.position.z);

        // 현재 오른쪽 바라보는 상태인가? → y=0이면 true
        bool isFacingRight = Mathf.Approximately(npcTarget.eulerAngles.y, 0f);

        // 목표 위치에 따라 회전 조절
        if (targetPoint.position.x < npcTarget.position.x && isFacingRight)
        {
            // 왼쪽으로 회전
            npcTarget.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (targetPoint.position.x > npcTarget.position.x && !isFacingRight)
        {
            // 오른쪽으로 회전
            npcTarget.rotation = Quaternion.Euler(0, 0, 0);
        }

        // 애니메이션 시작
        npcAnimator.SetBool(motionName, true);

        while (Vector3.Distance(npcTarget.position, targetPos) > 0.05f)
        {
            Vector3 dir = (targetPos - npcTarget.position).normalized;

            // 혹시라도 dir이 0이거나 NaN이면 바로 빠져나오기
            if (dir == Vector3.zero || float.IsNaN(dir.x))
            {
                Debug.LogWarning("이동 방향이 0이거나 계산 불가능 (NaN)합니다.");
                break;
            }

            npcTarget.position += dir * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // 위치 보정
        npcTarget.position = targetPos;
        // 애니메이션 정지
        npcAnimator.SetBool(motionName, false);
        npcAnimator.Play("Idle");
    }

    IEnumerator ErrorBlink()
    {
        Color32 fromColor = new Color32(255, 0, 0, 255);
        Color32 toColor = new Color32(118, 0, 0, 255);
        float duration = 1f;

        error9.SetActive(true);

        error9txt.text = "보안 등급 2단계. 보안 등급 2단계. \n 침입자를 저지하세요.";
        float endAlpha = 70f / 255f;
        error9img.color = new Color(toColor.r, toColor.g, toColor.b, 10f);
        Color originalColor = error9img.color;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / 1f);

            float newAlpha = Mathf.Lerp(0f, endAlpha, t);
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

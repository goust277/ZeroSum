using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using TMPro.Examples;

public class CutSceneBase : MonoBehaviour
{
    [SerializeField] protected ProCamera2D proCamera2D;
    [SerializeField] protected Transform[] cutsceneTarget; // 컷씬 시 볼 지점
    [SerializeField] protected GameObject[] dialogs; // 컷씬 시 볼 지점

    protected GameObject player;
    protected Transform playerTarget;
    protected GameObject inputManager;
    [SerializeField] protected Animator playerAnimator;

    [SerializeField] protected float zoomDuringCutscene = 4.5f;
    [SerializeField] protected float zoomSpeed = 1.5f;

    protected float originOrthographic = 6.7f;
    protected bool hasPlayed = false;
    private Coroutine moveCoroutine;



    [Header("BackGround")]
    [SerializeField] protected GameObject up;
    [SerializeField] protected GameObject down;

    protected void Start()
    {
        player = GameObject.Find("Player");
        playerTarget = player.transform;
        inputManager = GameObject.Find("InputManager");

        if(inputManager == null || player == null)
        {
            Debug.LogError("못찾음");
        }

    }

    protected void StartCutScene()
    {
        // 플레이어 따라다니는 거 제거
        proCamera2D.RemoveAllCameraTargets();

        up.SetActive(true);
        down.SetActive(true);
        StartCoroutine(MoveUIVerticallyDown(up, 100.0f));
        StartCoroutine(MoveUIVerticallyUp(down, 100.0f));

        if (inputManager != null)
            inputManager.SetActive(false);
    }

    protected void EndCutScene()
    {
        float startZoom = Camera.main.orthographicSize;

        proCamera2D.AddCameraTarget(playerTarget, 0f, 2f);
        proCamera2D.Zoom(originOrthographic - startZoom, 2.0f);

        player.GetComponent<PlayerAnimation>().enabled = true;
        up.SetActive(false);
        down.SetActive(false);
        StartCoroutine(MoveUIVerticallyDown(down, 100.0f));
        StartCoroutine(MoveUIVerticallyUp(up, 100.0f));

        if (inputManager != null)
            inputManager.SetActive(true);
    }

    public void MoveAndZoomTo(Vector2 targetPosition, float targetZoom, float duration)
    {
        StartCoroutine(MoveCameraCoroutine(targetPosition, targetZoom, duration));
    }

    private IEnumerator MoveCameraCoroutine(Vector2 targetPosition, float targetZoom, float duration)
    {
        Vector3 startPos = proCamera2D.transform.position;
        Vector3 endPos = new Vector3(targetPosition.x, targetPosition.y, startPos.z);

        float startZoom = Camera.main.orthographicSize;
        float elapsed = 0f;

        proCamera2D.Zoom(targetZoom - startZoom, duration);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            proCamera2D.transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        proCamera2D.transform.position = endPos;
    }

    private IEnumerator MoveUIVerticallyDown(GameObject targetObj, float distance)
    {
        RectTransform target = targetObj.transform as RectTransform;

        Vector2 startPos = target.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0f, distance *-1f);
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;  // TimeScale 영향 없이 실행
            float t = Mathf.Clamp01(elapsed / duration);
            target.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        target.anchoredPosition = endPos;
    }

    private IEnumerator MoveUIVerticallyUp(GameObject targetObj, float distance)
    {
        RectTransform target = targetObj.transform as RectTransform;

        Vector2 startPos = target.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0f, distance);
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;  // TimeScale 영향 없이 실행
            float t = Mathf.Clamp01(elapsed / duration);
            target.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        target.anchoredPosition = endPos;
    }

    public void StartPlayerMove(Transform targetTransform, float duration)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MovePlayerTo(targetTransform, duration));
    }

    protected IEnumerator MovePlayerTo(Transform targetPoint, float duration)
    {
        float moveSpeed = 2f;
        float startTime = Time.time;

        // y, z 고정한 목표 위치
        Vector3 targetPos = new Vector3(targetPoint.position.x, playerTarget.position.y, playerTarget.position.z);

        // 현재 오른쪽 바라보는 상태인가? → y=0이면 true
        bool isFacingRight = Mathf.Approximately(playerTarget.eulerAngles.y, 0f);

        // 목표 위치에 따라 회전 조절
        if (targetPoint.position.x < playerTarget.position.x && isFacingRight)
        {
            // 왼쪽으로 회전
            playerTarget.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (targetPoint.position.x > playerTarget.position.x && !isFacingRight)
        {
            // 오른쪽으로 회전
            playerTarget.rotation = Quaternion.Euler(0, 0, 0);
        }

        // 애니메이션 시작
        player.GetComponent<PlayerAnimation>().enabled = false;
        playerAnimator.SetBool("Move", true);

        while (Mathf.Abs(playerTarget.position.x - targetPos.x) > 0.05f && Time.time - startTime < duration)
        {
            Vector3 dir = (targetPos - playerTarget.position).normalized;
            playerTarget.position += dir * moveSpeed * Time.deltaTime;

            yield return null;
        }

        // 위치 보정
        playerTarget.position = targetPos;
        // 애니메이션 정지
        playerAnimator.SetBool("Move", false);
        playerAnimator.Play("Idle");

        moveCoroutine = null;
    }


}

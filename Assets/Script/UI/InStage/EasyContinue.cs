using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UnityLinker;
using UnityEngine;
using UnityEngine.UI;

public class EasyContinue : MonoBehaviour
{
    [SerializeField] private ProCamera2D proCamera2D;
    [SerializeField] private float blockingDuration = 5.0f;

    [Header("Camera Resource")]
    [SerializeField] private readonly float targetSize = 4.5f;
    [SerializeField] private readonly float duration = 1.0f;

    [Header("BackGround Resource")]
    [SerializeField] Image redImg;
    [SerializeField] Image blackImg;

    private GameObject playerObj;
    private Animator animator;
    private Camera cam;

    private void Start()
    {
        playerObj = GameObject.Find("Player");
        animator = GameObject.Find("Sprite").GetComponent<Animator>();

        cam = Camera.main;
        if (proCamera2D == null)
        {
            proCamera2D = cam.GetComponent<ProCamera2D>();
            if (proCamera2D == null)
            {
                Debug.LogWarning("Main Camera에 ProCamera2D가 없습니다!");
            }
        }
        StartCoroutine(FadeOutandIn());
    }

    private IEnumerator FadeOutandIn()
    {

        yield return StartCoroutine(HandleGameOverSequence()); // 느려지면서 줌인 + 알파
        yield return new WaitForSecondsRealtime(0.3f); // 약간의 텀 (옵션)

        Time.timeScale = 1; // 게임 재개
        
        float timer = 0f;
        float startZoom = Camera.main.orthographicSize;
        proCamera2D.Zoom(6.7f - startZoom, 1.0f);

        animator.SetBool("Dead", false);
        animator.Play("Idle");
        Ver01_DungeonStatManager.Instance.ResetDungeonState();
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alphaValue = duration / timer;

            // 검은 화면 점점 밝게 설정
            blackImg.color = new Color(blackImg.color.r, blackImg.color.g, blackImg.color.b, alphaValue);
            yield return null;
        }

        if (playerObj != null)
        {
            PlayerHP playerHP = playerObj.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                Debug.Log("FadeOutandIn - playerHP 찾았음 !! 실행중");
                playerHP.ContinueProcessing(blockingDuration);
            }
        }

        Destroy(gameObject, 1.0f);
    }

    IEnumerator HandleGameOverSequence()
    {
        Color color = redImg.color;

        float startSize = cam.orthographicSize;
        float time = 0f;

        Time.timeScale = 0.5f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime; // timeScale 영향을 안 받게
            float easedT = Mathf.SmoothStep(0f, 1f, time / duration);
            float alpha = Mathf.Lerp(0.0f, 1.0f, time / duration);
            float redAlpha = Mathf.Lerp(0f, 1.0f, time / duration);

            redImg.color = new Color(color.r, color.g, color.b, redAlpha);
            blackImg.color = new Color(color.r, color.g, color.b, alpha);
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, time / duration);

            yield return null;
        }

        //GameStateManager.Instance.StartMoveUIUp();
        Time.timeScale = 0.0f;
    }

}

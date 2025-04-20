using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("Camera Resource")]
    [SerializeField] private float targetSize = 2.0f;
    [SerializeField] private float duration = 2.0f;

    [Header("BackGround Resource")]
    [SerializeField] Image redImg;
    [SerializeField] Image blackImg;
    [SerializeField] private GameObject deadBodyPrefab;
    [SerializeField] private Canvas gameOverCanvas;

    [Header("GameOverUI Resource")]
    [SerializeField] GameObject gameOverUI;
    [SerializeField] RectTransform up;
    [SerializeField] RectTransform down;
    [SerializeField] private float blackBarTargetY = 130f; // 이동할 거리
    [SerializeField] private float blackBarSlideAmount = 130f;

    private Transform player;
    private GameObject camObj;
    private Camera cam;
    private ProCamera2D proCamera2D;
    Vector2 upStartPos;
    Vector2 downStartPos;

    private void Start()
    {
        upStartPos = up.anchoredPosition;
        downStartPos = down.anchoredPosition;


        camObj = GameObject.FindWithTag("MainCamera");
        if (camObj == null)
        {
            Debug.LogError("GameOver - 카메라 못찾음 ");
            return;
        }

        cam = camObj.GetComponent<Camera>();
        proCamera2D = camObj.GetComponent<ProCamera2D>();

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다.");
            return;
        }
        player = playerObj.transform;

        gameOverCanvas.worldCamera = cam; // 또는 너가 쓰는 proCamera2D 같은 카메라

        StartCoroutine(HandleGameOverSequence());
        proCamera2D.Zoom(targetSize-cam.orthographicSize, duration, EaseType.EaseInOut);
    }


    IEnumerator HandleGameOverSequence()
    {
        Color color = redImg.color;

        float halfDuration = duration / 2;
        float startSize = cam.orthographicSize;
        float time = 0f;

        Time.timeScale = 0.5f;
        while (time < halfDuration)
        {
            time += Time.unscaledDeltaTime; // timeScale 영향을 안 받게
            float easedT = Mathf.SmoothStep(0f, 1f, time / halfDuration);
            float alpha = Mathf.Lerp(0.0f, 0.4f, time / halfDuration);
            float redAlpha = Mathf.Lerp(0f, 0.9f, time / halfDuration);

            redImg.color = new Color(color.r, color.g, color.b, redAlpha);
            blackImg.color = new Color(color.r, color.g, color.b, alpha);
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, time / duration);

            up.anchoredPosition = Vector2.Lerp(upStartPos, upStartPos + new Vector2(0, -130), easedT);
            down.anchoredPosition = Vector2.Lerp(downStartPos, downStartPos + new Vector2(0, 130), easedT);

            yield return null;
        }

        ShowDeadBodySprite();

        time = 0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0.4f, 0.95f, time / halfDuration);
            float redAlpha = Mathf.Lerp(0.9f, 1.0f, time / halfDuration);

            blackImg.color = new Color(color.r, color.g, color.b, alpha);
            redImg.color = new Color(color.r, color.g, color.b, redAlpha);
            yield return null;
        }
        gameOverUI.SetActive(true);
        Time.timeScale = 1f;

    }

    void ShowDeadBodySprite()
    {
        SpriteRenderer playerSr = player.GetComponentInChildren<SpriteRenderer>();
        if (playerSr == null || playerSr.sprite == null) return;

        GameObject deadObj = Instantiate(deadBodyPrefab);
        SpriteRenderer sr = deadObj.GetComponent<SpriteRenderer>();

        sr.sprite = playerSr.sprite;
        sr.flipX = playerSr.flipX;

        sr.transform.position = playerSr.transform.position;
        sr.transform.rotation = player.transform.rotation;

        sr.sortingLayerName = "UI";
        sr.sortingOrder = 20;

        // 처음엔 투명하게
        sr.color = new Color(1f, 1f, 1f, 0f);

        // 페이드인 시작
        StartCoroutine(FadeInSprite(sr, 0.8f)); // duration 0.8초 정도
    }

    IEnumerator FadeInSprite(SpriteRenderer sr, float duration)
    {
        float time = 0f;
        Color startColor = sr.color;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0f, 1f, time / duration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // 마지막 값 보정
        sr.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
    }
}
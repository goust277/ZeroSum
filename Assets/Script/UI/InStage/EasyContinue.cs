using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyContinue : MonoBehaviour
{
    [SerializeField] private ProCamera2D proCamera2D;
    [SerializeField] private float blockingDuration = 5.0f;
    public Image fadeCanvas;
    public float fadeDuration = 1f;
    [SerializeField] private GameObject gameOverUI;

    private GameObject playerObj;
    private bool isUsed = false;
    private Animator animator;

    private void Start()
    {
        playerObj = GameObject.Find("Player");
        animator = GameObject.Find("Sprite").GetComponent<Animator>();

        if (proCamera2D == null)
        {
            proCamera2D = Camera.main.GetComponent<ProCamera2D>();
            if (proCamera2D == null)
            {
                Debug.LogWarning("Main Camera에 ProCamera2D가 없습니다!");
            }
        }

    }

    public void OnClickContinue()
    {
        if (isUsed) return;
        isUsed = true;

        Ver01_DungeonStatManager.Instance.ResetDungeonState();
        StartCoroutine(FadeOutandIn());
    }

    private IEnumerator FadeOutandIn()
    {
        Debug.Log("FadeOutandIn 실행중");
        float timer = 0f;
        Time.timeScale = 1; // 게임 재개
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alphaValue = timer / fadeDuration;

            // 검은 화면 점점 어두워지게 설정
            fadeCanvas.color = new Color(fadeCanvas.color.r, fadeCanvas.color.g, fadeCanvas.color.b, alphaValue);
            yield return null;
        }
        gameOverUI.SetActive(false);
        timer = 0f;
        GameStateManager.Instance.StartMoveUIDown();
        if (playerObj != null)
        {
            PlayerHP playerHP = playerObj.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                Debug.Log("FadeOutandIn - playerHP 찾았음 !! 실행중");
                playerHP.ContinueProcessing(blockingDuration);
            }
        }


        float startZoom = Camera.main.orthographicSize;
        proCamera2D.Zoom(6.7f - startZoom, 1.0f);

        yield return new WaitForSeconds(1.0f);
        animator.SetBool("Dead", false);
        animator.Play("Idle");

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alphaValue = fadeDuration / timer;

            // 검은 화면 점점 어두워지게 설정
            fadeCanvas.color = new Color(fadeCanvas.color.r, fadeCanvas.color.g, fadeCanvas.color.b, alphaValue);
            yield return null;
        }
        Destroy(gameObject, 1.0f);
    }
}

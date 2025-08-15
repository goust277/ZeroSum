using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EasyContinue : MonoBehaviour
{
    [SerializeField] private ProCamera2D proCamera2D;
    [SerializeField] private float blockingDuration = 5.0f;

    [Header("Camera Resource")]
    [SerializeField] private float zoomSize = 2.0f;
    [SerializeField] private float zoomDuration = 1.0f;

    [Header("BackGround Resource")]
    [SerializeField] private Image redImg;
    [SerializeField] private Image blackImg;

    [Header("CountDown Resource")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI countTimeText;

    [Header("ResurrectingNPC")]
    public GameObject npcObj;

    private GameObject playerObj;
    private Camera cam;

    private void Start()
    {
        playerObj = GameObject.Find("Player");
        npcObj = GameObject.Find("NPC");

        cam = Camera.main;
        if (proCamera2D == null && cam != null)
        {
            proCamera2D = cam.GetComponent<ProCamera2D>();
            if (proCamera2D == null)
                Debug.LogError("Main Camera에 ProCamera2D가 없습니다!");
        }

        if (npcObj == null)
            Debug.LogError("NPC 오브젝트 못찾음");

        StartCoroutine(FadeOutandIn());
    }

    private IEnumerator FadeOutandIn()
    {
        // 1. 줌인 + 어두워짐
        yield return StartCoroutine(HandleGameOverSequence());

        // 유지 + 텍스트 출력
        yield return StartCoroutine(HandleCountdownWhileDark());

        // NPC 상태 초기화
        ResettingStats();

        // 밝아짐 + 줌아웃
        yield return StartCoroutine(HandleFadeIn());

        if (playerObj != null)
        {
            PlayerHP playerHP = playerObj.GetComponent<PlayerHP>();
            if (playerHP != null)
                playerHP.ContinueProcessing(blockingDuration);
        }

        Destroy(gameObject, 1.0f);
    }

    private IEnumerator HandleGameOverSequence()
    {
        Time.timeScale = 0.5f;
        proCamera2D.Zoom(-zoomSize, zoomDuration); // 줌인

        float time = 0f;
        float fadeDuration = 2.0f;
        Color color = redImg.color;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0.0f, 0.8f, time / fadeDuration);

            redImg.color = new Color(color.r, color.g, color.b, alpha);
            blackImg.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        redImg.color = blackImg.color = new Color(color.r, color.g, color.b, 0.8f);
    }

    private IEnumerator HandleCountdownWhileDark()
    {
        float waitTime = 3f;
        float timer = waitTime;

        while (timer > 0f)
        {
            countTimeText.text = $"{Mathf.CeilToInt(timer)}초 후 자동 부활합니다...";
            yield return new WaitForSecondsRealtime(1f);
            timer -= 1f;
        }

        gameOverText.text = "";
        countTimeText.text = "";
    }

    private IEnumerator HandleFadeIn()
    {
        Time.timeScale = 1.0f;
        proCamera2D.Zoom(zoomSize, zoomDuration); // 줌아웃

        float timer = 0f;
        float fadeInDuration = 1.0f;
        Color color = redImg.color;

        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0.8f, 0f, timer / fadeInDuration);

            redImg.color = new Color(color.r, color.g, color.b, alpha);
            blackImg.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        redImg.color = blackImg.color = new Color(color.r, color.g, color.b, 0f);
    }

    private void ResettingStats()
    {
        //던전 스탯 초기화
        if (Ver01_DungeonStatManager.Instance != null)
            Ver01_DungeonStatManager.Instance.ResetDungeonState();

        //이 밑은 엔피씨 설정 초기화 (1스테이지에서는 바로 리턴해버리게)
        if (npcObj == null) return;

        NPC_Hp nPC_Hp = npcObj.GetComponent<NPC_Hp>();
        NPCController npcController = npcObj.GetComponent<NPCController>();

        if (npcController != null)
            npcController.animator.Play("NPC_Idle");

        if (nPC_Hp != null)
        {
            nPC_Hp.isDead = false;
            nPC_Hp.Damage(0);
            nPC_Hp.SetHp(5);
        }
    }
}

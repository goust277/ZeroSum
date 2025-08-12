using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EasyContinue : MonoBehaviour
{
    [SerializeField] private ProCamera2D proCamera2D;
    [SerializeField] private float blockingDuration = 5.0f;

    [Header("Camera Resource")]
    [SerializeField] private readonly float zoomSize = 2.0f;
    [SerializeField] private readonly float duration = 2.0f;

    [Header("BackGround Resource")]
    [SerializeField] Image redImg;
    [SerializeField] Image blackImg;

    [Header("ResurrectingNPC")]
    public GameObject npcObj;

    private GameObject playerObj;
    private Camera cam;

    private void Start()
    {
        playerObj = GameObject.Find("Player");

        cam = Camera.main;
        if (proCamera2D == null)
        {
            proCamera2D = cam.GetComponent<ProCamera2D>();
            if (proCamera2D == null)
            {
                Debug.LogError("Main Camera에 ProCamera2D가 없습니다!");
            }
        }

        npcObj = GameObject.Find("NPC");

        if (npcObj == null)
        {
            Debug.LogError("NPC 오브젝트 못찾음");
        }

        StartCoroutine(FadeOutandIn());
    }

    private void ResettingStats()
    {
        NPC_Hp nPC_Hp = npcObj.GetComponent<NPC_Hp>();

        npcObj.GetComponent<NPCController>().animator.Play("NPC_Idle");

        nPC_Hp.isDead = false;
        nPC_Hp.Damage(0);
        nPC_Hp.SetHp(5);
        Ver01_DungeonStatManager.Instance.ResetDungeonState();

    }

    private IEnumerator FadeOutandIn()
    {

        yield return StartCoroutine(HandleGameOverSequence()); // 느려지면서 줌인 + 알파
        
        yield return new WaitForSecondsRealtime(0.3f);

        //Time.timeScale = 1; // 게임 재개
        
        float timer = 0f;
        ResettingStats();
        Color color = redImg.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // 알파값을 0.8에서 0으로 점차적으로 감소
            float alphaValue = Mathf.Lerp(0.8f, 0f, timer / duration);

            // 검은 화면 점점 밝게 설정
            redImg.color = new Color(color.r, color.g, color.b, alphaValue);
            blackImg.color = new Color(color.r, color.g, color.b, alphaValue);
            yield return null;
        }
        proCamera2D.Zoom(zoomSize, 1.0f);

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

        float time = 0f;

        Time.timeScale = 0.5f;

        proCamera2D.Zoom(zoomSize * -1.0f, 1.0f);

        while (time < duration)
        {
            time += Time.unscaledDeltaTime; // timeScale 영향을 안 받게
            float alpha = Mathf.Lerp(0.0f, 0.8f, time / duration);

            redImg.color = new Color(color.r, color.g, color.b, alpha);
            blackImg.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Time.timeScale = 1.0f;
        //GameStateManager.Instance.StartMoveUIUp();
        //Time.timeScale = 0.0f;
    }

}

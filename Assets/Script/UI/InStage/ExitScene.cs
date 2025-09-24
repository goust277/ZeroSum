using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ExitScene : MonoBehaviour
{
    [Header("Pre-work Resource")]
    [SerializeField] private Collider2D elevatorCd;
    [SerializeField] private GameObject targetObject;        // On/Off 대상

    [Header("Cut Secene Resource")]
    [SerializeField] private Transform targetPoint;          // 이동할 지점
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ProCamera2D proCamera2D;

    [SerializeField] private bool isPlayerInside = false;
    private bool collided = false;
    private Coroutine moveCoroutine;
    private GameObject inputManager;
    private Transform playerTransform;
    private bool canActivate = false; // F 키 입력 활성화 여부
    private GameObject player;


    [Header("FadeOut Resource")]
    [SerializeField] private Image FadeOutObj;
    readonly float fadeTime = 3;  //페이드아웃이 진행될 시간
    float currentTime = 0;
    [SerializeField] private string nextScene = "Lobby";


    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        //elevatorCd.enabled = false; 이미꺼져있음
    }

    private void Update()
    {
        if (canActivate && Input.GetKeyDown(KeyCode.F) && moveCoroutine == null)
        {
            StartAutoMove();
            canActivate = false; // 중복 방지
        }
    }

    //캔버스 on
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            isPlayerInside = false;
            return;
        }

        if (other.CompareTag("Player") && !isPlayerInside)
        {
            isPlayerInside = true;
            targetObject.SetActive(true);
            canActivate = true;
        }
    }

    //캔버스 off
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isPlayerInside)
        {
            canActivate = false;
            isPlayerInside = false;
            targetObject.SetActive(false);
        }
    }

    //이동시작
    private void StartAutoMove()
    {
        if (moveCoroutine == null && playerTransform != null)
        {
            moveCoroutine = StartCoroutine(MovePlayer());
            StartCoroutine(fadeOut());
        }
    }

    // --- [추가] 타겟을 바라보도록 Y 회전 세팅 (오른쪽=0°, 왼쪽=–180°)
    private void FaceTowards(Vector3 targetPos)
    {
        if (playerTransform == null) return;

        float dx = targetPos.x - playerTransform.position.x;
        if (Mathf.Abs(dx) < 0.001f) return; // 거의 같은 x면 유지

        float targetY = (dx >= 0f) ? 0f : -180f; // 프로젝트 규칙
        Vector3 e = playerTransform.eulerAngles;
        playerTransform.rotation = Quaternion.Euler(e.x, targetY, e.z);
    }


    //카메라 줌 + 플레이어가 걸어간다.
    private IEnumerator MovePlayer()
    {
        proCamera2D.Zoom(-1.0f, 1.0f);

        float duration = 1f;
        float elapsedTime = 0f;

        if (inputManager != null)
            inputManager.SetActive(false);

        // 카메라 멈춤
        if (proCamera2D != null)
            proCamera2D.RemoveAllCameraTargets();

        
        collided = false;
        player.GetComponent<PlayerAnimation>().enabled = collided;
        playerAnimator.SetBool("Move", true);

        // 타겟 바라보게 만들기
        FaceTowards(targetPoint.position);

        while (!collided && elapsedTime < duration)
        {
            Vector3 dir = (targetPoint.position - playerTransform.position).normalized;
            playerTransform.position += dir * moveSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        playerAnimator.SetBool("Move", false);
        playerAnimator.Play("Idle");

        elevatorCd.enabled = true;
        moveCoroutine = null;
    }


    //점점 어두워지기
    IEnumerator fadeOut()
    {
        FadeOutObj.gameObject.SetActive(true);

        Color alpha = FadeOutObj.color;
        alpha.a = 0f;
        FadeOutObj.color = alpha;

        while (alpha.a < 1)
        {
            currentTime += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(0, 1, currentTime);
            FadeOutObj.color = alpha;
            yield return null;
        }
        SceneManager.LoadScene(nextScene);
    }
}

using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ExitScene : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;        // On/Off ���
    [SerializeField] private Transform targetPoint;          // �̵��� ����
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ProCamera2D proCamera2D;

    private bool isPlayerInside = false;
    private bool collided = false;
    private Coroutine moveCoroutine;
    private GameObject inputManager;
    private Transform playerTransform;
    private bool canActivate = false; // F Ű �Է� Ȱ��ȭ ����
    private GameObject player;


    [Header("FadeOut Resource")]
    [SerializeField] private Image FadeOutObj;
    float fadeTime = 3;  //���̵�ƿ��� ����� �ð�
    float currentTime = 0;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (canActivate && Input.GetKeyDown(KeyCode.F) && moveCoroutine == null)
        {
            StartAutoMove();
            canActivate = false; // �ߺ� ����
        }
    }

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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isPlayerInside)
        {
            isPlayerInside = false;
            targetObject.SetActive(false);
        }
    }

    private void StartAutoMove()
    {
        if (moveCoroutine == null && playerTransform != null)
        {
            moveCoroutine = StartCoroutine(MovePlayer());
            StartCoroutine(fadeOut());
        }
    }

    private IEnumerator MovePlayer()
    {
        proCamera2D.Zoom(2.0f, 1.0f);

        float duration = 1f;
        float elapsedTime = 0f;

        if (inputManager != null)
            inputManager.SetActive(false);

        // ī�޶� ����
        if (proCamera2D != null)
            proCamera2D.RemoveAllCameraTargets();

        
        collided = false;
        player.GetComponent<PlayerAnimation>().enabled = collided;
        playerAnimator.SetBool("Move", true);

        while (!collided && elapsedTime < duration)
        {
            Vector3 dir = (targetPoint.position - playerTransform.position).normalized;
            playerTransform.position += dir * moveSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        playerAnimator.SetBool("Move", false);
        playerAnimator.Play("Idle");

        moveCoroutine = null;
    }


    IEnumerator fadeOut()
    {
        FadeOutObj.gameObject.SetActive(true);
        Color alpha = FadeOutObj.color;
        while (alpha.a < 1)
        {
            currentTime += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(0, 1, currentTime);
            FadeOutObj.color = alpha;
            yield return null;
        }
        SceneManager.LoadScene("Lobby");
    }
}

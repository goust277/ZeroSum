using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;


public class ExitScene : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;        // On/Off 대상
    [SerializeField] private Transform targetPoint;          // 이동할 지점
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ProCamera2D proCamera2D;

    private bool isPlayerInside = false;
    private bool collided = false;
    private Coroutine moveCoroutine;
    private GameObject inputManager;
    private Transform playerTransform;
    private bool canActivate = false; // F 키 입력 활성화 여부
    private GameObject player;

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
            canActivate = false; // 중복 방지
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlayerInside)
        {
            isPlayerInside = true;
            targetObject.SetActive(true);
            canActivate = true;
        }
        else if (other.CompareTag("MovingBlock"))
        {
            collided = true;
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
        }
    }

    private IEnumerator MovePlayer()
    {
        float duration = 1f;
        float elapsedTime = 0f;

        if (inputManager != null)
            inputManager.SetActive(false);

        // 카메라 멈춤
        if (proCamera2D != null)
            proCamera2D.RemoveAllCameraTargets();

        
        collided = false;
        player.GetComponent<PlayerAnimation>().enabled = collided;
        playerAnimator.Play("Walk");

        while (!collided && elapsedTime < duration)
        {
            Vector3 dir = (targetPoint.position - playerTransform.position).normalized;
            playerTransform.position += dir * moveSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        playerAnimator.Play("Idle");

        moveCoroutine = null;
    }
}

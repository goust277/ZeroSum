using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Playables;
using Com.LuisPedroFonseca.ProCamera2D;
using Unity.VisualScripting;

public class TutorialSkipper : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;

    [Header("ConnectPause")]
    [SerializeField] private Pause ver01_Pause;  // TogglePause 들어 있는 외부 오브젝트
    [SerializeField] private GameObject inputManager;
    [SerializeField] private PlayerInput playerInput;

    [Header("EndCutScene")]
    [SerializeField] private GameObject skipUI;
    [SerializeField] private GameObject TutorialObj;
    [SerializeField] private Stage1_Num4 stage1_Num4;
    [SerializeField] private PlayerHP playerHP;
    [SerializeField] private GameObject tutorialMission;

    [Header("UI")]
    [SerializeField] private GameObject MissionUI;
    [SerializeField] private GameObject up;
    [SerializeField] private ProCamera2D proCamera2D;

    private bool isTutorialPhase = true;
    private bool isAwaitingSkipConfirm = false;
    private float originOrthographic = 6.7f;

    void Start()
    {
        GameStateManager.Instance.StartMoveUIUp();
        playerHP.isBlocked = true;
        if (GameStateManager.Instance.GetCurrentSceneEnterCount() > 1)
        {
            isTutorialPhase = false;
            StartCoroutine(ConfirmSkip());
        }
    }

    void Update()
    {
        if (!isTutorialPhase) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!isAwaitingSkipConfirm)
            {
                ShowSkipPopup();
            }
            else
            {
                CancelSkipPopup();
            }
        }

        if (isAwaitingSkipConfirm && Keyboard.current.fKey.wasPressedThisFrame)
        {
            StartCoroutine(ConfirmSkip());
        }
    }

    void ShowSkipPopup()
    {
        skipUI.SetActive(true);
        Time.timeScale = 0f; // 시간 정지 (애니메이션 멈춤)
        isAwaitingSkipConfirm = true;
    }

    void CancelSkipPopup()
    {
        skipUI.SetActive(false);
        Time.timeScale = 1f;
        isAwaitingSkipConfirm = false;
    }

    IEnumerator ConfirmSkip()
    {
        yield return StartCoroutine(MoveUIVerticallyDown());
        GameStateManager.Instance.StartMoveUIDown();

        EndCutScene();
        ConnectPause();
        skipUI.SetActive(false);
        tutorialMission.SetActive(false);
        Time.timeScale = 1f;

        
        isTutorialPhase = false;
        isAwaitingSkipConfirm = false;

        gameObject.SetActive(false);  // 코루틴 끝난 후 끄기
    }


    private void OnDisable()
    {
        playerHP.isBlocked = false;
    }

    public void ConnectPause()
    {
        // TogglePause()를 ESC에 연결
        var escAction = playerInput.actions["ESC"];
        escAction.Disable(); // 내부적으로 콜백 유지 안 함
        escAction.Enable();  // 재등록 필요

        playerInput.actions["ESC"].performed += ver01_Pause.OnESC;
    }

    private IEnumerator MoveUIVerticallyDown()
    {
        RectTransform target = MissionUI.transform as RectTransform;
        if (target.anchoredPosition.y > 600f)
        { 
            Vector2 startPos = target.anchoredPosition;
            Vector2 endPos = startPos + new Vector2(0f, -180f);
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

    private void EndCutScene()
    {
        float startZoom = Camera.main.orthographicSize;

        stage1_Num4.SwapMissionUI();
        stage1_Num4.MonsterEnable();
        stage1_Num4.ev.enabled = true;
        playerTarget.GetComponent<PlayerAnimation>().enabled = true;
        TutorialObj.SetActive(false);
        

        proCamera2D.RemoveAllCameraTargets();
        MoveAndZoomTo(new Vector2(playerTarget.position.x, playerTarget.position.y), originOrthographic, 2.0f);
        proCamera2D.AddCameraTarget(playerTarget, 1f, 1f, 0f, new Vector2(0f, 2f));

        //player.GetComponent<PlayerAnimation>().enabled = true;
        up.SetActive(false);
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
}

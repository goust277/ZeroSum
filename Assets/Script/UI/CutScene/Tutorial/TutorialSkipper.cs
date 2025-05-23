using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Playables;
using Com.LuisPedroFonseca.ProCamera2D;

public class TutorialSkipper : MonoBehaviour
{
    [SerializeField] private GameObject skipUI;
    [SerializeField] private Pause ver01_Pause;  // TogglePause 들어 있는 외부 오브젝트
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private BeforeEvUp beforeEvUp;
    [SerializeField] private GameObject TutorialObj;


    [Header("UI")]
    [SerializeField] private GameObject MissionUI;
    [SerializeField] private ProCamera2D proCamera2D;

    private bool isTutorialPhase = true;
    private bool isAwaitingSkipConfirm = false;

    void Start()
    {
        GameStateManager.Instance.StartMoveUIUp();

        if (GameStateManager.Instance.GetCurrentSceneEnterCount() > 1)
        {
            isTutorialPhase = false;
            ConfirmSkip();
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
            ConfirmSkip();
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

    void ConfirmSkip()
    {
        StartCoroutine(RunFadeThenDeactivate());
        StartCoroutine(MoveUIVerticallyDown());
        GameStateManager.Instance.StartMoveUIDown();

        foreach (PlayableDirector director in FindObjectsOfType<PlayableDirector>())
        {
            if (director.state == PlayState.Playing)
            {
                director.Stop();
            }
        }
        proCamera2D.Zoom(3.2f, 2.0f);

        skipUI.SetActive(false);
        Time.timeScale = 1f;
        isTutorialPhase = false;
        isAwaitingSkipConfirm = false;

        Debug.Log("TutorialSkipper - 튜토리얼 스킵");

        // TogglePause()를 ESC에 연결
        var escAction = playerInput.actions["ESC"];
        escAction.Disable(); // 내부적으로 콜백 유지 안 함
        escAction.Enable();  // 재등록 필요

        playerInput.actions["ESC"].performed += ver01_Pause.OnESC;
    }

    private IEnumerator MoveUIVerticallyDown()
    {
        RectTransform target = MissionUI.transform as RectTransform;

        Vector2 startPos = target.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0f, -200f);
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

    private IEnumerator MoveUIVerticallyUp()
    {
        RectTransform target = MissionUI.transform as RectTransform;

        Vector2 startPos = target.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0f, +200f);
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


    IEnumerator RunFadeThenDeactivate()
    {
        yield return StartCoroutine(beforeEvUp.GrowAndFade());
        beforeEvUp.ev.enabled = true;

        TutorialObj.SetActive(false);
        gameObject.SetActive(false);  // 코루틴 끝난 후 끄기
    }

}

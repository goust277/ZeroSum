using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string line;
    public DialogueLine(string speaker, string line) { this.speaker = speaker; this.line = line; }
}

public class IntroController : MonoBehaviour
{
    // ------------------- Inspector -------------------
    [Header("Resources Img")]
    [SerializeField] private Animator animator;
    [SerializeField] private Image Panel;

    [Header("Index")]
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private int currentLineIndex = 0;

    [Header("Status Check")]
    [SerializeField] private bool isTyping = false;
    [SerializeField] private bool typingSkipped = false;
    [SerializeField] private bool isWaitingForNextLine = false;
    [SerializeField] private bool isFKeyBuffered = false;

    [Header("Resources Text")]
    [SerializeField] protected TextMeshProUGUI nameTXT;
    [SerializeField] protected TextMeshProUGUI desTXT;
    [SerializeField] protected TextMeshProUGUI nextText;

    [Header("Effects")]
    [SerializeField] private float typewriterDelay = 0.05f;
    [SerializeField] private float blinkInterval = 0.5f;
    [SerializeField] private float fadeoutTime = 2.0f;

    // ------------------- Runtime -------------------
    private Coroutine typingCoroutine;
    private Coroutine blinkCoroutine;
    private float currentTime = 0.0f;


    // ------------------- Data -------------------
    private readonly string[] npcNames = { "사회자로 보이는 로봇", "전문가로 보이는 로봇", "정체불명의 남성", "전광판에서 들려오는 목소리", "삭" };
    private readonly int[] num = {
        0, 1,        // 2
        1, 1,        // 3 
        0, 1,        // 4
        1, 0, 0,     // 5
        2, 2, 2,     // 6
        3, 3,        // 7
        4, 4, 4      // 8 
    };
    private readonly string[] dialogues = {
        "영화수님의 예언이 처음으로 내려온 게 \n 2146년이었죠.",
        "네, 그 예언을 계기로 저희는 \n 살아남을 길을 찾아내기 시작했습니다.",
        "비로소 영화수님은 저희의 중심이 되셨죠.",
        "영화수님이 계시는 한, 도시는 건재합니다.",
        "하지만, 영화수님에게 문제가 생기면 어떡하죠?",
        "오, 상당히 위험한 말씀을 하시는군요.",
        "걱정하지 않아도 괜찮습니다. \n 모든 만일을 위해 저희 피안이 있는 거니까요.",
        "휴! \n 실은 모든 분이 알고 계시겠지만, \n 다시 한번 여쭤봤습니다.",
        "그게 제 일이니까요! 하하.",
        "그래, 그렇게만 하라고…",
        "진짜 세상이 다가오면…",
        "너도 날 이해할 수밖에 없겠지.",
        "네, 지금까지 피안에서 모신 박사님이셨습니다.",
        "쇼는 다음 이 시간에도...",
        "...",
        "당신은 뭘 바라보고 있는거지?",
        "아버지..."
    };
    private readonly int[] linesPerClip = {
        0, 0,       // Intro_0 ~ Intro_1
        2, 2, 2,    // Intro_2 ~ Intro_4
        3, 3, 2,    // Intro_5 ~ Intro_7
        3           // Intro_8
    };

    private readonly List<List<DialogueLine>> groupedDialogues = new List<List<DialogueLine>>();

    // ------------------- Convenience -------------------
    private List<DialogueLine> CurrentLines => groupedDialogues[currentIndex];
    private bool HasLines => CurrentLines.Count > 0;
    private bool ClipFinished => currentLineIndex >= CurrentLines.Count;

    // ===================================================
    //                       Unity
    // ===================================================
    void Start()
    {
        BuildGroupedDialogues();
        ClearNextText(); // 시작 시 비활성화
        PlayCurrent();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFKeyBuffered) return;
            isFKeyBuffered = true;
            HandleFPressed();
            StartCoroutine(ReleaseFBufferNextFrame());  // 다음 프레임에 자동 해제(키업 누락 대비)
        }

        if (Input.GetKeyUp(KeyCode.F))
            isFKeyBuffered = false;
    }

    // F키 누르면 판별
    private void HandleFPressed()
    {
        // 1) 타이핑 중이면 즉시 스킵
        if (isTyping)
        {
            typingSkipped = true;
            return;
        }

        var lines = CurrentLines;

        // 2) 줄 대기 상태
        if (isWaitingForNextLine)
        {
            // 마지막 줄이면 즉시 다음 클립
            if (currentLineIndex + 1 >= lines.Count)
            {
                AdvanceClip();
            }
            else
            {
                currentLineIndex++;
                StartNextLine();
            }
            return;
        }

        // 3) 대사 없는 클립이면 바로 다음 클립
        if (lines.Count == 0)
        {
            AdvanceClip();
            return;
        }

        // 4) 그 외 -> 첫 줄 시작
        StartNextLine();
    }

    private void PlayCurrent()
    {
        animator.Play($"Intro_{currentIndex + 1}");
        currentLineIndex = 0;

        if (!HasLines)
        {
            isTyping = false;
            isWaitingForNextLine = false;
            SetNextTextReady(); // 빈 클립에서도 "다음 [F]" 유지
            return;
        }

        StartNextLine();
    }

    private void AdvanceClip()
    {
        currentIndex++;
        if (currentIndex >= groupedDialogues.Count)
        {
            StartCoroutine(FadeOut());
            return;
        }
        PlayCurrent();
    }

    private void StartNextLine()
    {
        var lines = CurrentLines;

        if (ClipFinished)
        {
            isTyping = false;
            isWaitingForNextLine = false;
            SetNextTextReady(); // "다음 [F]" 유지 + 깜빡임
            return;
        }

        StopTypingCoroutineIfAny();

        DialogueLine lineData = lines[currentLineIndex];
        nameTXT.text = lineData.speaker;
        typingCoroutine = StartCoroutine(TypeWriterLine(lineData.line));
    }

    private IEnumerator TypeWriterLine(string line)
    {
        isTyping = true;
        typingSkipped = false;
        isWaitingForNextLine = false;

        SetNextTextTyping(); // "스킵 [F]"
        desTXT.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            if (typingSkipped)
            {
                desTXT.text = line;
                break;
            }
            desTXT.text += line[i];
            yield return new WaitForSeconds(typewriterDelay);
        }

        isTyping = false;
        typingCoroutine = null;
        isWaitingForNextLine = true;

        SetNextTextReady();  // "다음 [F]" + 깜빡임
    }

    private IEnumerator FadeOut()
    {
        ClearNextText(); // 전환 중 라벨 숨김
        Panel.gameObject.SetActive(true);

        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            currentTime += Time.deltaTime / fadeoutTime;
            alpha.a = Mathf.Lerp(0f, 1f, currentTime);
            Panel.color = alpha;
            yield return null;
        }
        SceneManager.LoadScene("Lobby");
    }

    // UI 유틸
    private void SetNextTextTyping()
    {
        StopBlinking();
        if (nextText == null) return;
        nextText.gameObject.SetActive(true);
        var c = nextText.color; c.a = 1f; nextText.color = c;
    }

    private void SetNextTextReady()
    {
        if (nextText == null) return;
        nextText.gameObject.SetActive(true);
        StartBlinking();
    }

    private void ClearNextText()
    {
        StopBlinking();
        if (nextText != null)
            nextText.gameObject.SetActive(false);
    }

    private void StartBlinking()
    {
        StopBlinking();
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    private void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        if (nextText != null)
        {
            var c = nextText.color; c.a = 1f; nextText.color = c;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        var c = nextText.color;
        while (true)
        {
            c.a = 1f; nextText.color = c;
            yield return new WaitForSeconds(blinkInterval);
            c.a = 0f; nextText.color = c;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // Init / Helpers
    private void BuildGroupedDialogues()
    {
        int cursor = 0;
        foreach (int count in linesPerClip)
        {
            var clipLines = new List<DialogueLine>(count);
            for (int i = 0; i < count; i++)
            {
                clipLines.Add(new DialogueLine(npcNames[num[cursor]], dialogues[cursor]));
                cursor++;
            }
            groupedDialogues.Add(clipLines);
        }
    }

    private void StopTypingCoroutineIfAny()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    private IEnumerator ReleaseFBufferNextFrame()
    {
        yield return null; // 다음 프레임
        isFKeyBuffered = false;
    }
}

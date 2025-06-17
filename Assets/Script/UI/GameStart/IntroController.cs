using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string line;

    public DialogueLine(string speaker, string line)
    {
        this.speaker = speaker;
        this.line = line;
    }
}

public class IntroController : MonoBehaviour
{
    [Header("Resources Img")]
    [SerializeField] private Animator animator;
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private int currentLineIndex = 0;

    [Header("Status Check")]
    [SerializeField] private bool isTyping = false;
    [SerializeField] private bool typingSkipped = false;
    [SerializeField] private bool isWaitingForNextLine = false;
    [SerializeField] private bool isFKeyBuffered = false;

    private Coroutine typingCoroutine;

    [SerializeField] private Image Panel;
    private readonly float fadeoutTime = 2.0f;
    float currentTime = 0.0f;

    [Header("Resources Text")]
    [SerializeField] protected TextMeshProUGUI nameTXT;
    [SerializeField] protected TextMeshProUGUI desTXT;
    [SerializeField] protected TextMeshProUGUI nextText;

    private readonly string[] npcNames = { "사회자로 보이는 로봇", "전문가로 보이는 로봇", "정체불명의 남성", "전광판에서 들려오는 목소리", "삭" };
    private readonly int[] num = {
        0, 1,
        1, 1,
        0, 1,
        1, 0, 0,
        2, 2, 2,
        3, 3,
        4, 4, 4
    };
    private readonly string[] dialogues =
    {
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

    private List<List<DialogueLine>> groupedDialogues = new List<List<DialogueLine>>();

    void Start()
    {
        int dialogueCursor = 0;

        foreach (int count in linesPerClip)
        {
            var clipLines = new List<DialogueLine>();
            for (int i = 0; i < count; i++)
            {
                var speaker = npcNames[num[dialogueCursor]];
                var text = dialogues[dialogueCursor];
                clipLines.Add(new DialogueLine(speaker, text));
                dialogueCursor++;
            }
            groupedDialogues.Add(clipLines);
        }

        PlayCurrent();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFKeyBuffered) return; // 연타 방지
            isFKeyBuffered = true;

            var lines = groupedDialogues[currentIndex];

            if (isTyping)
            {
                typingSkipped = true;
            }
            else if (isWaitingForNextLine)
            {
                currentLineIndex++;
                StartNextLine();
            }
            else if (lines.Count == 0)
            {
                currentIndex++;
                if (currentIndex >= groupedDialogues.Count)
                {
                    StartCoroutine(fadeOut());
                    return;
                }
                PlayCurrent();
            }
            else
            {
                currentIndex++;
                if (currentIndex >= groupedDialogues.Count)
                {
                    StartCoroutine(fadeOut());
                    return;
                }
                PlayCurrent();
            }
        }

        // F 키에서 손 뗐을 때만 다시 입력 허용
        if (Input.GetKeyUp(KeyCode.F))
        {
            isFKeyBuffered = false;
        }
    }


    void PlayCurrent()
    {
        animator.Play($"Intro_{currentIndex+1}");
        currentLineIndex = 0;
        var lines = groupedDialogues[currentIndex];

        if (lines.Count == 0)
        {
            isTyping = false;
            isWaitingForNextLine = false;
            return;
        }

        StartNextLine();
    }

    void StartNextLine()
    {
        var lines = groupedDialogues[currentIndex];

        if (currentLineIndex >= lines.Count)
        {
            isTyping = false;
            isWaitingForNextLine = false;
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        DialogueLine lineData = lines[currentLineIndex];
        nameTXT.text = lineData.speaker;
        typingCoroutine = StartCoroutine(TypeWriterLine(lineData.line));
    }

    private IEnumerator TypeWriterLine(string line)
    {
        isTyping = true;
        typingSkipped = false;
        isWaitingForNextLine = false;
        desTXT.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            if (typingSkipped)
            {
                desTXT.text = line;
                break;
            }
            desTXT.text += line[i];
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
        typingCoroutine = null;
        isWaitingForNextLine = true;
    }

    private IEnumerator fadeOut()
    {
        Panel.gameObject.SetActive(true);
        Color alpha = Panel.color;
        while (alpha.a < 1)
        {
            currentTime += Time.deltaTime / fadeoutTime;
            alpha.a = Mathf.Lerp(0, 1, currentTime);
            Panel.color = alpha;
            yield return null;
        }
        SceneManager.LoadScene("Lobby");
    }
}

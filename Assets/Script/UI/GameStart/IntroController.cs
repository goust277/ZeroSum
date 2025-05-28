using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class IntroController : MonoBehaviour
{

    [Header("Resources Img")]
    [SerializeField] private Sprite[] introImgs;
    [SerializeField] private Image targetImage;
    [SerializeField] private Image Panel;
    private readonly float fadeoutTime = 2.0f;
    float currentTime = 0.0f;

    [Header("Resources Text")]
    [SerializeField] protected TextMeshProUGUI nameTXT;
    [SerializeField] protected TextMeshProUGUI desTXT;
    [SerializeField] protected TextMeshProUGUI nextText;

    private readonly string[] npcNames = { "사회자로 보이는 로봇", "전문가로 보이는 로봇","" };
    private readonly int[] num = { 0, 1, 1, 0, 1, 0,2,2 };
    private readonly string[] logs =
    {
        "서기 2214년이었나요? \n 세계수님의 예언이 내려오기 시작한지요.",
        "네, 그 예언을 계기로 \n 저희는 살아남을 길을 찾아내기 시작했습니다.",
        "영화수님이 계시는 한, \n 도시는 건재합니다.",
        "영화수님에게 문제가생기면 어떡하죠?",
        "걱정하지 않아도 괜찮습니다. \n 모든 만일을 위해 저희 피안이 있는거니까요.",
        "든든합니다! \n 그럼 지금까지, 피안사의 창립 30주년을 기념으로…",
        "",
        "...아버지"
    };

    private string line = "";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IntroCoroutine());
    }

    public void ChangeImagesToIndex(int index)
    {
        if (index < 0 || index >= introImgs.Length)
        {
            Debug.LogWarning("인덱스 범위 초과");
            return;
        }

        targetImage.sprite = introImgs[index];
    }

    private IEnumerator TypeWriterLine(string line)
    {
        desTXT.text = "";  // 매번 출력 전에 텍스트 초기화
        for (int index = 0; index < line.Length; index++)
        {
            desTXT.text += line[index].ToString();
            yield return new WaitForSeconds(0.02f); // 글자 한 글자씩 출력
        }
    }

    IEnumerator IntroCoroutine()
    {
        ChangeImagesToIndex(0);
        nextText.enabled = true;
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;  // 매 프레임 기다리며 체크
        }
        nextText.enabled = false;

        ChangeImagesToIndex(1);

        nextText.enabled = true;
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;  // 매 프레임 기다리며 체크
        }
        nextText.enabled = false;

        ChangeImagesToIndex(2);
        yield return TypeWriter(0);
        yield return TypeWriter(1);

        ChangeImagesToIndex(3);
        yield return TypeWriter(2);

        ChangeImagesToIndex(4);
        yield return TypeWriter(3);
        yield return TypeWriter(4);

        ChangeImagesToIndex(5);
        yield return TypeWriter(5);

        ChangeImagesToIndex(6);
        yield return TypeWriter(6);

        ChangeImagesToIndex(7);
        nextText.enabled = true;
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;  // 매 프레임 기다리며 체크
        }
        nextText.enabled = false;
        yield return TypeWriter(7);

        StartCoroutine(fadeOut());
    }

    IEnumerator TypeWriter(int index)
    {
        nameTXT.text = npcNames[num[index]];
        line = logs[index];

        desTXT.text = "";
        yield return StartCoroutine(TypeWriterLine(line));// 대사 출력 코루틴

        nextText.enabled = true;
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;  // 매 프레임 기다리며 체크
        }
        nextText.enabled = false;
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

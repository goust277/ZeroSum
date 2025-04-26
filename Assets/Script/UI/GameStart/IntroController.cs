using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    [Header("Resources Audio")]
    [SerializeField] private AudioClip radioOn;
    [SerializeField] private AudioClip radioOff;

    [Header("Resources Img")]
    [SerializeField] private Transform[] movingSections;
    [SerializeField] private float duration;
    [SerializeField] private Image cityImg;

    [Header("Resources Text")]
    [SerializeField] protected TextMeshProUGUI nameTXT;
    [SerializeField] protected TextMeshProUGUI desTXT;

    private readonly string[] npcNames = { "MC의 목소리", "전문가로 들리는 목소리" };
    private readonly int[] num = { 0, 1, 1, 0, 1, 0 };
    private readonly string[] logs =
    {
        "서기 2214년이었나요? 세계수님의 예언이 내려오기 시작한지요.",
        "네, 그 예언을 계기로 저희는 살아남을 길을 찾아내기 시작했죠.",
        "세계수님이 건재하는 한, 저희 도시는 영원합니다.",
        "mc의 목소리 - 세계수님에게 문제가 생기면 어떡하죠?",
        "하하, 걱정하지 않아도 괜찮습니다. 그 문제를 위해 저희 피안이 있는거니까요.",
        "mc의 목소리 - 든든합니다! 그럼 지금까지, 피안사의 창립 30주년을 기념으로-"
    };

    private Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        currentCoroutine = StartCoroutine(MoveImageCoroutine());
    }

    private IEnumerator MoveImageCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 startPos = movingSections[0].position;
        Vector3 endPos = movingSections[1].position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            cityImg.gameObject.GetComponent<RectTransform>().position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        cityImg.gameObject.GetComponent<RectTransform>().position = endPos; // 최종 위치 보정

    }
    private IEnumerator TypeWriterLine(string line)
    {
        desTXT.text = "";  // 매번 출력 전에 텍스트 초기화
        for (int index = 0; index < line.Length; index++)
        {
            desTXT.text += line[index].ToString();
            yield return new WaitForSeconds(0.05f); // 글자 한 글자씩 출력
        }
    }

    //IEnumerator TypeWriter()
    //{
    //    int i = 0;
    //    bool setNextDialog = false;

    //    for(i =0; i< logs.Length; i++)
    //    {
    //        nameTXT.text = npcNames[num[i]];

    //        desTXT.text = "";
    //        bool isSkipLine = false;
    //        dialogCoroutine = StartCoroutine(TypeWriterLine(line));// 대사 출력 코루틴

    //        //중간에 대사 스킵되도록 처리
    //        while (!isSkipLine)
    //        {
    //            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
    //            {
    //                isSkipLine = true;
    //                StopCoroutine(dialogCoroutine); // 중지
    //                desTXT.text = line; // 스킵 눌렀을 때 줄 전체 출력
    //            }
    //            yield return null; // 기다림
    //        }

    //        // 대사 출력 후, 다음 대사로 넘어가기 위한 입력 대기
    //        while (!setNextDialog)
    //        {
    //            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
    //            {
    //                setNextDialog = true;
    //            }
    //            yield return null;
    //        }
    //        setNextDialog = false; // 한 번만 작동하도록 초기화

    //        dialogCoroutine = null;

    //        i++;
    //    }

    //    EndConversation();
    //}

}

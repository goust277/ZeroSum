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

    private readonly string[] npcNames = { "MC�� ��Ҹ�", "�������� �鸮�� ��Ҹ�" };
    private readonly int[] num = { 0, 1, 1, 0, 1, 0 };
    private readonly string[] logs =
    {
        "���� 2214���̾�����? ��������� ������ �������� ����������.",
        "��, �� ������ ���� ����� ��Ƴ��� ���� ã�Ƴ��� ��������.",
        "��������� �����ϴ� ��, ���� ���ô� �����մϴ�.",
        "mc�� ��Ҹ� - ������Կ��� ������ ����� �����?",
        "����, �������� �ʾƵ� �������ϴ�. �� ������ ���� ���� �Ǿ��� �ִ°Ŵϱ��.",
        "mc�� ��Ҹ� - ����մϴ�! �׷� ���ݱ���, �ǾȻ��� â�� 30�ֳ��� �������-"
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

        cityImg.gameObject.GetComponent<RectTransform>().position = endPos; // ���� ��ġ ����

    }
    private IEnumerator TypeWriterLine(string line)
    {
        desTXT.text = "";  // �Ź� ��� ���� �ؽ�Ʈ �ʱ�ȭ
        for (int index = 0; index < line.Length; index++)
        {
            desTXT.text += line[index].ToString();
            yield return new WaitForSeconds(0.05f); // ���� �� ���ھ� ���
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
    //        dialogCoroutine = StartCoroutine(TypeWriterLine(line));// ��� ��� �ڷ�ƾ

    //        //�߰��� ��� ��ŵ�ǵ��� ó��
    //        while (!isSkipLine)
    //        {
    //            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
    //            {
    //                isSkipLine = true;
    //                StopCoroutine(dialogCoroutine); // ����
    //                desTXT.text = line; // ��ŵ ������ �� �� ��ü ���
    //            }
    //            yield return null; // ��ٸ�
    //        }

    //        // ��� ��� ��, ���� ���� �Ѿ�� ���� �Է� ���
    //        while (!setNextDialog)
    //        {
    //            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
    //            {
    //                setNextDialog = true;
    //            }
    //            yield return null;
    //        }
    //        setNextDialog = false; // �� ���� �۵��ϵ��� �ʱ�ȭ

    //        dialogCoroutine = null;

    //        i++;
    //    }

    //    EndConversation();
    //}

}

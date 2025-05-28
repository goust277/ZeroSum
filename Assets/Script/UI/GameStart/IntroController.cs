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

    private readonly string[] npcNames = { "��ȸ�ڷ� ���̴� �κ�", "�������� ���̴� �κ�","" };
    private readonly int[] num = { 0, 1, 1, 0, 1, 0,2,2 };
    private readonly string[] logs =
    {
        "���� 2214���̾�����? \n ��������� ������ �������� ����������.",
        "��, �� ������ ���� \n ����� ��Ƴ��� ���� ã�Ƴ��� �����߽��ϴ�.",
        "��ȭ������ ��ô� ��, \n ���ô� �����մϴ�.",
        "��ȭ���Կ��� ����������� �����?",
        "�������� �ʾƵ� �������ϴ�. \n ��� ������ ���� ���� �Ǿ��� �ִ°Ŵϱ��.",
        "����մϴ�! \n �׷� ���ݱ���, �ǾȻ��� â�� 30�ֳ��� ������Ρ�",
        "",
        "...�ƹ���"
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
            Debug.LogWarning("�ε��� ���� �ʰ�");
            return;
        }

        targetImage.sprite = introImgs[index];
    }

    private IEnumerator TypeWriterLine(string line)
    {
        desTXT.text = "";  // �Ź� ��� ���� �ؽ�Ʈ �ʱ�ȭ
        for (int index = 0; index < line.Length; index++)
        {
            desTXT.text += line[index].ToString();
            yield return new WaitForSeconds(0.02f); // ���� �� ���ھ� ���
        }
    }

    IEnumerator IntroCoroutine()
    {
        ChangeImagesToIndex(0);
        nextText.enabled = true;
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;  // �� ������ ��ٸ��� üũ
        }
        nextText.enabled = false;

        ChangeImagesToIndex(1);

        nextText.enabled = true;
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;  // �� ������ ��ٸ��� üũ
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
            yield return null;  // �� ������ ��ٸ��� üũ
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
        yield return StartCoroutine(TypeWriterLine(line));// ��� ��� �ڷ�ƾ

        nextText.enabled = true;
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;  // �� ������ ��ٸ��� üũ
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

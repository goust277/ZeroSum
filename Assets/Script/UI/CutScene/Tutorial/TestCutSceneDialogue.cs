using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TestCutScneDialogue : MonoBehaviour
{
    [SerializeField] private Canvas textBox;
    [SerializeField] private TextMeshProUGUI dialogueText;


    [Header("Dialog Resources")]
    [SerializeField] private string[] texts ={};
    [SerializeField] private Vector3 movingOffset = new Vector3(0.5f, 2.3f, 0);

    [Header("CutScene Resources")]
    private RectTransform uiTextTransform;

    [Header("Tracking Resources")]
    [SerializeField] private Transform trackingTransform;

    public void Start()
    {
        uiTextTransform = gameObject.GetComponent<RectTransform>();
        uiTextTransform.position = trackingTransform.position + movingOffset;

        StartCoroutine(CutSceneTextWriter());
    }


    IEnumerator CutSceneTextWriter()
    {
        float time = 0.0f;

        foreach (var dialog in texts)
        {
            dialogueText.text = "";
            string currentText = "";
            bool insideTag = false;
            string tagBuffer = "";

            for (int index = 0; index < dialog.Length; index++)
            {
                char c = dialog[index];

                if (c == '<')  // �±� ����
                {
                    insideTag = true;
                    tagBuffer += c;
                    continue;
                }

                if (insideTag)
                {
                    tagBuffer += c;
                    if (c == '>')  // �±� ��
                    {
                        insideTag = false;
                        currentText += tagBuffer;  // �±� ��°�� �߰�
                        tagBuffer = "";
                    }
                    continue;  // �±� �� ���� ������ ��ٸ�
                }

                currentText += c;
                dialogueText.text = currentText;

                yield return new WaitForSeconds(0.05f);
                time += 0.05f;
            }

            dialogueText.text += "\n";
            // ��� �ð� ����
            float waitTime = 1.0f;
            if (dialog.Length >= 20) waitTime = 3.0f;
            else if (dialog.Length >= 10) waitTime = 2.0f;
            else waitTime = 1.0f;

            // �ش� ��� ��� �� ���
            yield return new WaitForSeconds(waitTime);
            time += waitTime;
        }

        yield return new WaitForSeconds(2.0f);
        time += 2.0f;

        if (textBox != null)
        {
            textBox.enabled = false;
        }

        Debug.Log($"CutSceneTextWriter �� �ҿ� �ð�: {time}��");
    }

}

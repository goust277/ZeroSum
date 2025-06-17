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

                if (c == '<')  // 태그 시작
                {
                    insideTag = true;
                    tagBuffer += c;
                    continue;
                }

                if (insideTag)
                {
                    tagBuffer += c;
                    if (c == '>')  // 태그 끝
                    {
                        insideTag = false;
                        currentText += tagBuffer;  // 태그 통째로 추가
                        tagBuffer = "";
                    }
                    continue;  // 태그 다 붙을 때까지 기다림
                }

                currentText += c;
                dialogueText.text = currentText;

                yield return new WaitForSeconds(0.05f);
                time += 0.05f;
            }

            dialogueText.text += "\n";
            // 대기 시간 설정
            float waitTime = 1.0f;
            if (dialog.Length >= 20) waitTime = 3.0f;
            else if (dialog.Length >= 10) waitTime = 2.0f;
            else waitTime = 1.0f;

            // 해당 대사 출력 후 대기
            yield return new WaitForSeconds(waitTime);
            time += waitTime;
        }

        yield return new WaitForSeconds(2.0f);
        time += 2.0f;

        if (textBox != null)
        {
            textBox.enabled = false;
        }

        Debug.Log($"CutSceneTextWriter 총 소요 시간: {time}초");
    }

}

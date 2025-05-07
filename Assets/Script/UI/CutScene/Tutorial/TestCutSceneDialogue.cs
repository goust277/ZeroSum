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
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private PlayableDirector director;

    [Header("Dialog Resources")]
    [SerializeField] private string[] texts ={};
    [SerializeField] private Vector3 movingOffset = new Vector3(0.5f, 2.3f, 0);

    [Header("CutScene Resources")]
    private RectTransform uiTextTransform;

    public void Start()
    {
        uiTextTransform = gameObject.GetComponent<RectTransform>();
        uiTextTransform.position = cutsceneManager.GetPlayerTransform().position + movingOffset;

        if (director != null) {
            cutsceneManager.PlayCutscene(director);
        }
        StartCoroutine(CutSceneTextWriter());
    }


    IEnumerator CutSceneTextWriter()
    {
        foreach (var dialog in texts)
        {
            //Debug.Log(dialog);
            //foreach (var line in dialog)
            //{   //desTXT.text = line;
            dialogueText.text = "";
            for (int index = 0; index < dialog.Length; index++)
            {
                dialogueText.text += dialog[index].ToString();

                // 텍스트 추가 후 강제로 레이아웃 갱신
                //LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueText.rectTransform);
                yield return new WaitForSeconds(0.05f); //
            }
            dialogueText.text += "\n";
            yield return new WaitForSeconds(1.0f);
            //}
        }
        yield return new WaitForSeconds(0.5f);

        textBox.enabled = false; // 대사창 비활성화
    }

}

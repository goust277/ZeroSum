using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TestCutScneDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private PlayableDirector director;

    [SerializeField] private string[] texts ={};


    public void Start()
    {
        cutsceneManager.PlayCutscene(director);
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

                // �ؽ�Ʈ �߰� �� ������ ���̾ƿ� ����
                //LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueText.rectTransform);
                yield return new WaitForSeconds(0.05f); //
            }
            dialogueText.text += "\n";
            yield return new WaitForSeconds(1.0f);
            //}
        }
        yield return new WaitForSeconds(0.5f);

        textBox.gameObject.SetActive(false); // ���â ��Ȱ��ȭ
        cutsceneManager.OnCutsceneEnd(director);
    }

}

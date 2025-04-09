using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestCutScneDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private string[] texts =
    {
        "requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.GetCurrentSceneID());",
        " mapImage.gameObject.GetComponent<RectTransform>()"
    };


    public void ShowDialogue(string text)
    {
        //dialogueText.text = text;
        textBox.gameObject.SetActive(true);  // ���â Ȱ��ȭ
        StartCoroutine(CutSceneTextWriter());
    }

    public void HideDialogue()
    {
        textBox.gameObject.SetActive(false); // ���â ��Ȱ��ȭ
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
                yield return new WaitForSeconds(0.03f); //
            }
            dialogueText.text += "\n";
            yield return new WaitForSeconds(1.0f);
            //}
        }
    }
}

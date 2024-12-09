using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private DialogueRoot dialogueRoot;
    private List<DialogData> RequiredScenes;

    #region ������ ����X
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    #endregion

    public List<Image> portraits; //��ȭâ�� ��� �ʻ�ȭ ����


    void Start()
    {
        LoadDialogueData("Dialog.json");

        // SecneID ���� n(=0) �� Dialog�� ��������
        RequiredScenes = GetDialogBySecneID(0);

        StartCoroutine(TypeWriter());
 
    }
    // JSON ������ �ҷ��� �Ľ�
    private void LoadDialogueData(string filename)
    {
        string Path = Application.dataPath + "/Resources/Json/" + filename;
        string jsonData = File.ReadAllText(Path);
        dialogueRoot = JsonConvert.DeserializeObject<DialogueRoot>(jsonData);
    }

    // Ư�� SecneID�� Dialog ��ȯ
    public List<DialogData> GetDialogBySecneID(int secneID)
    {
        //dialogueRoot�� Secnes���� SecneID�� �Ķ���� secneID�� ���� ã�Ƽ� scene �� ����..
        SecneData secne = dialogueRoot.Secnes.Find(scene => scene.SecneID == secneID);

        //secne�� ���̾ƴϸ� ���̾�α� ��ȯ ���̸� �� ��ȯ
        return secne?.Dialog;
    }

    IEnumerator TypeWriter()
    {
        foreach (var dialog in RequiredScenes)
        {
            nameTXT.text = dialog.name;
            UpdatePortraits(dialog.pos);

            //��� Ÿ���� �ִ�
            foreach (var line in dialog.log)
            {   //desTXT.text = line;
                desTXT.text = "";
                for (int index = 0; index < line.Length; index++)
                {
                    desTXT.text += line[index].ToString();
                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitUntil(() => Input.GetKey(KeyCode.E));
            }
        }
        //�ʿ��� ��ȭ �������� ����Ʈ �����
        RequiredScenes.Clear();
        portraits.Clear();
    }

    //���ϴ� ��� �ٲ�� �ʻ�ȭ ����ٲ������
    private void UpdatePortraits(int speakingPortraits)
    {
        for (int i = 0; i < portraits.Count; i++)
        {
            //pos�� ���� ������� ���ʺ��� 0~n��. 
            // �ش���ġ�� ĳ���Ͱ� ���ϰ� ������ ���� ��(1,1,1,1)����ϰ� �ƴϸ� ��Ӱ� ó����
            portraits[i].color = (i == speakingPortraits) ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1);

        }
    }

}

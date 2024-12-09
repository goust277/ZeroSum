using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
    private DialogueRoot dialogueRoot;
    private List<DialogData> requiredScenes;

    #region ���̽� ��¹� ��ó�� UI
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    [SerializeField] private GameObject conversationUI; // ��ȭǥ�� UI
    [SerializeField] private GameObject interactPrompt; // ��ȣ�ۿ� Ű ǥ�� UI
    #endregion

    public bool isConversation = false; // ��ȭâ�� ���� ���ִ��� ���� 

    [SerializeField] private int currentEventID = 0;
    public List<Image> portraits; //��ȭâ�� ��� �ʻ�ȭ


    void Start()
    {
        LoadDialogueData("Chap0Dialog.json");

        // SecneID ���� n(=0) �� Dialog�� ��������

        requiredScenes = GetDialogBySecneID(currentEventID);
        conversationUI.SetActive(false);
        interactPrompt.SetActive(false);
    }


    #region ���̽� �Ľ�
    // JSON ������ �ҷ��� �Ľ�
    private void LoadDialogueData(string filename)
    {
        string Path = Application.dataPath + "/Resources/Json/Ver00/Dialog/" + filename;
        string jsonData = File.ReadAllText(Path);
        dialogueRoot = JsonConvert.DeserializeObject<DialogueRoot>(jsonData);
    }

    // ������ ��� �����ϴ��� Ȯ���ϴ� �Լ�
    private bool CheckEventConditions(EventConditions eventConditions, Dictionary<string, bool> currentEventConditions)
    {
        // �ʿ� ���ǵ��� ��� Ȯ��
        foreach (string condition in eventConditions.needEventConditions)
        {
            // currentEventConditions�� ���� �����̰ų� ���� false�� ������ �������� ����
            if (!currentEventConditions.ContainsKey(condition) || !currentEventConditions[condition])
            {
                return false;
            }
        }

        // ��� ������ �����Ǹ� true ��ȯ
        return true;
    }

    // Ư�� SecneID�� Dialog ��ȯ
    private List<DialogData> GetDialogBySecneID(int secneID)
    {
        //dialogueRoot�� Secnes���� SecneID�� �Ķ���� secneID�� ���� ã�Ƽ� scene �� ����..
        SecneData secne = dialogueRoot.Secnes.Find(scene => scene.SecneID == secneID);
        //secne�� ���̾ƴϸ� ���̾�α� ��ȯ ���̸� �� ��ȯ

        return secne?.dialog;
    }
    #endregion


    #region ��� ��� �� UI ������Ʈ
    IEnumerator TypeWriter()
    {
        foreach (var dialog in requiredScenes)
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
                    yield return new WaitForSeconds(0.05f); //Ÿ���� �ӵ� = 1�ڴ� 0.05��
                }
                yield return new WaitUntil(() => Input.GetKey(KeyCode.E));
            }
        }

        EndConversation();
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

    // ��ȭ ����
    public void StartConversation()
    {
        isConversation = true;
        conversationUI.SetActive(true);
        StartCoroutine(TypeWriter());
    }

    // ��ȭ ����
    private void EndConversation()
    {
        conversationUI.SetActive(false);
        requiredScenes.Clear();
        portraits.Clear();
        isConversation = false;
    }
    #endregion
}

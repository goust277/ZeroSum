using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;
using System;


public class DialogueManager : MonoBehaviour
{
    private DialogueRoot ChapterRoot;
    private SecneData requiredSecneData;
    private List<DialogData> requiredScenes;

    #region ���̽� ��¹� ��ó�� UI
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    [SerializeField] private GameObject conversationUI; // ��ȭǥ�� UI
    [SerializeField] private GameObject interactPrompt; // ��ȣ�ۿ� Ű ǥ�� UI
    #endregion

    public bool isConversation = false; // ��ȭâ�� ���� ���ִ��� ���� 

    public List<Image> portraits; //��ȭâ�� ��� �ʻ�ȭ


    void Start()
    {
        LoadChapterData(GameStateManager.Instance.chapterNum);

        // SecneID ���� n(=0) �� Dialog�� ��������

        requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.currentSceneID);
        conversationUI.SetActive(false);
        interactPrompt.SetActive(false);
    }


    #region ���̽� �Ľ�
    // JSON ������ �ҷ��� �Ľ�
    private void LoadChapterData(int chapterNum)
    {
        //é�� ��� ����
        string fName = $"Chap{chapterNum}Dialog.json";
        string Path = Application.dataPath + "/Resources/Json/Ver00/Dialog/" + fName;
        string jsonData = File.ReadAllText(Path);
        ChapterRoot = JsonConvert.DeserializeObject<DialogueRoot>(jsonData);


        //é�� �̺�Ʈ �÷��� ����
        Path = Application.dataPath + "/Resources/Json/Ver00/Dataset/Eventcondition.json";
        jsonData = File.ReadAllText(Path);

        EventRoot currentEventFlags = JsonConvert.DeserializeObject<EventRoot>(jsonData);
        Event events = currentEventFlags.Events.Find(e => e.chapterNum == chapterNum);

        GameStateManager.Instance.currentEventFlags = events?.EventFlags;
    }

    // ������ ��� �����ϴ��� Ȯ���ϴ� �Լ�
    private bool CheckEventConditions(Prerequisites prerequisites, int CollisionNPC)
    {
        if(prerequisites.npc != CollisionNPC) return false;
        // �ʿ� ���ǵ��� ��� Ȯ��

        if(prerequisites.needEventConditions != null)
        {
            foreach (string condition in prerequisites.needEventConditions)
            {
                // currentEventConditions�� ���� �����̰ų� ���� false�� ������ �������� ����
                if (GameStateManager.Instance.currentEventFlags[condition])
                {
                    return false;
                }
            }
        }
        if(prerequisites.currentSecneID != GameStateManager.Instance.currentSceneID) return false; 

        // ��� ������ �����Ǹ� true ��ȯ
        return true;
    }

    // Ư�� SecneID�� Dialog ��ȯ
    private SecneData GetDialogBySecneID(int secneID)
    {
        //dialogueRoot�� Secnes���� SecneID�� �Ķ���� secneID�� ���� ã�Ƽ� scene �� ����..
        SecneData secne = ChapterRoot.Secnes.Find(scene => scene.SecneID == secneID);
        //secne�� ���̾ƴϸ� ���̾�α� ��ȯ ���̸� �� ��ȯ
        return secne;
    }

    private void AfterConversationProcess(AfterConditions after)
    {
        foreach (string condition in after.changeEventConditions)
        {
            //�̺�Ʈ �� �÷��� true ����
            GameStateManager.Instance.currentEventFlags[condition] = true;
        }
        GameStateManager.Instance.currentSceneID = after.changeSecneID;
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
    public void StartConversation(int CollisionNPC)
    {
        requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.currentSceneID);

        if (requiredSecneData == null)
        {
            Debug.LogError($"No dialog found for Scene ID: {GameStateManager.Instance.currentSceneID}");
            return; // ��ȭ �����Ͱ� ������ �޼��带 ����
        }

        if (CheckEventConditions(requiredSecneData.prerequisites, CollisionNPC))
        {
            requiredScenes = requiredSecneData?.dialog;
            isConversation = true;
            conversationUI.SetActive(true);
            StartCoroutine(TypeWriter());
        }
        Debug.Log("StartConversation�� ������ ��" + CollisionNPC + "�� ��ȭ");
    }

    // ��ȭ ����
    private void EndConversation()
    {
        AfterConversationProcess(requiredSecneData.afterConditions);
        conversationUI.SetActive(false);
        requiredScenes.Clear();
        portraits.Clear();
        isConversation = false;
    }
    #endregion
}

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
using System.Reflection;
using Unity.VisualScripting;


public class DialogueManager : MonoBehaviour
{
    private DialogueRoot ChapterRoot;
    private SecneData requiredSecneData;
    private List<DialogData> requiredScenes;

    #region ���̽� ��¹� ��ó�� UI
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    [SerializeField] private GameObject conversationUI; // ��ȭǥ�� UI
    #endregion

    public bool isConversation = false; // ��ȭâ�� ���� ���ִ��� ���� 

    public List<Image> portraits; //��ȭâ�� ��� �ʻ�ȭ
    private Dictionary<int, NPCInfo> npcDictionary = new Dictionary<int, NPCInfo>();
    private NPCInfo ColNPC;


    private void Awake()
    {
        LoadNPCs();
    }

    private void LoadNPCs()
    {
        string Path = Application.dataPath + "/Resources/Json/Ver00/Dataset/NPC.json";
        string jsonData = File.ReadAllText(Path);

        NPCData npcData = JsonConvert.DeserializeObject<NPCData>(jsonData);
        foreach (var npc in npcData.NPCs) //���Ǿ��� id�� �ٷ� ���� ���� �����ϰ� ����
        {
            npcDictionary[npc.id] = npc;
        }
    }

    void Start()
    {
        LoadChapterData(GameStateManager.Instance.chapterNum);

        // SecneID ���� n(=0) �� Dialog�� ��������

        //requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.currentSceneID);
        conversationUI.SetActive(false);
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
    #endregion

    #region ��ȭ ��ó��
    public NPCInfo GetNPC(int id)
    {
        if (npcDictionary.TryGetValue(id, out NPCInfo npc))
        {
            return npc;
        }
        else
        {
            Debug.LogWarning($"NPC with ID {id} not found in cache.");
            return null;
        }
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

        // ��� ������ �����Ǹ� true ��ȯ
        return prerequisites.currentSecneID == GameStateManager.Instance.currentSceneID;
    }

    // Ư�� SecneID�� Dialog ��ȯ
    private SecneData GetDialogBySecneID(int secneID)
    {
        //dialogueRoot�� Secnes���� SecneID�� �Ķ���� secneID�� ���� ã�Ƽ� scene �� ����..
        SecneData secne = ChapterRoot.Secnes.Find(scene => scene.SecneID == secneID);
        //secne�� ���̾ƴϸ� ���̾�α� ��ȯ ���̸� �� ��ȯ
        return secne;
    }
    private void PortraitArrangement()
    {
        HashSet<int> uniqueNpcIds = new HashSet<int>();
        string[] portraitPaths = new string[4];

        //��ȭ�� �ʿ��� ���Ǿ��� Ȯ���ϰ� �ʻ�ȭ �ҷ�����
        foreach (var entry in requiredSecneData.dialog)
        {
            if (uniqueNpcIds.Add(entry.id) && npcDictionary.TryGetValue(entry.id, out NPCInfo npc))
            {
                portraitPaths[entry.pos] = npc.portrait;
                Debug.Log($"portraitPaths[{entry.pos}] = {npc.portrait};");
            }
            else if (!npcDictionary.ContainsKey(entry.id))
            {
                Debug.LogWarning($"NPC with ID {entry.id} not found in cache.");
            }
        }

        //�ҷ��� �ʻ�ȭ pos�� �°� ������Ʈ�� ��ġ�ϱ�
        for (int i = 0; i < portraits.Count; i++)
        {
            if (!string.IsNullOrEmpty(portraitPaths[i]))
            {
                Sprite portraitSprite = Resources.Load<Sprite>(portraitPaths[i]);
                if (portraitSprite != null)
                {
                    portraits[i].sprite = portraitSprite;
                }
                else
                {
                    Debug.LogWarning($"Sprite not found at path: {portraitPaths[i]}");
                }
            }
            else
            {
                Color color = portraits[i].color;
                color.a = .0f;
            }
        }
    }
    #endregion

    #region ��� ��� �� UI ������Ʈ
    private void NormalCommunication()
    {
        if (requiredSecneData == null) return; // null Ȯ��

        requiredScenes = requiredSecneData.dialog;
        //�ʻ�ȭ��ġ
        PortraitArrangement();
        isConversation = true;
        conversationUI.SetActive(true);
        StartCoroutine(TypeWriter());
    }
    private void DefaultSpeech()
    {
        isConversation = true;

        if (ColNPC == null)
        {
            Debug.LogError("ColNPC is null!");
            return; // ColNPC�� null�� ��� ó��
        }

        Sprite portraitSprite = Resources.Load<Sprite>(ColNPC.portrait);
        if (portraitSprite != null)
        {
            portraits[1].sprite = portraitSprite;
        }

        conversationUI.SetActive(true);

        Color color = portraits[0].color;
        color.a = .0f;
        color = portraits[3].color;
        color.a = .0f;
        color = portraits[2].color;
        color.a = .0f;

        StartCoroutine(DefaultTypeWriter());
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

    #region ��ȭ Ÿ���� �ڷ�ƾ
    //���ϴ� ��� �ٲ�� �ʻ�ȭ ����ٲ������
    private void UpdatePortraits(int speakingPortraits)
    {
        for (int i = 0; i < portraits.Count; i++)
        {
            if (portraits[i].gameObject.activeSelf) // ���� ������Ʈ�� Ȱ��ȭ�� ��쿡�� ó��
            {
                //pos�� ���� ������� ���ʺ��� 0~n��. 
                // �ش���ġ�� ĳ���Ͱ� ���ϰ� ������ ���� ��(1,1,1,1)����ϰ� �ƴϸ� ��Ӱ� ó����
                portraits[i].color = (i == speakingPortraits) ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1);
            }

        }
    }

    IEnumerator TypeWriter()
    {
        if (requiredScenes == null || requiredScenes.Count == 0) yield break; // null üũ

        foreach (var dialog in requiredScenes)
        {
            nameTXT.text = GetNPC(dialog.id)?.name;
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

        AfterConversationProcess(requiredSecneData.afterConditions);
        EndConversation();
    }

    IEnumerator DefaultTypeWriter()
    {
        nameTXT.text = ColNPC.name;
        UpdatePortraits(1);
        //��� Ÿ���� �ִ�

        desTXT.text = "";
        for (int index = 0; index < ColNPC.defalutDialog.Length; index++)
        {
            desTXT.text += ColNPC.defalutDialog[index].ToString();
            yield return new WaitForSeconds(0.05f); //Ÿ���� �ӵ� = 1�ڴ� 0.05��
        }
        yield return new WaitUntil(() => Input.GetKey(KeyCode.E));
        EndConversation();
    }

    #endregion

    #region ��ȭ �յ� ȣ��� public func
    // ��ȭ ����
    public void StartConversation(int CollisionNPC)
    {
        requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.currentSceneID);
        ColNPC = GetNPC(CollisionNPC);

        if (requiredSecneData == null)
        {
            DefaultSpeech();
            Debug.Log($"No dialog found for Scene ID: {GameStateManager.Instance.currentSceneID}");
            return; // ��ȭ �����Ͱ� ������ �޼��带 ����
        }
        

        if (CheckEventConditions(requiredSecneData.prerequisites, CollisionNPC)) //��ȭ���� ���¿��� ��ȭ�� �ϸ�
        {
            NormalCommunication();
        }
        else
        {
            DefaultSpeech();
        }
    }

    // ��ȭ ����
    private void EndConversation()
    {
        conversationUI.SetActive(false);
        isConversation = false;
        if (requiredSecneData == null)
        {
            Debug.LogError("requiredSecneData is null");
            return; // null�� ��� ó��
        }

        if (requiredScenes != null)
        {
            requiredScenes.Clear();
        }
    }
    #endregion
}

using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
//using static UnityEditor.Progress;
using System;
using System.Reflection;
using Unity.VisualScripting;


public class DialogueManager : MonoBehaviour
{
    private DialogueRoot ChapterRoot;
    private SecneData requiredSecneData;
    private List<DialogData> requiredScenes;

    #region conversation UI Resources
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    [SerializeField] private GameObject conversationUI; // ��ȭǥ�� UI
    #endregion

    public bool isConversation = false; // ��ȭâ�� ���� ���ִ��� ���� 

    [SerializeField] private GameObject[] portraits = new GameObject[4];
    //public List<Image> portraits; //��ȭâ�� ��� �ʻ�ȭ
    private Dictionary<int, NPCInfo> npcDictionary = new Dictionary<int, NPCInfo>();
    private NPCInfo ColNPC;


    private void Awake()
    {
        LoadNPCs();
    }

    private void LoadNPCs()
    {
        TextAsset NPCJson = Resources.Load<TextAsset>("Json/Ver00/Dataset/NPC");

        //string Path = Application.dataPath + "/Resources/Json/Ver00/Dataset/NPC.json";
        //string jsonData = File.ReadAllText(Path);

        string jsonData = NPCJson.text;

        NPCData npcData = JsonConvert.DeserializeObject<NPCData>(jsonData);
        foreach (NPCInfo npc in npcData.NPCs) //���Ǿ��� id�� �ٷ� ���� ���� �����ϰ� ����
        {
            npcDictionary[npc.NPCid] = npc;
        }
    }

    void Start()
    {
        conversationUI.SetActive(false);
        LoadChapterData(GameStateManager.Instance.GetChapterNum());

        // SecneID ���� n(=0) �� Dialog�� ��������

        //requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.currentSceneID);

    }

    #region ���̽� �Ľ�
    // JSON ������ �ҷ��� �Ľ�
    private void LoadChapterData(int chapterNum)
    {
        //é�� ��� ����
        string fName = $"Chap{chapterNum}Dialog";
        string Path = "Json/Ver00/Dialog/" + fName;

        TextAsset NPCJson = Resources.Load<TextAsset>(Path);

        //string jsonData = File.ReadAllText(Path);
        string jsonData = NPCJson.text;

        ChapterRoot = JsonConvert.DeserializeObject<DialogueRoot>(jsonData);
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

    // ������ ��� �����ϴ��� Ȯ���ϴ� �Լ�* ���丮 ������ �����Ѱ�?
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
        return prerequisites.currentSecneID == GameStateManager.Instance.GetCurrentSceneID();
    }

    // Ư�� SecneID�� Dialog ��ȯ
    private SecneData GetDialogBySecneID(int secneID)
    {
        //dialogueRoot�� Secnes���� SecneID�� �Ķ���� secneID�� ���� ã�Ƽ� scene �� ����..
        //secne�� ���̾ƴϸ� ���̾�α� ��ȯ ���̸� �� ��ȯ
        return ChapterRoot.Secnes.Find(scene => scene.SecneID == secneID);
    }

    //�ʻ�ȭ ��ġ
    private void PortraitArrangement()
    {
        HashSet<int> uniqueNpcIds = new HashSet<int>();
        string[] portraitPaths = new string[4];

        //��ȭ�� �ʿ��� ���Ǿ��� Ȯ���ϰ� �ʻ�ȭ �ҷ�����
        foreach (var entry in requiredSecneData.dialog)
        {
            if (uniqueNpcIds.Add(entry.id) && npcDictionary.TryGetValue(entry.id, out NPCInfo npc))
            {
                portraitPaths[entry.pos] = npc.NPCportrait;
                Debug.Log($"portraitPaths[{entry.pos}] = {npc.NPCportrait};");
            }
            else if (!npcDictionary.ContainsKey(entry.id))
            {
                Debug.LogWarning($"NPC with ID {entry.id} not found in cache.");
            }
        }

        //�ҷ��� �ʻ�ȭ pos�� �°� ������Ʈ�� ��ġ�ϱ�
        for (int i = 0; i < portraitPaths.Length; i++)
        {
            if (!string.IsNullOrEmpty(portraitPaths[i]))
            {
                Sprite portraitSprite = Resources.Load<Sprite>(portraitPaths[i]);
                if (portraitSprite != null)
                {
                    portraits[i].GetComponent<Image>().sprite = portraitSprite;
                    portraits[i].GetComponent<AdjustSpriteSize>().SetSprite();
                }
                else
                {
                    Debug.LogWarning($"Sprite not found at path: {portraitPaths[i]}");
                }
            }
            else
            {
                portraits[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region ��� ��� �� UI ������Ʈ
    private void NormalCommunication() //���丮 ���డ���� ��ȭ�� ���
    {
        if (requiredSecneData == null) return; // null Ȯ��

        requiredScenes = requiredSecneData.dialog;
        //�ʻ�ȭ��ġ
        PortraitArrangement();
        isConversation = true;
        conversationUI.SetActive(true);
        StartCoroutine(TypeWriter());
    }
    private void DefaultSpeech() //���丮 ��ȭ�� �ƴ� �Ϲݴ�ȭ
    {
        isConversation = true;

        if (ColNPC == null) //����ó��
        {
            Debug.LogError("ColNPC is null");
            return; // ColNPC�� null�� ��� ó��
        }

        DefaultPortraitArrangement();

        UpdatePortraits(1);
        if (ColNPC.itemsForSale != null) //���Ǿ��� �������ҵ� �� ���
        {
            Debug.Log(ColNPC.NPCname + "���Ǿ��� ���� ���Ǿ��Դϴ�.");
        }

        StartCoroutine(DefaultTypeWriter());

    }

    private void DefaultPortraitArrangement() //�Ϲ� ��ȭ�� �ʻ�ȭ �迭
    {
        Sprite portraitSprite = Resources.Load<Sprite>(ColNPC.NPCportrait);
        if (portraitSprite != null)
        {
            portraits[1].GetComponent<Image>().sprite = portraitSprite;
        }

        conversationUI.SetActive(true);
        portraits[0].SetActive(false);
        portraits[2].SetActive(false);
        portraits[3].SetActive(false);
    }

    private void AfterConversationProcess(AfterConditions after)
    {
        if (after == null) {
            return;
        }
        foreach (string condition in after.changeEventConditions)
        {
            //�̺�Ʈ �� �÷��� true ����
            GameStateManager.Instance.currentEventFlags[condition] = true;
        }
        GameStateManager.Instance.SetCurrenSceneID(after.changeSecneID);
    }

    #endregion

    #region ��ȭ Ÿ���� �ڷ�ƾ
    //���ϴ� ��� �ٲ�� �ʻ�ȭ ����ٲ������
    private void UpdatePortraits(int speakingPortraits)
    {
        for (int i = 0; i < portraits.Length; i++)
        {
            if (portraits[i].activeSelf) // ���� ������Ʈ�� Ȱ��ȭ�� ��쿡�� ó��
            {
                //pos�� ���� ������� ���ʺ��� 0~n��. 
                // �ش���ġ�� ĳ���Ͱ� ���ϰ� ������ ���� ��(1,1,1,1)����ϰ� �ƴϸ� ��Ӱ� ó����
                portraits[i].GetComponent<Image>().color = (i == speakingPortraits) ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1);
            }

        }
    }

    IEnumerator TypeWriter()
    {
        if (requiredScenes == null || requiredScenes.Count == 0) yield break; // null üũ

        foreach (var dialog in requiredScenes)
        {
            nameTXT.text = GetNPC(dialog.id)?.NPCname;
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
                yield return new WaitUntil(() => Input.GetKey(KeyCode.F));
            }
        }

        AfterConversationProcess(requiredSecneData.afterConditions);
        EndConversation();
    }

    IEnumerator DefaultTypeWriter()
    {
        nameTXT.text = ColNPC.NPCname;
        //��� Ÿ���� �ִ�

        desTXT.text = "";
        for (int index = 0; index < ColNPC.defalutDialog.Length; index++)
        {
            desTXT.text += ColNPC.defalutDialog[index].ToString();
            yield return new WaitForSeconds(0.05f); //Ÿ���� �ӵ� = 1�ڴ� 0.05��
        }
        yield return new WaitUntil(() => Input.GetKey(KeyCode.F));
        EndConversation();
    }

    #endregion

    #region ��ȭ �յ� ȣ��� public func
    // ��ȭ ����
    public void StartConversation(int CollisionNPC)
    {
        requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.GetCurrentSceneID());
        ColNPC = GetNPC(CollisionNPC);

        if (requiredSecneData == null)
        {
            DefaultSpeech();
            Debug.Log($"No dialog found for Scene ID: {GameStateManager.Instance.GetCurrentSceneID()}");
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
        portraits[0].gameObject.SetActive(true);
        portraits[1].gameObject.SetActive(true);
        portraits[2].gameObject.SetActive(true);
        portraits[3].gameObject.SetActive(true);

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

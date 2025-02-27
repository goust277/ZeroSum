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
using System.Xml;


public class Ver01_ConvManager : MonoBehaviour
{
    private DialogueRoot ChapterRoot;
    private SecneData requiredSecneData;
    private List<DialogData> requiredScenes;

    #region conversation UI Resources
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI totalLogTXT; //prtivate
    #endregion
    public bool isConversation = false; // ��ȭâ�� ���� ���ִ��� ���� 

    [SerializeField] private GameObject[] portraits = new GameObject[2];
    //public List<Image> portraits; //��ȭâ�� ��� �ʻ�ȭ
    private Dictionary<int, NPCInfo> npcDictionary = new Dictionary<int, NPCInfo>();
    private NPCInfo ColNPC;

    private Material tmpMaterial;


    private void Awake()
    {
        LoadNPCs();
    }

    private void LoadNPCs()
    {
        TextAsset NPCJson = Resources.Load<TextAsset>("Json/Ver01/Dataset/NPC");

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
        //conversationUI.SetActive(false);
        LoadChapterData(GameStateManager.Instance.GetChapterNum());

        // SecneID ���� n(=0) �� Dialog�� ��������

        requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.GetCurrentSceneID());
        NormalCommunication();

        // Outline 효과 적용
        totalLogTXT.outlineWidth = 0.5f; // 외곽선 두께
        totalLogTXT.outlineColor = Color.cyan; // 네온 색상
    }

    #region DialogResourLoad
    // JSON ������ �ҷ��� �Ľ�
    private void LoadChapterData(int chapterNum)
    {
        //é�� ��� ����
        string fName = $"Chap{chapterNum}Dialog";
        string Path = "Json/Ver01/Dialog/" + fName;

        TextAsset NPCJson = Resources.Load<TextAsset>(Path);

        //string jsonData = File.ReadAllText(Path);
        string jsonData = NPCJson.text;

        ChapterRoot = JsonConvert.DeserializeObject<DialogueRoot>(jsonData);
    }
    #endregion

    #region Dialog Controller
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

    // Is posiible Conversation?
    private bool CheckEventConditions(Prerequisites prerequisites, int CollisionNPC)
    {
        if(prerequisites.needEventConditions != null)
        {
            foreach (string condition in prerequisites.needEventConditions)
            {
                if (GameStateManager.Instance.currentEventFlags[condition])
                {
                    return false;
                }
            }
        }
        // 모든 조건 완료시 대회진행
        return prerequisites.currentSecneID == GameStateManager.Instance.GetCurrentSceneID();
    }

    // 현재 대화진행정도
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
        string[] portraitPaths = new string[3];

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
        requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.GetCurrentSceneID());

        if (requiredSecneData == null) return; // null Ȯ��
        requiredScenes = requiredSecneData.dialog;
        //�ʻ�ȭ��ġ

        //PortraitArrangement();
        isConversation = true;
        StartCoroutine(TypeWriter());
    }

    private void AfterConversationProcess(AfterConditions after)
    {
        if (after == null) {
            return;
        }
        foreach (string condition in after.changeEventConditions)
        {
            //대화 플래그 조절
            GameStateManager.Instance.currentEventFlags[condition] = true;
        }

        GameStateManager.Instance.SetCurrenSceneID(after.changeSecneID);
    }

    #endregion

    #region 대화출력
    IEnumerator TypeWriter()
    {
        if (requiredScenes == null || requiredScenes.Count == 0) yield break; // null chk

        foreach (var dialog in requiredScenes)
        {
            
            string portraitPaths = GetNPC(dialog.id)?.NPCportrait;
            Sprite portraitSprite = Resources.Load<Sprite>(portraitPaths);
            if (portraitSprite != null)
            {
                portraits[dialog.pos].GetComponent<Image>().sprite = portraitSprite;
                portraits[dialog.pos].GetComponent<AdjustSpriteSize>().SetSprite();
            }
            else
            {
                Debug.LogWarning($"Sprite not found at path: {portraitPaths}");
            }

            nameTXT.text = GetNPC(dialog.id)?.NPCname;

            foreach (var line in dialog.log)
            {   //desTXT.text = line;
                desTXT.text = "";
                for (int index = 0; index < line.Length; index++)
                {
                    desTXT.text += line[index].ToString();
                    yield return new WaitForSeconds(0.05f); //Ÿ���� �ӵ� = 1�ڴ� 0.05��
                }
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                totalLogTXT.text += GetNPC(dialog.id)?.NPCname + " : " + line + "\n";
            }

        }

        AfterConversationProcess(requiredSecneData.afterConditions);
        EndConversation();
    }
    #endregion

    #region ��ȭ �յ� ȣ��� public func
    // ��ȭ ����
    public void StartConversation(int CollisionNPC)
    {
        
        ColNPC = GetNPC(CollisionNPC);

        if (requiredSecneData == null)
        {
            Debug.Log($"No dialog found for Scene ID: {GameStateManager.Instance.GetCurrentSceneID()}");
            return; // ��ȭ �����Ͱ� ������ �޼��带 ����
        }
        

        if (CheckEventConditions(requiredSecneData.prerequisites, CollisionNPC)) //��ȭ���� ���¿��� ��ȭ�� �ϸ�
        {
            NormalCommunication();
        }
    }

    // ��ȭ ����
    private void EndConversation()
    {

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

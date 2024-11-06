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

    #region 제이슨 출력및 전처리 UI
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    [SerializeField] private GameObject conversationUI; // 대화표시 UI
    #endregion

    public bool isConversation = false; // 대화창이 지금 떠있는지 여부 

    public List<Image> portraits; //대화창에 띄울 초상화
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
        foreach (NPCInfo npc in npcData.NPCs) //엔피씨의 id로 바로 내용 접근 가능하게 설정
        {
            npcDictionary[npc.NPCid] = npc;
        }
    }

    void Start()
    {
        LoadChapterData(GameStateManager.Instance.GetChapterNum());

        // SecneID 값이 n(=0) 인 Dialog만 가져오기

        //requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.currentSceneID);
        conversationUI.SetActive(false);
    }

    #region 제이슨 파싱
    // JSON 파일을 불러와 파싱
    private void LoadChapterData(int chapterNum)
    {
        //챕터 대사 파일
        string fName = $"Chap{chapterNum}Dialog.json";
        string Path = Application.dataPath + "/Resources/Json/Ver00/Dialog/" + fName;
        string jsonData = File.ReadAllText(Path);
        ChapterRoot = JsonConvert.DeserializeObject<DialogueRoot>(jsonData);
    }
    #endregion

    #region 대화 전처리
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

    // 조건을 모두 만족하는지 확인하는 함수* 스토리 진행이 가능한가?
    private bool CheckEventConditions(Prerequisites prerequisites, int CollisionNPC)
    {
        if(prerequisites.npc != CollisionNPC) return false;
        // 필요 조건들을 모두 확인

        if(prerequisites.needEventConditions != null)
        {
            foreach (string condition in prerequisites.needEventConditions)
            {
                // currentEventConditions에 없는 조건이거나 값이 false면 조건을 만족하지 않음
                if (GameStateManager.Instance.currentEventFlags[condition])
                {
                    return false;
                }
            }
        }
        // 모든 조건이 만족되면 true 반환
        return prerequisites.currentSecneID == GameStateManager.Instance.GetCurrentSceneID();
    }

    // 특정 SecneID의 Dialog 반환
    private SecneData GetDialogBySecneID(int secneID)
    {
        //dialogueRoot의 Secnes에서 SecneID가 파라미터 secneID인 것을 찾아서 scene 를 저장..
        //secne이 널이아니면 다이어로그 반환 널이면 널 반환
        return ChapterRoot.Secnes.Find(scene => scene.SecneID == secneID);
    }

    //초상화 배치
    private void PortraitArrangement()
    {
        HashSet<int> uniqueNpcIds = new HashSet<int>();
        string[] portraitPaths = new string[4];

        //대화에 필요한 엔피씨들 확인하고 초상화 불러오기
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

        //불러온 초상화 pos에 맞게 오브젝트에 배치하기
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
                portraits[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region 대사 출력 및 UI 업데이트
    private void NormalCommunication() //스토리 진행가능한 대화일 경우
    {
        if (requiredSecneData == null) return; // null 확인

        requiredScenes = requiredSecneData.dialog;
        //초상화배치
        PortraitArrangement();
        isConversation = true;
        conversationUI.SetActive(true);
        StartCoroutine(TypeWriter());
    }
    private void DefaultSpeech() //스토리 대화가 아닌 일반대화
    {
        isConversation = true;

        if (ColNPC == null) //예외처리
        {
            Debug.LogError("ColNPC is null");
            return; // ColNPC가 null일 경우 처리
        }

        DefaultPortraitArrangement();

        UpdatePortraits(1);
        if (ColNPC.itemsForSale != null) //엔피씨가 상점역할도 할 경우
        {
            Debug.Log(ColNPC.NPCname + "엔피씨는 상점 엔피씨입니다.");
        }
        else
        {
            StartCoroutine(DefaultTypeWriter());
        }
    }

    private void DefaultPortraitArrangement() //일반 대화의 초상화 배열
    {
        Sprite portraitSprite = Resources.Load<Sprite>(ColNPC.NPCportrait);
        if (portraitSprite != null)
        {
            portraits[1].sprite = portraitSprite;
        }

        conversationUI.SetActive(true);
        portraits[0].gameObject.SetActive(false);
        portraits[2].gameObject.SetActive(false);
        portraits[3].gameObject.SetActive(false);
    }

    private void AfterConversationProcess(AfterConditions after)
    {
        if (after == null) {
            return;
        }
        foreach (string condition in after.changeEventConditions)
        {
            //이벤트 후 플래그 true 설정
            GameStateManager.Instance.currentEventFlags[condition] = true;
        }
        GameStateManager.Instance.SetCurrenSceneID(after.changeSecneID);
    }

    #endregion

    #region 대화 타이핑 코루틴
    //말하는 사람 바뀌면 초상화 색깔바꿔줘야함
    private void UpdatePortraits(int speakingPortraits)
    {
        for (int i = 0; i < portraits.Count; i++)
        {
            if (portraits[i].gameObject.activeSelf) // 게임 오브젝트가 활성화된 경우에만 처리
            {
                //pos값 보고 순서대로 왼쪽부터 0~n임. 
                // 해당위치의 캐릭터가 말하고 있으면 원래 값(1,1,1,1)출력하고 아니면 어둡게 처리함
                portraits[i].color = (i == speakingPortraits) ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1);
            }

        }
    }

    IEnumerator TypeWriter()
    {
        if (requiredScenes == null || requiredScenes.Count == 0) yield break; // null 체크

        foreach (var dialog in requiredScenes)
        {
            nameTXT.text = GetNPC(dialog.id)?.NPCname;
            UpdatePortraits(dialog.pos);

            //대사 타이핑 애니
            foreach (var line in dialog.log)
            {   //desTXT.text = line;
                desTXT.text = "";
                for (int index = 0; index < line.Length; index++)
                {
                    desTXT.text += line[index].ToString();
                    yield return new WaitForSeconds(0.05f); //타이핑 속도 = 1자당 0.05초
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
        //대사 타이핑 애니

        desTXT.text = "";
        for (int index = 0; index < ColNPC.defalutDialog.Length; index++)
        {
            desTXT.text += ColNPC.defalutDialog[index].ToString();
            yield return new WaitForSeconds(0.05f); //타이핑 속도 = 1자당 0.05초
        }
        yield return new WaitUntil(() => Input.GetKey(KeyCode.F));
        EndConversation();
    }

    #endregion

    #region 대화 앞뒤 호출용 public func
    // 대화 시작
    public void StartConversation(int CollisionNPC)
    {
        requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.GetCurrentSceneID());
        ColNPC = GetNPC(CollisionNPC);

        if (requiredSecneData == null)
        {
            DefaultSpeech();
            Debug.Log($"No dialog found for Scene ID: {GameStateManager.Instance.GetCurrentSceneID()}");
            return; // 대화 데이터가 없으면 메서드를 종료
        }
        

        if (CheckEventConditions(requiredSecneData.prerequisites, CollisionNPC)) //대화가능 상태에서 대화를 하면
        {
            NormalCommunication();
        }
        else
        {
            DefaultSpeech();
        }
    }

    // 대화 종료
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
            return; // null일 경우 처리
        }

        if (requiredScenes != null)
        {
            requiredScenes.Clear();
        }
    }
    #endregion
}

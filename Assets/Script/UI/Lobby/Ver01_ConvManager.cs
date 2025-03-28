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
using TMPro.Examples;
using UnityEngine.SceneManagement;


public class Ver01_ConvManager : MonoBehaviour
{
    private DialogueRoot ChapterRoot;
    private SecneData requiredSecneData;
    private List<DialogData> requiredScenes;

    [Header("Resources Before Conversation")]
    #region conversation UI Resources
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    //[SerializeField] protected TextMeshProUGUI totalLogTXT; //prtivate
    #endregion
    public bool isConversation = false; // ��ȭâ�� ���� ���ִ��� ���� 

    [SerializeField] private GameObject[] portraits = new GameObject[2];
    //public List<Image> portraits; //��ȭâ�� ��� �ʻ�ȭ
    private Dictionary<int, NPCInfo> npcDictionary = new Dictionary<int, NPCInfo>();
    private NPCInfo ColNPC;
    private Material tmpMaterial;

    [Header("Resource After Conversation")]
    [SerializeField] private Transform[] movingSections = new Transform[2];
    [SerializeField] private GameObject mapImage;
    [SerializeField] protected TextMeshProUGUI missionTXT;
    [SerializeField] private Image pressE;
    [SerializeField] private float duration = 1.0f; // 이동 시간
    private readonly float transitionDuration = 0.2f; 
    private readonly float consistenceDuration = 0.5f; 

    private readonly Color alpha0 = new Color(1f, 1f, 1f, 0f);
    private readonly Color alpha1 = new Color(1f, 1f, 1f, 1f);
    private Coroutine dialogCoroutine = null;
    private bool isTransitionRunning = false;

    private List<String> portraitPaths = new List<String>();


    //[Header("Temp Flags")] //대화 넘기기관련 플래그들
    private Coroutine currentCoroutine;
    //[SerializeField] private bool isSkipLine = false;
    //[SerializeField] private setNextDialog = false;

    [Header("Change Scene String")]
    [SerializeField] private String nextScene;

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

        requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.GetCurrentSceneID());
        NormalCommunication();

        pressE.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            EnteredSkipKey();
        }
     
        if (isTransitionRunning && Input.GetKeyDown(KeyCode.F))
        {
            ChangeScene();
        }
    }

    void EnteredSkipKey()
    {
        StopCoroutine(currentCoroutine);
        Debug.Log("SKIPBTN : F5 IS ENTEREDDDDDDDDDDDDDDD");
        //StopAllCoroutines();  // 모든 코루틴 정리
        pressE.gameObject.SetActive(true);
        currentCoroutine = StartCoroutine(TransitionToSprite());
    }

    #region DialogResourLoad
    // JSON
    private void LoadChapterData(int chapterNum)
    {
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
        return ChapterRoot.Secnes.Find(scene => scene.SecneID == secneID);
    }

    //초상화 배치
    private void PortraitArrangement()
    {
        //��ȭ�� �ʿ��� ���Ǿ��� Ȯ���ϰ� �ʻ�ȭ �ҷ�����
        foreach (var entry in requiredSecneData.dialog)
        {
            if (npcDictionary.TryGetValue(entry.id, out NPCInfo npc))
            {  
                portraitPaths.Add(npc.NPCportrait);
                Debug.Log($"portraitPaths[{entry.pos}] = {npc.NPCportrait};");
            }
            else if (!npcDictionary.ContainsKey(entry.id))
            {
                Debug.LogWarning($"NPC with ID {entry.id} not found in cache.");
            }
        }
    }
    #endregion

    #region afterConvLog
    private void NormalCommunication() 
    {
        requiredSecneData = GetDialogBySecneID(GameStateManager.Instance.GetCurrentSceneID());

        if (requiredSecneData == null) return; 
        requiredScenes = requiredSecneData.dialog;


        PortraitArrangement();
        isConversation = true;
        currentCoroutine = StartCoroutine(TypeWriter());
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

        // active PressE 
        StartCoroutine(MissionWriter());
    }

    IEnumerator MissionWriter()
    {
        foreach (var dialog in requiredSecneData.afterConditions.missionString)
        {
            Debug.Log(dialog);
            //foreach (var line in dialog)
            //{   //desTXT.text = line;
            missionTXT.text = "";
            for (int index = 0; index < dialog.Length; index++)
            {
                missionTXT.text += dialog[index].ToString();
                yield return new WaitForSeconds(0.05f); //
            }
            missionTXT.text += "\n";
            yield return new WaitForSeconds(1.0f);
            //}
        }

        currentCoroutine = StartCoroutine(MoveImageCoroutine());
    }

    private IEnumerator TransitionToSprite()
    {
        isTransitionRunning = true;

        //Debug.Log("TransitionToSprite");
        float time = 0f;
        Color originalColor = pressE.color;
        while (true)
        {
            yield return new WaitForSeconds(consistenceDuration);
            while (time < transitionDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, time / transitionDuration);
                pressE.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            time = 0f;

            yield return new WaitForSeconds(consistenceDuration); 

            while (time < transitionDuration) 
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, time / transitionDuration);
                pressE.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            time = 0f;
        }
    }

    private IEnumerator MoveImageCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 startPos = movingSections[0].position;
        Vector3 endPos = movingSections[1].position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            mapImage.gameObject.GetComponent<RectTransform>().position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        mapImage.gameObject.GetComponent<RectTransform>().position = endPos; // 최종 위치 보정

        pressE.gameObject.SetActive(true);
        currentCoroutine = StartCoroutine(TransitionToSprite());
    }

    //씬넘김
    void ChangeScene()
    {
        isTransitionRunning = false;
        StopAllCoroutines();  // 모든 코루틴 정리
        SceneManager.LoadScene(nextScene);
    }

    #endregion

    #region 대화출력
    private void UpdatePortraits(int speakingPortraits, int index)
    {
        Sprite portraitSprite = Resources.Load<Sprite>(portraitPaths[index]);

        portraits[speakingPortraits].GetComponent<Image>().sprite = portraitSprite;
        portraits[speakingPortraits].GetComponent<AdjustSpriteSize>().SetSprite();

        for (int i = 0; i < portraits.Length; i++)
        {
            if (portraits[i].activeSelf)
            {
                portraits[i].GetComponent<Image>().color = (i == speakingPortraits) ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1);
            }
        }
    }

    IEnumerator TypeWriter()
    {
        int i = 0;
        bool setNextDialog = false;

        if (requiredScenes == null || requiredScenes.Count == 0) yield break; // null 체크

        foreach (var dialog in requiredScenes)
        {
            nameTXT.text = GetNPC(dialog.id)?.NPCname;
            UpdatePortraits(dialog.pos, i);

            foreach (var line in dialog.log)
            {
                desTXT.text = "";
                bool isSkipLine = false;
                dialogCoroutine = StartCoroutine(TypeWriterLine(line));// 대사 출력 코루틴

                //중간에 대사 스킵되도록 처리
                while (!isSkipLine)
                {
                    if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
                    {
                        isSkipLine = true;
                        StopCoroutine(dialogCoroutine); // 중지
                        desTXT.text = line; // 스킵 눌렀을 때 줄 전체 출력
                    }
                    yield return null; // 기다림
                }

                // 대사 출력 후, 다음 대사로 넘어가기 위한 입력 대기
                while (!setNextDialog)
                {
                    if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
                    {
                        setNextDialog = true;
                    }
                    yield return null;
                }
                setNextDialog = false; // 한 번만 작동하도록 초기화

                dialogCoroutine = null;
            }

            i++;
        }

        EndConversation();
    }

    private IEnumerator TypeWriterLine(string line)
    {
        desTXT.text = "";  // 매번 출력 전에 텍스트 초기화
        for (int index = 0; index < line.Length; index++)
        {
            desTXT.text += line[index].ToString();
            yield return new WaitForSeconds(0.05f); // 글자 한 글자씩 출력
        }
    }




    #endregion

    #region public func
    private void EndConversation()
    {
        currentCoroutine = null;
        currentCoroutine = StartCoroutine(MissionWriter());
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

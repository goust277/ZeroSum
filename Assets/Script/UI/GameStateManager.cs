using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using TMPro.Examples;

public class GameStateManager : MonoBehaviour
{

    public static GameStateManager Instance { get; private set; }

    // ���� ���� ������
    public Dictionary<string, bool> currentEventFlags;  // �̺�Ʈ �÷��� (��: �̺�Ʈ �Ϸ� ����)
    [SerializeField] private int currentSceneID = 0;                          // ���� �� ID
    private int chapterNum = 0;                           // ���� é��
    private int Gold = 0;
    private int hp = 5;
    private EventRoot eventRoot;


    [Header("HUD Resource")]
    [SerializeField] private TextMeshProUGUI currentMagazineText;
    [SerializeField] private TextMeshProUGUI totalMagazineText;
    [SerializeField] private TextMeshProUGUI reinforcementText;

    private int currentMagazine;
    private int totalMagazine;
    private int reinforcement;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // ���� �ٲ� ����
            
            // ���� �ʱ�ȭ
            //currentEventFlags = new Dictionary<string, bool>();
        }
    }

    private void Start()
    {
        LoadEventFlags();
        getChangedHP(0);
    }

    private void LoadEventFlags()
    {
        if(currentEventFlags != null)
        {
            Debug.Log("GameStateManager - LoadEventFlags// There is already currentEventFlags............");
            return;
        }
        string Path = Application.dataPath + "/Resources/Json/Ver00/Dataset/Eventcondition.json";
        string jsonData = File.ReadAllText(Path);

        eventRoot = JsonConvert.DeserializeObject<EventRoot>(jsonData);
        Event events = eventRoot.Events.Find(e => e.chapterNum == chapterNum);
        
        currentEventFlags = events?.EventFlags;
        Debug.Log($"GameStateManager - LoadEventFlags// EventFlags: {string.Join(", ", currentEventFlags.Select(kv => $"{kv.Key}: {kv.Value}"))}");
    }


    public void SetEventFlag(string eventName, bool value)
    {
        if (currentEventFlags.ContainsKey(eventName))
        {
            currentEventFlags[eventName] = value;
        }
        else
        {
            currentEventFlags.Add(eventName, value);
        }
    }

    public EventRoot GetEventRoot()
    {
        return eventRoot;
    }

    public bool GetEventFlag(string eventName)
    {
        return currentEventFlags.ContainsKey(eventName) ? currentEventFlags[eventName] : false;
    }

    public int GetCurrentSceneID()
    {
        return currentSceneID;
    }

    public void SetCurrenSceneID(int sceneID)
    {
        currentSceneID = sceneID;
    }

    public int GetChapterNum()
    {
        return chapterNum;
    }

    public void SetchapterNum(int chapNum)
    {
        Event events = eventRoot.Events.Find(e => e.chapterNum == chapterNum);
        if (events != null)
        {
            events.EventFlags = currentEventFlags;
        }

        chapterNum = chapNum;
    }

    //강화수
    public void GetReinforcementItem()
    {

        if (reinforcement == 0)
        {
            //이펙트 강화, 연사력 강화
            reinforcement++;
            reinforcementText.text = reinforcement.ToString();
            return;
        }
        if (reinforcement == 1)
        {
            //이펙트, 외형강화
            reinforcement++;
            reinforcementText.text = reinforcement.ToString();
            AdjustMagazine();
            return;
        }
        if (reinforcement == 2)
        {
            //이펙트 강화, 공격력 5
            reinforcement++;
            reinforcementText.text = reinforcement.ToString();
            return;
        }
        if (reinforcement == 3)
        {

            //이펙트 강화, 외형 강화
            reinforcement++;
            reinforcementText.text = reinforcement.ToString();
            AdjustMagazine();
            
            return;
        }
        Debug.Log("GameStateManager - GetReinforcementItem // Already reinforcement is max");
    }

    public void GetHit()
    {
        reinforcement--;
        if (reinforcement < 0)
        {
            Debug.Log("GameStateManager - GetHit // Game Over");
        }
        reinforcementText.text = reinforcement.ToString();
        AdjustMagazine();
    }


    //탄창수
    private void AdjustMagazine()
    {
        if (reinforcement == 2)
        {
            totalMagazine = 8;
            totalMagazineText.text = totalMagazine.ToString();
            //이펙트, 외형강화
            return;

        }

        if ( reinforcement == 4)
        {
            totalMagazine = 12;
            totalMagazineText.text = totalMagazine.ToString();
            return;
        }
    }



    //hp는 추후에 또 조정해야함
    public void getChangedHP(int fixHP)
    {

    }

    public int getCurrentHP()
    {
        return hp;
    }




}


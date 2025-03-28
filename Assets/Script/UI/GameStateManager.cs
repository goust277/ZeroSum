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
    [SerializeField] private int currentSceneID = 0;
    [SerializeField] private int currentStagePoint = 0;     // 
    private int chapterNum = 0;                           // ���� é��
    //private int Gold = 0;
    private int hp = 5;
    private EventRoot eventRoot;


    [Header("HUD Resource")]
    [SerializeField] private TextMeshProUGUI totalMagazineText;
    [SerializeField] private TextMeshProUGUI reinforcementText;

    private int totalMagazine = 5;
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
        }
    }

    private void Start()
    {
        LoadEventFlags();
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

    public int GetStagePoint()
    {
        return currentStagePoint;
    }

    public void SetStagePoint(int stagePointID)
    {
        currentStagePoint = stagePointID;
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
    public int GetReinforcement()
    {
        return reinforcement;
    }

    public int GetTotalMagazine()
    {
        return totalMagazine;
    }

    public void resetReinforcement()
    {
        reinforcement = 0;
        totalMagazine = 5;
        UpdateReinforcementHUD();
    }

    public void TakeReinforcementItem()
    {
        reinforcement++;
        UpdateReinforcementHUD();
    }

    public void ReinforcementItem()
    {
        reinforcement++;
        UpdateReinforcementHUD();
    }


    public void UseReinforcement()
    {
        if (reinforcement == 0)
        {
            Debug.Log("GameStateManager - GetHit // Game Over");
            return;
        }

        reinforcement--;
        UpdateReinforcementHUD();
    }

    //탄창수
    private void UpdateReinforcementHUD()
    {
        if (reinforcement == 0)
        {
            //이펙트 x, 연사력 x
        }
        if (reinforcement == 1)
        {
            //이펙트 강화, 연사력 강화
            
            totalMagazine = 5;
        }
        if (reinforcement == 2)
        {
            //이펙트, 외형강화
            
            totalMagazine = 8;
        }
        if (reinforcement == 3)
        {
            //이펙트 강화, 공격력 5
            
            totalMagazine = 8;

        }
        if (reinforcement == 4)
        {
            //이펙트 강화, 외형 강화
            totalMagazine = 12;
        }
        if(reinforcement == 5)
        {
            reinforcement--;
            Debug.Log("GameStateManager - GetReinforcementItem // Already reinforcement is max");
        }
        reinforcementText.text = reinforcement.ToString();
        totalMagazineText.text = totalMagazine.ToString();
    }

}


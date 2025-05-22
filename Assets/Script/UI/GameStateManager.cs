using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using TMPro.Examples;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    // ���� ���� ������
    public Dictionary<string, bool> currentEventFlags;  // �̺�Ʈ �÷��� (��: �̺�Ʈ �Ϸ� ����)
    [SerializeField] private int currentSceneID = 0;
    [SerializeField] private int currentStagePoint = 0;     // 
    [SerializeField] private int chapterNum = 0;                           // ���� é��
    //private int Gold = 0;
    private int hp = 5;
    private EventRoot eventRoot;
    private Dictionary<string, int> sceneEnterCount = new();

    [Header("HUD Resource")]
    [SerializeField] public GameObject hudUI;
    [SerializeField] private TextMeshProUGUI totalMagazineText;
    [SerializeField] private TextMeshProUGUI reinforcementText;


    [Header("Battle Resource")]
    [SerializeField] private int totalMagazine = 5;
    private int reinforcement;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
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
        string Path = Application.dataPath + "/Resources/Json/Ver01/Dataset/Eventcondition.json";
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        if (sceneEnterCount.ContainsKey(sceneName))
            sceneEnterCount[sceneName]++;
        else
            sceneEnterCount[sceneName] = 1;

        Debug.Log($"[씬 입장] {sceneName} 입장 {sceneEnterCount[sceneName]}회");
    }

    public int GetEnterCount(string sceneName)
    {
        return sceneEnterCount.ContainsKey(sceneName) ? sceneEnterCount[sceneName] : 0;
    }

    public int GetCurrentSceneEnterCount()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (sceneEnterCount.ContainsKey(currentSceneName))
        {
            return sceneEnterCount[currentSceneName];
        }

        return 0; // 아직 한 번도 안 들어온 경우
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

    private void SetchapterNum(int chapNum)
    {
        Event events = eventRoot.Events.Find(e => e.chapterNum == chapterNum);
        if (events != null)
        {
            events.EventFlags = currentEventFlags;
        }

        chapterNum = chapNum;
    }

    public void SetNextChapterNum()
    {
        chapterNum++;
        SetchapterNum(chapterNum);
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

    public void OnOffHudUI()
    {
        hudUI.SetActive(!hudUI.activeSelf);
    }

    private IEnumerator MoveUIVerticallyDown(RectTransform target)
    {
        Vector2 startPos = target.anchoredPosition;

        Vector2 endPos = startPos + new Vector2(0f, -312f);
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;  // TimeScale 영향 없이 실행
            float t = Mathf.Clamp01(elapsed / duration);
            target.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        target.anchoredPosition = endPos;
    }
    public void StartMoveUIDown()
    {
        RectTransform target = hudUI.transform as RectTransform;

        if (target == null || target.anchoredPosition.y <= 300f)
        {
            return;
        }

        StartCoroutine(MoveUIVerticallyDown(target));
    }

    private IEnumerator MoveUIVerticallyUp(RectTransform target)
    {
        Vector2 startPos = target.anchoredPosition;

        Vector2 endPos = startPos + new Vector2(0f, +312f);
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;  // TimeScale 영향 없이 실행
            float t = Mathf.Clamp01(elapsed / duration);
            target.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        target.anchoredPosition = endPos;
    }
    public void StartMoveUIUp()
    {
        RectTransform target = hudUI.transform as RectTransform;

        if (target == null || target.anchoredPosition.y > 1.0f)
        {
            return;
        }

        StartCoroutine(MoveUIVerticallyUp(target));
    }


    //탄창수
    private void UpdateReinforcementHUD()
    {
        if (reinforcement == 0)
        {
            //이펙트 x, 연사력 x
            totalMagazine = 5;
        }
        else if (reinforcement == 1)
        {
            //이펙트 강화, 연사력 강화
            totalMagazine = 8;
        }
        else if(reinforcement == 2)
        {
            //이펙트, 외형강화
            
            totalMagazine = 12;
        }
        else if(reinforcement == 3)
        {
            //이펙트 강화, 공격력 5
            
            totalMagazine = 16;

        }
        else if(reinforcement == 4)
        {
            //이펙트 강화, 외형 강화
            totalMagazine = 20;
        }
        else if(reinforcement == 5)
        {
            totalMagazine = 30;
        }
        else if(reinforcement == 6)
        {
            reinforcement--;
            Debug.Log("GameStateManager - GetReinforcementItem // Already reinforcement is max");
        }
        reinforcementText.text = reinforcement.ToString();
        totalMagazineText.text = totalMagazine.ToString();
    }

}


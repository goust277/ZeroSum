using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using TMPro.Examples;
using System.Linq;

public class Ver0_GameStateManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static Ver0_GameStateManager Instance { get; private set; }

    // ���� ���� ������
    public Dictionary<string, bool> currentEventFlags;  // �̺�Ʈ �÷��� (��: �̺�Ʈ �Ϸ� ����)
    [SerializeField] private int currentSceneID = 0;                          // ���� �� ID
    private int chapterNum = 0;                           // ���� é��
    private int Gold = 0;
    private int hp = 100;
    private EventRoot eventRoot;

    [Header("무기강화, 탄알 조정 리소스")]
    [SerializeField] private Image HPbar;
    [SerializeField] private GameObject DamageValuePrefab;

    private void Awake()
    {
        // �̱��� ���� ����: �̹� �ν��Ͻ��� �����ϸ� �ı�, �׷��� ������ ����
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
            Debug.Log("�̹� �̺�Ʈ Ʈ���Ű� ������ ");
            return;
        }
        string Path = Application.dataPath + "/Resources/Json/Ver00/Dataset/Eventcondition.json";
        string jsonData = File.ReadAllText(Path);

        eventRoot = JsonConvert.DeserializeObject<EventRoot>(jsonData);
        Event events = eventRoot.Events.Find(e => e.chapterNum == chapterNum);
        
        currentEventFlags = events?.EventFlags;
        Debug.Log($"���� é�� EventFlags: {string.Join(", ", currentEventFlags.Select(kv => $"{kv.Key}: {kv.Value}"))}");
    }


    // ���� ������Ʈ �޼����
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

    public void getGold(int getAmount)
    {
        Gold += getAmount;
    }

    public int getCurrentGold()
    {
        return Gold;
    }

    public void getChangedHP(int fixHP)
    {
        hp -= fixHP;
        if (HPbar != null)
        {
            HPbar.fillAmount = Mathf.Clamp(hp, 0, 100) / 100f; //0~1 ���̷� Ŭ����
        }
    }

    public int getCurrentHP()
    {
        return hp;
    }


    public void spendGold(int spendAmount)
    {
        if (Gold - spendAmount < 0)
        {
            return;
        }
        else
        {
            Gold -= spendAmount;
        }
    }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class GameStateManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static GameStateManager Instance { get; private set; }

    // ���� ���� ������
    public Dictionary<string, bool> currentEventFlags;  // �̺�Ʈ �÷��� (��: �̺�Ʈ �Ϸ� ����)
    private int currentSceneID = 0;                          // ���� �� ID
    private int chapterNum = 0;                           // ���� é��
    private int Gold = 0;
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

            // WeaponManager�� ChipsetManager �ν��Ͻ� ���� + ������ �ε�

            
            // ���� �ʱ�ȭ
            currentEventFlags = new Dictionary<string, bool>();
        }
    }

    private void Start()
    {
        LoadEventFlags();

        WeaponManager.Instance.activeWeapons[0] = -1;
        WeaponManager.Instance.activeWeapons[1] = -1;

    }

    private void LoadEventFlags()
    {
        string Path = Application.dataPath + "/Resources/Json/Ver00/Dataset/Eventcondition.json";
        string jsonData = File.ReadAllText(Path);

        EventRoot eventRoot = JsonConvert.DeserializeObject<EventRoot>(jsonData);
        Event events = eventRoot.Events.Find(e => e.chapterNum == chapterNum);

        currentEventFlags = events?.EventFlags;
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class GameStateManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameStateManager Instance { get; private set; }

    // 유저 상태 변수들
    public Dictionary<string, bool> currentEventFlags;  // 이벤트 플래그 (예: 이벤트 완료 여부)
    private int currentSceneID = 0;                          // 현재 씬 ID
    private int chapterNum = 0;                           // 현재 챕터
    private int Gold = 0;
    private void Awake()
    {
        // 싱글톤 패턴 구현: 이미 인스턴스가 존재하면 파괴, 그렇지 않으면 유지
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // 씬이 바뀌어도 유지

            // WeaponManager와 ChipsetManager 인스턴스 생성 + 데이터 로드

            
            // 상태 초기화
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


    // 상태 업데이트 메서드들
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


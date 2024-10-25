using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameStateManager Instance { get; private set; }

    // 유저 상태 변수들
    public Dictionary<string, bool> currentEventFlags;  // 이벤트 플래그 (예: 이벤트 완료 여부)
    public int currentSceneID = 0;                          // 현재 씬 ID
    public int chapterNum = 0;                           // 현재 챕터

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
            //ChipsetManager chipsetManager = gameObject.AddComponent<ChipsetManager>();
            WeaponManager weaponManager = gameObject.AddComponent<WeaponManager>();
            weaponManager.activeItems[0] = 0; // 0번 칩셋
            weaponManager.activeItems[1] = 1; // 2번 칩셋
            
            // 상태 초기화
            currentEventFlags = new Dictionary<string, bool>();
        }
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

    public void SetSceneID(int sceneID)
    {
        currentSceneID = sceneID;
    }

}


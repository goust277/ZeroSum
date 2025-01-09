using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSaveFile : MonoBehaviour
{
    private string saveFile1 = "User01.json";
    private string savePath;

    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.dataPath + "/Resources/Json/Ver00/SaveFile/" + saveFile1;
    }

    void LoadBtnOnClick()
    {
        if (File.Exists(savePath)) //세이브 파일 있는 경우
        {
            LoadSaveData();
        }
    }

    void LoadSaveData()
    {
        //게임 스탯
        string Path = Application.dataPath + "/Resources/Json/Ver00/SaveFile/User01.json";
        string jsonData = File.ReadAllText(Path);
        PlayerState playerState = JsonConvert.DeserializeObject<PlayerState>(jsonData);

        int chapterNum = playerState.Version.chapter;
        //씬로드 -> 트리거 만들어놔야할듯?
        int sceneNum = playerState.Version.scene; //빌드에 있는 씬 번호로 넘기기

        //플레이어배치

        //가진 무기 리스트 목록 추가
        foreach (int item in playerState.weapons)
        {
            WeaponManager.Instance.AddAcquiredItemIds(item);
        }

        //옵션적용
        ApplySettings(playerState.settings);

        //이벤트 플래그 
        Path = Application.dataPath + "/Resources/Json/Ver00/SaveFile/User02.json";
        jsonData = File.ReadAllText(Path);

        EventRoot eventRoot = JsonConvert.DeserializeObject<EventRoot>(jsonData);
        Event events = eventRoot.Events.Find(e => e.chapterNum == chapterNum);

        GameStateManager.Instance.currentEventFlags = events?.EventFlags;
    }

    private void ApplySettings(Settings settings)
    {
        PlayerPrefs.SetInt("language", settings.language);
        PlayerPrefs.SetFloat("brightness", settings.brightness);
        PlayerPrefs.SetFloat("BackgroundVolume", settings.BackgroundVolume);
        PlayerPrefs.SetFloat("EffectsVolume", settings.EffectVolume);
        Debug.Log("옵션 값 적용 완료");
    }

}

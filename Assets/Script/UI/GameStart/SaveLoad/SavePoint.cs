using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// 클래스 정의
[Serializable]
public class PlayerState
{
    public VersionInfo Version;
    public int savePointID;
    public Position position;
    public int hp;
    public int Gold;
    public List<int> weapons;
    public Settings settings;
}

[Serializable]
public class VersionInfo
{
    public int version;
    public int chapter;
    public int scene;
    public int currentStorySceneID;
}

[Serializable]
public class Position
{
    public float x;
    public float y;
}

[Serializable]
public class Settings
{
    public int language;
    public float brightness;
    public float BackgroundVolume;
    public float EffectVolume;
}

public class SavePoint : MonoBehaviour
{
    [SerializeField] private int spID;
    private string savePath;
    private string saveFile1 = "User01.json";
    private string saveFile2 = "User02.json";
    private string formatPath;
    

    private EventRoot eventDict;

    private void Start()
    {
        savePath = Application.dataPath + "/Resources/Json/Ver00/SaveFile/";
        formatPath = Application.dataPath + "/Resources/Json/Ver00/SaveFileFormat/";
        //Debug.Log($"Save Path: {savePath}");
        //Debug.Log($"Format Path: {formatPath}");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnSaveFile();
        }
    }

    private void OnSaveFile()
    {
        CheckExistFile();

        if (eventDict == null)
        {
            Debug.Log("eventDict is null");
            return;
        }

        int currentChapNum = GameStateManager.Instance.GetChapterNum();

        // User01.json 저장 데이터 구성
        var saveData = new PlayerState
        {
            Version = new VersionInfo
            {
                version = 0,
                chapter = currentChapNum,
                scene = SceneManager.GetActiveScene().buildIndex,
                currentStorySceneID = GameStateManager.Instance.GetCurrentSceneID()
            },
            savePointID = spID,
            position = new Position
            {
                x = transform.position.x,
                y = transform.position.y
            },
            hp = GameStateManager.Instance.getCurrentHP(),
            Gold = GameStateManager.Instance.getCurrentGold(),
            weapons = WeaponManager.Instance.GetAcquiredWeapons01(),
            settings = new Settings
            {
                language = PlayerPrefs.GetInt("language", 0),
                brightness = PlayerPrefs.GetFloat("brightness", 1.0f),
                BackgroundVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1.0f),
                EffectVolume = PlayerPrefs.GetFloat("EffectsVolume", 1.0f)
            }
        };

        // User02.json 이벤트 데이터 수정
        Event existingEvent = eventDict.Events.Find(e => e.chapterNum == currentChapNum);
        if (existingEvent != null)
        {
            // 기존 플래그 업데이트 또는 추가
            foreach (var kvp in GameStateManager.Instance.currentEventFlags)
            {
                if (existingEvent.EventFlags.ContainsKey(kvp.Key))
                {
                    existingEvent.EventFlags[kvp.Key] = kvp.Value;
                }
                else
                {
                    existingEvent.EventFlags.Add(kvp.Key, kvp.Value);
                }
            }
        }
        else
        {
            Debug.LogWarning($"챕터 {currentChapNum} 데이터가 존재하지 않아 새로 추가합니다.");
            eventDict.Events.Add(new Event
            {
                chapterNum = currentChapNum,
                EventFlags = GameStateManager.Instance.currentEventFlags
            });
        }

        // 파일 저장
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        File.WriteAllText(savePath + saveFile1, JsonConvert.SerializeObject(saveData, Formatting.Indented));
        File.WriteAllText(savePath + saveFile2, JsonConvert.SerializeObject(eventDict, Formatting.Indented));

        Debug.Log($"파일 저장 완료: {savePath}");
    }

    private void CheckExistFile()
    {
        string eventFilePath = savePath + saveFile2;

        if (File.Exists(eventFilePath))
        {
            string jsonData = File.ReadAllText(eventFilePath);
            eventDict = JsonConvert.DeserializeObject<EventRoot>(jsonData);
        }
        else
        {
            string formatFilePath = formatPath + saveFile2;
            if (File.Exists(formatFilePath))
            {
                string jsonData = File.ReadAllText(formatFilePath);
                eventDict = JsonConvert.DeserializeObject<EventRoot>(jsonData);
            }
            else
            {
                Debug.LogError($"포맷 파일이 {formatFilePath} 경로에 존재하지 않습니다!");
            }
        }
    }
}
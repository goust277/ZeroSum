using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
//using System.Linq;

// Ŭ���� ����
//[Serializable]
//public class PlayerState
//{
//    public VersionInfo Version;
//    public int savePointID;
//    public Position position;
//    public int hp;
//    private int reinforcement;
//    public Settings settings;
//}

//[Serializable]
//public class VersionInfo
//{
//    public int version;
//    public int chapter;
//    public int scene;
//    private int currentSceneID;
//    public int currentStagePoint;
//}

//[Serializable]
//public class Position
//{
//    public float x;
//    public float y;
//    public float z;
//}

//[Serializable]
//public class Settings
//{
//    public int language;
//    public float brightness;
//    public float BackgroundVolume;
//    public float EffectVolume;
//}

public class SavePoint : MonoBehaviour
{
    //[SerializeField] private int spID;
    //private string savePath;
    //private string saveFile1 = "User01.json";
    //private string saveFile2 = "User02.json";
    //private string formatPath;
    

    //private EventRoot eventDict;

    //private void Start()
    //{
    //    savePath = Application.dataPath + "/Resources/Json/Ver00/SaveFile/";
    //    formatPath = Application.dataPath + "/Resources/Json/Ver00/SaveFileFormat/";
    //    //Debug.Log($"Save Path: {savePath}");
    //    //Debug.Log($"Format Path: {formatPath}");
    //}


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        OnSaveFile();
    //    }
    //}

    //private void OnSaveFile()
    //{
    //    CheckExistFile();

    //    if (eventDict == null)
    //    {
    //        Debug.Log("eventDict is null");
    //        return;
    //    }

    //    int currentChapNum = GameStateManager.Instance.GetChapterNum();

    //    // User01.json ���� ������ ����
    //    var saveData = new PlayerState
    //    {
    //        Version = new VersionInfo
    //        {
    //            version = 0,
    //            chapter = currentChapNum,
    //            scene = SceneManager.GetActiveScene().buildIndex,
    //            currentStorySceneID = GameStateManager.Instance.GetCurrentSceneID()
    //        },
    //        savePointID = spID,
    //        position = new Position
    //        {
    //            x = transform.position.x,
    //            y = transform.position.y
    //        },
    //        hp = GameStateManager.Instance.getCurrentHP(),
    //        weapons = WeaponManager.Instance.GetAcquiredWeapons01(),
    //        settings = new Settings
    //        {
    //            language = PlayerPrefs.GetInt("language", 0),
    //            brightness = PlayerPrefs.GetFloat("brightness", 1.0f),
    //            BackgroundVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1.0f),
    //            EffectVolume = PlayerPrefs.GetFloat("EffectsVolume", 1.0f)
    //        }
    //    };

    //    // User02.json �̺�Ʈ ������ ����
    //    eventDict = GameStateManager.Instance.GetEventRoot();
        //Event existingEvent = eventDict.Events.Find(e => e.chapterNum == currentChapNum);
        //Debug.Log($"���� é�� EventFlags: {string.Join(", ", GameStateManager.Instance.currentEventFlags.Select(kv => $"{kv.Key}: {kv.Value}"))}");

        //if (existingEvent != null)
        //{
        //    // ���� �÷��� ������Ʈ �Ǵ� �߰�
        //    foreach (var kvp in GameStateManager.Instance.currentEventFlags)
        //    {
        //        if (existingEvent.EventFlags.ContainsKey(kvp.Key))
        //        {
        //            existingEvent.EventFlags[kvp.Key] = kvp.Value;
        //        }
        //        else
        //        {
        //            existingEvent.EventFlags.Add(kvp.Key, kvp.Value);
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning($"é�� {currentChapNum} �����Ͱ� �������� �ʾ� ���� �߰��մϴ�.");
        //    eventDict.Events.Add(new Event
        //    {
        //        chapterNum = currentChapNum,
        //        EventFlags = GameStateManager.Instance.currentEventFlags
        //    });
        //}

        // ���� ����
        //if (!Directory.Exists(savePath))
        //{
        //    Directory.CreateDirectory(savePath);
        //}
        //File.WriteAllText(savePath + saveFile1, JsonConvert.SerializeObject(saveData, Formatting.Indented));
        //File.WriteAllText(savePath + saveFile2, JsonConvert.SerializeObject(eventDict, Formatting.Indented));

        //Debug.Log($"SavePoint - OnSaveFile //: {savePath}");
    //}

    //private void CheckExistFile()
    //{
    //    string eventFilePath = savePath + saveFile2;

    //    if (File.Exists(eventFilePath))
    //    {
    //        string jsonData = File.ReadAllText(eventFilePath);
    //        eventDict = JsonConvert.DeserializeObject<EventRoot>(jsonData);
    //    }
    //    else
    //    {
    //        string formatFilePath = formatPath + saveFile2;
    //        if (File.Exists(formatFilePath))
    //        {
    //            string jsonData = File.ReadAllText(formatFilePath);
    //            eventDict = JsonConvert.DeserializeObject<EventRoot>(jsonData);
    //        }
    //        else
    //        {
    //            Debug.LogError($"SavePoint - CheckExistFile // {formatFilePath} << already existtttttt");
    //        }
    //    }
    //}
}
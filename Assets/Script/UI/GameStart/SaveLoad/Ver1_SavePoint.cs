using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
//using System.Linq;

// Ŭ���� ����
[Serializable]
public class PlayerState
{
    public VersionInfo Version;
    public Position position;
    public int hp;
    private int reinforcement;
    public Settings settings;
}

[Serializable]
public class VersionInfo
{
    public int version;
    public int chapter;
    public int scene;
    public int currentSceneID;
    public int currentStagePoint;
}

[Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class Settings
{
    public float BackgroundVolume;
    public float EffectVolume;
}

public class Ver1_SavePoint : MonoBehaviour
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

        // User01.json ���� ������ ����
        var saveData = new PlayerState
        {
            Version = new VersionInfo
            {
                version = 0,
                chapter = currentChapNum,
                scene = SceneManager.GetActiveScene().buildIndex,
                currentSceneID = GameStateManager.Instance.GetCurrentSceneID(),
                currentStagePoint = GameStateManager.Instance.GetStagePoint()
            },
            position = new Position
            {
                x = transform.position.x,
                y = transform.position.y
            },
            hp = Ver01_DungeonStatManager.Instance.GetMaxHP(),
            settings = new Settings
            {
                BackgroundVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1.0f),
                EffectVolume = PlayerPrefs.GetFloat("EffectsVolume", 1.0f)
            }
        };

        eventDict = GameStateManager.Instance.GetEventRoot();


        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        File.WriteAllText(savePath + saveFile1, JsonConvert.SerializeObject(saveData, Formatting.Indented));
        File.WriteAllText(savePath + saveFile2, JsonConvert.SerializeObject(eventDict, Formatting.Indented));

        Debug.Log($"SavePoint - OnSaveFile //: {savePath}");
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
                Debug.LogError($"SavePoint - CheckExistFile // {formatFilePath} << already existtttttt");
            }
        }
    }
}
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
    //private string saveFile1 = "User01.json";
    private string savePath;
    private readonly string savePath1 = Application.dataPath + "/Resources/Json/Ver00/SaveFile/User01.json";
    private readonly string savePath2 = Application.dataPath + "/Resources/Json/Ver00/SaveFile/User02.json";

    // ���̺� �����͸� �ҷ����� �޼���
    public PlayerState LoadPlayerState()
    {
        if (!File.Exists(savePath1))
        {
            Debug.LogWarning("User01.json ���̺� ������ �������� �ʽ��ϴ�!");
            return null;
        }

        string jsonData = File.ReadAllText(savePath1);
        return JsonConvert.DeserializeObject<PlayerState>(jsonData);
    }

    public EventRoot LoadEventRoot()
    {
        if (!File.Exists(savePath2))
        {
            Debug.LogWarning("User02.json ���̺� ������ �������� �ʽ��ϴ�!");
            return null;
        }

        string jsonData = File.ReadAllText(savePath2);
        return JsonConvert.DeserializeObject<EventRoot>(jsonData);
    }


}

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
        if (File.Exists(savePath)) //���̺� ���� �ִ� ���
        {
            LoadSaveData();
        }
    }

    void LoadSaveData()
    {
        //���� ����
        string Path = Application.dataPath + "/Resources/Json/Ver00/SaveFile/User01.json";
        string jsonData = File.ReadAllText(Path);
        PlayerState playerState = JsonConvert.DeserializeObject<PlayerState>(jsonData);

        int chapterNum = playerState.Version.chapter;
        //���ε� -> Ʈ���� ���������ҵ�?
        int sceneNum = playerState.Version.scene; //���忡 �ִ� �� ��ȣ�� �ѱ��

        //�÷��̾��ġ

        //���� ���� ����Ʈ ��� �߰�
        foreach (int item in playerState.weapons)
        {
            WeaponManager.Instance.AddAcquiredItemIds(item);
        }

        //�ɼ�����
        ApplySettings(playerState.settings);

        //�̺�Ʈ �÷��� 
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
        Debug.Log("�ɼ� �� ���� �Ϸ�");
    }

}

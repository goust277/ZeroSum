using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static GameStateManager Instance { get; private set; }

    // ���� ���� ������
    public Dictionary<string, bool> currentEventFlags;  // �̺�Ʈ �÷��� (��: �̺�Ʈ �Ϸ� ����)
    public int currentSceneID = 0;                          // ���� �� ID
    public int chapterNum = 0;                           // ���� é��

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
            //ChipsetManager chipsetManager = gameObject.AddComponent<ChipsetManager>();
            WeaponManager weaponManager = gameObject.AddComponent<WeaponManager>();
            weaponManager.activeItems[0] = 0; // 0�� Ĩ��
            weaponManager.activeItems[1] = 1; // 2�� Ĩ��
            
            // ���� �ʱ�ȭ
            currentEventFlags = new Dictionary<string, bool>();
        }
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

    public void SetSceneID(int sceneID)
    {
        currentSceneID = sceneID;
    }

}


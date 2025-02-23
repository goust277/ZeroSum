using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Linq;


public class GameStartController : BaseUi
{
    [SerializeField] private Image Panel;
    float currentTime = 0.0f;  //���� �ð�
    private readonly float fadeoutTime = 2.0f;  //���̵�ƿ��� ����� �ð�
    private bool isSettingOpen = false;
    private SettingsManager settingsManager;
    private readonly string savePath1 = Application.dataPath + "/Resources/Json/Ver00/SaveFile/User01.json";

    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Start()
    {
        base.Start();
        //Panel.gameObject.SetActive(false);
    }

    protected override void ButtonFuncion(string btnName)
    {
        base.ButtonFuncion(btnName);
        switch (btnName)
        {
            case "NewStart":
                Debug.Log("NewStart");
                NewStart();
                break;
            case "Continue":
                Debug.Log("Continue");
                LoadBtnOnClick();
                break;
            case "Setting":
                SettingListener();
                Debug.Log("Setting");
                break;
            case "Exit":
                Debug.Log("Exit");
                GameQuit();
                break;
            case "SaveNClose":
                SettingListener();
                Debug.Log("SaveNClose");
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isSettingOpen)
        {
            isSettingOpen = false;
            //SettingsManager.Instance.SettingOnOff();
        }
    }

    private void SettingListener()
    {
        settingsManager ??= FindObjectsOfType<SettingsManager>(true).FirstOrDefault();

        settingsManager.SettingOnOff();
    }

    private void LoadBtnOnClick()
    {
        if (!File.Exists(savePath1))
        {
            Debug.LogWarning("User01.json ���̺� ������ �������� �ʽ��ϴ�!");
        }
        else
        {
            StartCoroutine(fadeOut());
        }
    }

    private IEnumerator fadeOut()
    {
        Panel.gameObject.SetActive(true);
        Color alpha = Panel.color;
        while (alpha.a < 1)
        {
            currentTime += Time.deltaTime / fadeoutTime;
            alpha.a = Mathf.Lerp(0, 1, currentTime);
            Panel.color = alpha;
            yield return null;
        }
        StartCoroutine(LoadSceneCoroutine("BootScene"));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // �ε� ����� ���
            Debug.Log("Loading progress: " + progress * 100 + "%");

            // �ε��� ���� ������ ���
            yield return null;
        }

        // �ε��� �Ϸ�� �� �߰� �۾�
        SceneManager.LoadScene(sceneName);
    }


    private void NewStart()
    {
        StartCoroutine(fadeOut());
    }

    private void GameQuit()
    {
        Application.Quit();
    }

}

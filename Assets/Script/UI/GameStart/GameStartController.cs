using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Linq;


public class GameStartController : BaseUi
{

    [Header("오디오")]
    [SerializeField] private AudioSource audioSource;

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
        settingsManager ??= FindObjectsOfType<SettingsManager>(true).FirstOrDefault();

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
        }
    }

    private void SettingListener()
    {
        ClickSount();

        settingsManager.SettingOnOff();
    }

    private void LoadBtnOnClick()
    {
        ClickSount();
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
        SceneManager.LoadScene("Lobby");
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
        ClickSount();
        StartCoroutine(fadeOut());
    }

    private void GameQuit()
    {
        ClickSount();
        Application.Quit();
    }

    private void ClickSount()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;


public class GameStartController : BaseUi
{
    [SerializeField] private Image Panel;
    float currentTime = 0.0f;  //현재 시간
    private readonly float fadeoutTime = 2.0f;  //페이드아웃이 진행될 시간
    private bool isSettingOpen = false;


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
                Debug.Log("NewGameStart");
                NewStart();
                break;
            case "Continue":
                Debug.Log("Continue");
                break;
            case "Setting":
                isSettingOpen = true;
                //SettingsManager.Instance.SettingOnOff();
                Debug.Log("Setting");
                break;
            case "Exit":
                Debug.Log("Exit");
                GameQuit();
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
        StartCoroutine(LoadSceneCoroutine("SampleSceneUI"));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 로딩 진행률 계산
            Debug.Log("Loading progress: " + progress * 100 + "%");

            // 로딩이 끝날 때까지 대기
            yield return null;
        }

        // 로딩이 완료된 후 추가 작업
        SceneManager.LoadScene("SampleSceneUI");
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

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
    float fadeoutTime = 2.0f;  //페이드아웃이 진행될 시간

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
                SettingsManager.Instance.SettingOnOff();
                Debug.Log("Setting");
                break;
            case "Exit":
                Debug.Log("Exit");
                GameQuit();
                break;
        }
    }


    IEnumerator fadeOut()
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
        SceneManager.LoadScene(4);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStartController : BaseUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Start()
    {
        base.Start();
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
                Debug.Log("Setting");
                break;
            case "Exit":
                Debug.Log("Exit");
                break;
        }
    }

    private void NewStart()
    {
        SceneManager.LoadScene(0);
    }

}

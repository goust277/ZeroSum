using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadSetting : MonoBehaviour
{
    public bool isMissionStart = false;
    public bool isMissionClear = false;

    [Header("mission")]
    [SerializeField] private GameObject missionTimeDisplay;


    void Start()
    {
        GameStateManager.Instance.StartMoveUIDown();   
    }

    void Update()
    {
        if (isMissionStart)
        {
            if (!missionTimeDisplay.activeSelf)
                missionTimeDisplay.SetActive(true);
        }

        if (isMissionClear)
        {
            if (missionTimeDisplay.activeSelf)
                missionTimeDisplay.SetActive(false);
        }
    }
}

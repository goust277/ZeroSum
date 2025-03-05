using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;

public class TempOnOff : MonoBehaviour
{
    [SerializeField] private Button optionSettingbtn;
    [SerializeField] private SettingsManager optionSettingUI; // GameObject → SettingsManager로 변경

    void Start()
    {
        optionSettingbtn.onClick.AddListener(SettingOnOff);
    }

    public void SettingOnOff()
    {
        optionSettingUI.SettingOnOff();
    }
}


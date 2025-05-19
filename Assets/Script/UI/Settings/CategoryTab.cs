using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용

public class CategoryTab : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("ActionBtns")]
    [SerializeField] private Button optionSettingBtn; // UI 버튼
    [SerializeField] private Button keySettingBtn;
    [SerializeField] private Button onOffBtn;

    [Header("Objects")]
    [SerializeField] private GameObject wholeCanvas;
    [SerializeField] private GameObject optionSettingUI;
    [SerializeField] private GameObject keySettingUI;

    void Start()
    {
        onOffBtn.onClick.AddListener(SettingOff);
        keySettingBtn.onClick.AddListener(KeySettingOnOff);
        optionSettingBtn.onClick.AddListener(OptionSettingOnOff);

        keySettingUI.SetActive(true);
        optionSettingUI.SetActive(false);
    }
    public void SettingOff()
    {
        bool newState = wholeCanvas.gameObject.activeSelf;

        if (newState)
        {
            wholeCanvas.gameObject.SetActive(false);
        }
    }

    public void KeySettingOnOff() {
        audioSource.Play();

        if (keySettingUI.activeSelf) return;

        keySettingUI.SetActive(true);
        if (optionSettingUI.activeSelf)
        {
            optionSettingUI.SetActive(false);
        }
    }
    public void OptionSettingOnOff() {
        audioSource.Play();

        if (optionSettingUI.activeSelf) return;

        optionSettingUI.SetActive(true);
        if (keySettingUI.activeSelf)
        {
            keySettingUI.SetActive(false);
        }
    }

}

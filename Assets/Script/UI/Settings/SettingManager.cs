using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;  // ����� ���� ����
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;
//using UnityEngine.Rendering.PostProcessing;  // ��� ���� ���� ó���� ����

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("UI menu Resource")]
    [SerializeField] private Button resolutionButton;  // �ػ� ���� ��ư
    [SerializeField] private Button fullscreenButton;  // ��üȭ�� ��ư
    [SerializeField] private Slider backgroundSoundSlider;  // ����� �����̴�
    [SerializeField] private Slider effectsSoundSlider;  // ȿ���� �����̴�
    [SerializeField] private Button vibrationButtons;   // ȭ�� ���� ��ư�� (5��)

    [Header("Option  Resource")]
    [SerializeField] private TextMeshProUGUI[] textMeshPros = new TextMeshProUGUI[4];
    // ����� �ͼ� (�����, ȿ����)
    public AudioMixer audioMixer;  // ������� ȿ������ �����ϴ� �ͼ�

    //default values
    private const int defaultVibrationLevel = 2;
    private const int defaultResolutionIndex = 0;
    private const bool defaultisFullscreen = true;
    private const float defaultEffectsVolume = 0.5f;
    private const float defaultBackgroundVolume = 0.5f;


    private List<ResolutionData> resolutionValues = new();
    // variable values
    private int vibrationLevel = 2;
    private int resolutionIndex = 0;
    private bool isFullscreen ;
    private bool isSettingOpen= true;

    [SerializeField] private Button optionResetbtn;
    [SerializeField] private GameObject optionSettingUI;

    //[SerializeField] private TextMeshProUGUI DebugTemp;
    //[SerializeField] private Image brightnessPanel;

    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        HashSet<string> resolutionSet = new HashSet<string>(); // �ߺ� üũ�� ���� Set

        foreach (Resolution resolution in resolutions)
        {
            // refreshRate�� int Ÿ���̹Ƿ�, �̸� float�� ��ȯ�Ͽ� ���
            float refreshRate = resolution.refreshRate;

            // refreshRateRatio�� �Ҽ��� ��° �ڸ��� �ݿø��Ͽ� ��
            string resolutionKey = $"{resolution.width}x{resolution.height}"; // �Ҽ��� ��° �ڸ����� �ݿø�

            // 16:9 ������ �ּ� �ʺ� 1024 ������ �����ϰ�, �ߺ��� �ػ󵵰� �ƴϸ� �߰�
            if (resolution.width >= 1024 &&
                Mathf.Approximately((float)resolution.width / resolution.height, 16.0f / 9.0f) &&
                !resolutionSet.Contains(resolutionKey)) // Set�� ������ �߰�
            {
                resolutionSet.Add(resolutionKey); // �ߺ� ������ ���� Ű �߰�
                resolutionValues.Add(new ResolutionData(resolution.width, resolution.height, resolution.refreshRateRatio));
                //DebugTemp.text += "new ResolutionData(" + resolution.width + ", " + resolution.height + ", " + resolution.refreshRateRatio + ")"+ "\n";
            }
        }

        isFullscreen = Screen.fullScreen;
        //ToggleFullscreen();
        //Screen.SetResolution(1920, 1080, true);
        //childTransform = gameObject.transform.GetChild(0);


        SettingOnOff();

        // �� UI ��ҿ� �̺�Ʈ ������ �߰�
        resolutionButton.onClick.AddListener(SetResolution);
        fullscreenButton.onClick.AddListener(ToggleFullscreen);
        //optionSettingbtn.onClick.AddListener(SettingOnOff);
        optionResetbtn.onClick.AddListener(OnClickResetBtn);

        // 1. 먼저 값 세팅
        backgroundSoundSlider.value = PlayerPrefs.GetFloat("BackgroundVolume", defaultBackgroundVolume);
        effectsSoundSlider.value = PlayerPrefs.GetFloat("EffectsVolume", defaultEffectsVolume);
        vibrationLevel = PlayerPrefs.GetInt("VibrationLevel", defaultVibrationLevel);

        // 2. 그 다음에 이벤트 연결
        backgroundSoundSlider.onValueChanged.AddListener(SetBackgroundSound);
        effectsSoundSlider.onValueChanged.AddListener(SetEffectsSound);
        vibrationButtons.onClick.AddListener(SetVibration);
    }

    private void OnClickResetBtn()
    {
        audioSource.Play();
        backgroundSoundSlider.value = defaultBackgroundVolume;
        SetBackgroundSound(defaultBackgroundVolume);

        effectsSoundSlider.value = defaultEffectsVolume;
        SetEffectsSound(defaultEffectsVolume);

        vibrationLevel = defaultVibrationLevel;
        string[] vibrationLevels = { "x0.3", "x0.7", "x1.0", "x1.3", "x1.6" };
        PlayerPrefs.SetInt("VibrationLevel", defaultVibrationLevel);  // ����
        textMeshPros[2].text = vibrationLevels[defaultVibrationLevel];

        textMeshPros[1].text = "켜짐";
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        isFullscreen = Screen.fullScreen;

        int width = 1920; // ����
        int height = 1080; // ����
        textMeshPros[0].text = $"{width} X {height}";

    }


    public void SettingOnOff()
    {
        bool newState = !optionSettingUI.gameObject.activeSelf;

        if (!isSettingOpen)
        {
            Debug.Log("창열림");
            optionSettingUI.gameObject.SetActive(newState);
            isSettingOpen = true;
        }
        else
        {
            Debug.Log("창닫김");
            optionSettingUI.gameObject.SetActive(newState);
            isSettingOpen = false;
        }
    }
    // �ػ󵵸� �����ϴ� �޼���
    private void SetResolution()
    {
        audioSource.Play();
        resolutionIndex--;

        if (resolutionIndex < 0 )
        {
            resolutionIndex = resolutionValues.Count-1;
        }
        //else if (resolutionIndex == resolutionValues.Count)
        //{
        //    resolutionIndex = 0;
        //}

        int width = resolutionValues[resolutionIndex].Width; // ����
        int height = resolutionValues[resolutionIndex].Height; // ����
        textMeshPros[0].text = $"{width} X {height}";
        Screen.SetResolution(width, height, isFullscreen);
        //DebugTemp.text += $"Resolution set to: {Screen.width}x{Screen.height}, Fullscreen: {Screen.fullScreen}";
        //DebugTemp.text += "\n";
    }

    // ��üȭ�� ���
    private void ToggleFullscreen()
    {
        audioSource.Play();
        //bool isFullscreen = Screen.fullScreen;

        isFullscreen = Screen.fullScreen;
        if (!isFullscreen) {
            textMeshPros[1].text = "켜짐";
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            textMeshPros[1].text = "꺼짐";
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        isFullscreen = !Screen.fullScreen;

        Screen.SetResolution(Screen.width, Screen.height, isFullscreen);  
        //DebugTemp.text += $"Is Fullscreen: {isFullscreen}, FullScreenMode: {Screen.fullScreenMode}, Resolution: {Screen.currentResolution.width}x{Screen.currentResolution.height}";
        //DebugTemp.text += "\n";
    }

    // ����� ����
    private void SetBackgroundSound(float value)
    {
        audioSource.Play();
        float volume = Mathf.Approximately(value, 0.0f) ? -80f : Mathf.Log10(value) * 20;
        int mappedValue = Mathf.RoundToInt((value) * (100f));
        textMeshPros[3].text = mappedValue.ToString();
        audioMixer.SetFloat("BackgroundVolume", Mathf.Max(volume, -80));
        PlayerPrefs.SetFloat("BackgroundVolume", value);
    }

    // ȿ���� ����
    private void SetEffectsSound(float value)
    {
        audioSource.Play();
        float volume = Mathf.Approximately(value, 0.0f) ? -80f : Mathf.Log10(value) * 20;
        int mappedValue = Mathf.RoundToInt((value) * (100f));
        textMeshPros[4].text = mappedValue.ToString();
        audioMixer.SetFloat("EffectsVolume", Mathf.Max(volume, -80));
        PlayerPrefs.SetFloat("EffectsVolume", value);
    }

    private void SetVibration()
    {
        audioSource.Play();
        vibrationLevel--;

        string[] vibrationLevels = { "x0.3", "x0.7", "x1.0", "x1.3", "x1.6" };

        if (vibrationLevel < 0)
        {
            vibrationLevel = vibrationLevels.Length - 1;
        }

        PlayerPrefs.SetInt("VibrationLevel", vibrationLevel);  // ����
        textMeshPros[2].text = vibrationLevels[vibrationLevel];

        //화면떨림 조정?
    }


}

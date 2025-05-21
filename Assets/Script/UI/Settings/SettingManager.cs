using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;
//using UnityEngine.Rendering.PostProcessing; 

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("UI menu Resource")]
    [SerializeField] private Button resolutionButton; 
    [SerializeField] private Button fullscreenButton; 
    [SerializeField] private Slider backgroundSoundSlider; 
    [SerializeField] private Slider effectsSoundSlider;
    [SerializeField] private Button vibrationButtons;

    [Header("Option  Resource")]
    [SerializeField] private TextMeshProUGUI[] textMeshPros = new TextMeshProUGUI[4];

    public AudioMixer audioMixer; 
    //default values
    //private const int defaultVibrationLevel = 2;
    private const int defaultResolutionIndex = 0;
    private const bool defaultisFullscreen = true;
    private const float defaultBackgroundVolume = 1f;
    private const float defaultEffectsVolume = 1f;


    private List<ResolutionData> resolutionValues = new();
    // variable values
    private int vibrationLevel = 2;
    private int resolutionIndex = 0;
    private bool isFullscreen ;
    private bool isSettingOpen= true;

    [SerializeField] private Button optionResetbtn;
    [SerializeField] private GameObject optionSettingUI;

    private const string BG_KEY = "BackgroundVolume";
    private const string EF_KEY = "EffectsVolume";

    //[SerializeField] private TextMeshProUGUI DebugTemp;
    //[SerializeField] private Image brightnessPanel;
    private void Awake()
    {
        // PlayerPrefs에서 슬라이더 값 복원 (0.0 ~ 1.0)
        float bgValue = PlayerPrefs.GetFloat(BG_KEY, defaultBackgroundVolume);
        float efValue = PlayerPrefs.GetFloat(EF_KEY, defaultEffectsVolume);

        // AudioMixer에 dB 적용 + 텍스트 업데이트
        SetVolume("BackgroundVolume", BG_KEY, bgValue, textMeshPros[3]);
        SetVolume("EffectsVolume", EF_KEY, efValue, textMeshPros[4]);

        // 슬라이더 값 설정 (이벤트 안 태우고)
        backgroundSoundSlider.SetValueWithoutNotify(bgValue);
        effectsSoundSlider.SetValueWithoutNotify(efValue);
    }

    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        HashSet<string> resolutionSet = new HashSet<string>();

        foreach (Resolution resolution in resolutions)
        {
            float refreshRate = resolution.refreshRate;

            string resolutionKey = $"{resolution.width}x{resolution.height}";
            if (resolution.width >= 1024 &&
                Mathf.Approximately((float)resolution.width / resolution.height, 16.0f / 9.0f) &&
                !resolutionSet.Contains(resolutionKey))
            {
                resolutionSet.Add(resolutionKey);
                resolutionValues.Add(new ResolutionData(resolution.width, resolution.height, resolution.refreshRateRatio));
                //DebugTemp.text += "new ResolutionData(" + resolution.width + ", " + resolution.height + ", " + resolution.refreshRateRatio + ")"+ "\n";
            }
        }

        isFullscreen = Screen.fullScreen;

        SettingOnOff();

        //resolutionButton.onClick.AddListener(SetResolution);
        fullscreenButton.onClick.AddListener(ToggleFullscreen);
        //optionSettingbtn.onClick.AddListener(SettingOnOff);
        optionResetbtn.onClick.AddListener(OnClickResetBtn);

        // 2. 그 다음에 이벤트 연결
        backgroundSoundSlider.onValueChanged.AddListener(
            value => SetVolume("BackgroundVolume", BG_KEY, value, textMeshPros[3])
        );

        effectsSoundSlider.onValueChanged.AddListener(
            value => SetVolume("EffectsVolume", EF_KEY, value, textMeshPros[4])
        );

        vibrationButtons.onClick.AddListener(SetVibration);
    }

    private void SetVolume(string mixerParamName, string prefsKey, float value, TextMeshProUGUI displayText)
    {
        float volume = Mathf.Approximately(value, 0.0f) ? -80f : Mathf.Log10(value) * 20f;
        audioMixer.SetFloat(mixerParamName, Mathf.Max(volume, -80f));
        PlayerPrefs.SetFloat(prefsKey, value);

        if (displayText != null)
        {
            int mappedValue = Mathf.RoundToInt(value * 100f);
            displayText.text = mappedValue.ToString();
        }
    }


    private void OnClickResetBtn()
    {
        audioSource.Play();

        // 배경음 초기화
        backgroundSoundSlider.value = defaultBackgroundVolume;
        SetVolume("BackgroundVolume", BG_KEY, defaultBackgroundVolume, textMeshPros[3]);

        // 효과음 초기화
        effectsSoundSlider.value = defaultEffectsVolume;
        SetVolume("EffectsVolume", EF_KEY, defaultEffectsVolume, textMeshPros[4]);

        // 진동 초기화
        //vibrationLevel = defaultVibrationLevel;
        //string[] vibrationLevels = { "x0.3", "x0.7", "x1.0", "x1.3", "x1.6" };
        //PlayerPrefs.SetInt("VibrationLevel", defaultVibrationLevel);
        //textMeshPros[2].text = vibrationLevels[defaultVibrationLevel];

        // 전체화면 초기화
        textMeshPros[1].text = "켜짐";
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        isFullscreen = Screen.fullScreen;

        // 해상도 초기화
        //int width = 1920;
        //int height = 1080;
        //textMeshPros[0].text = $"{width} X {height}";
        //Screen.SetResolution(width, height, isFullscreen);
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

        int width = resolutionValues[resolutionIndex].Width;
        int height = resolutionValues[resolutionIndex].Height;
        textMeshPros[0].text = $"{width} X {height}";
        Screen.SetResolution(width, height, isFullscreen);
        //DebugTemp.text += $"Resolution set to: {Screen.width}x{Screen.height}, Fullscreen: {Screen.fullScreen}";
        //DebugTemp.text += "\n";
    }

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
    private void SetVibration()
    {
        audioSource.Play();
        vibrationLevel--;

        string[] vibrationLevels = { "x0.3", "x0.7", "x1.0", "x1.3", "x1.6" };

        if (vibrationLevel < 0)
        {
            vibrationLevel = vibrationLevels.Length - 1;
        }

        PlayerPrefs.SetInt("VibrationLevel", vibrationLevel);
        textMeshPros[2].text = vibrationLevels[vibrationLevel];

        //화면떨림 조정?
    }

    public void playEffectSound()
    {
        audioSource.Play();
    }

}

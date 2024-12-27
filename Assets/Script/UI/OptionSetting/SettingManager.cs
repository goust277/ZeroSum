using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;  // 오디오 관련 제어
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;
//using UnityEngine.Rendering.PostProcessing;  // 언어 설정 관련 처리를 위해

public class SettingsManager : MonoBehaviour
{
    // UI 요소 연결
    [SerializeField] private Button[] languageButton = new Button[2];  // 언어 선택 버튼
    [SerializeField] private Button[] resolutionButton = new Button[2];  // 해상도 변경 버튼
    [SerializeField] private Button[] fullscreenButton = new Button[2];  // 전체화면 버튼
    [SerializeField] private Slider brightnessSlider;  // 화면 밝기 슬라이더
    [SerializeField] private Slider backgroundSoundSlider;  // 배경음 슬라이더
    [SerializeField] private Slider effectsSoundSlider;  // 효과음 슬라이더
    [SerializeField] private Button[] vibrationButtons = new Button[5];   // 화면 진동 버튼들 (5개)
    [SerializeField] private TextMeshProUGUI[] textMeshPros = new TextMeshProUGUI[4];
    // 오디오 믹서 (배경음, 효과음)
    public AudioMixer audioMixer;  // 배경음과 효과음을 관리하는 믹서


    // 화면 진동 설정
    private int vibrationLevel = 0;

    // 변경 리스트
    private string[] language = new string[2];
    private List<ResolutionData> resolutionValues = new();

    [SerializeField] private int resolutionIndex = 0;
    private int languageIndex = 0;
    private bool isFullscreen ;
    private bool isSettingOpen= true;
    private Transform childTransform;

    [SerializeField] private TextMeshProUGUI DebugTemp;
    [SerializeField] private Image brightnessPanel;

    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        HashSet<string> resolutionSet = new HashSet<string>(); // 중복 체크를 위한 Set

        foreach (Resolution resolution in resolutions)
        {
            // refreshRate는 int 타입이므로, 이를 float로 변환하여 사용
            float refreshRate = resolution.refreshRate;

            // refreshRateRatio를 소수점 둘째 자리로 반올림하여 비교
            string resolutionKey = $"{resolution.width}x{resolution.height}"; // 소수점 둘째 자리까지 반올림

            // 16:9 비율과 최소 너비 1024 조건을 만족하고, 중복된 해상도가 아니면 추가
            if (resolution.width >= 1024 &&
                Mathf.Approximately((float)resolution.width / resolution.height, 16.0f / 9.0f) &&
                !resolutionSet.Contains(resolutionKey)) // Set에 없으면 추가
            {
                resolutionSet.Add(resolutionKey); // 중복 방지를 위해 키 추가
                resolutionValues.Add(new ResolutionData(resolution.width, resolution.height, resolution.refreshRateRatio));
                DebugTemp.text += "new ResolutionData(" + resolution.width + ", " + resolution.height + ", " + resolution.refreshRateRatio + ")"+ "\n";
            }
        }

        isFullscreen = Screen.fullScreen;
        ToggleFullscreen();
        //Screen.SetResolution(1920, 1080, true);
        childTransform = gameObject.transform.GetChild(0);

        language[0] = "한국어";
        language[1] = "준비중";

        SettingOnOff();

        // 각 UI 요소에 이벤트 리스너 추가
        languageButton[0].onClick.AddListener(ChangeLanguage);
        languageButton[1].onClick.AddListener(ChangeLanguage);

        resolutionButton[0].onClick.AddListener(() => SetResolution(0));
        resolutionButton[1].onClick.AddListener(() => SetResolution(1));

        fullscreenButton[0].onClick.AddListener(ToggleFullscreen);
        fullscreenButton[1].onClick.AddListener(ToggleFullscreen);

        //brightnessSlider.onValueChanged.AddListener(SetBrightness);
        backgroundSoundSlider.onValueChanged.AddListener(SetBackgroundSound);
        effectsSoundSlider.onValueChanged.AddListener(SetEffectsSound);

        // 진동 버튼들에 이벤트 리스너 추가
        for (int i = 0; i < vibrationButtons.Length; i++)
        {
            int index = i;
            vibrationButtons[i].onClick.AddListener(() => SetVibration(index));
        }


        // 초기 값 설정 (슬라이더 및 버튼 초기화)
        //brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);  // 기본값 1.0
        backgroundSoundSlider.value = PlayerPrefs.GetFloat("BackgroundVolume", 1.0f);
        effectsSoundSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1.0f);
        vibrationLevel = PlayerPrefs.GetInt("VibrationLevel", 0);

        // 슬라이더 초기화
        brightnessSlider.minValue = 0f;
        brightnessSlider.maxValue = 0.6f;

        // 슬라이더 값 변경 이벤트 등록
        brightnessSlider.onValueChanged.AddListener(SetBrightness);

        // 슬라이더의 초기값과 패널의 투명도 동기화
        brightnessSlider.value = brightnessPanel.color.a;

    }


    public void SettingOnOff()
    {
        bool newState = !childTransform.gameObject.activeSelf;

        if (!isSettingOpen)
        {
            childTransform.gameObject.SetActive(newState);
            isSettingOpen = true;
        }
        else
        {
            childTransform.gameObject.SetActive(newState);
            isSettingOpen = false;
        }
    }


    // 언어 변경
    private void ChangeLanguage()
    {
        // 언어 변경 로직 (예: 한국어, 영어로 변경)
        // 예시로 단순히 씬을 리로드 하는 방식으로 구현할 수 있습니다.

        languageIndex = Math.Abs(languageIndex - 1);
        textMeshPros[0].text = language[languageIndex];

        //string currentScene = SceneManager.GetActiveScene().name;
        //SceneManager.LoadScene(currentScene);  // 씬 리로드로 언어 변경 적용
    }

    // 해상도 변경
    // 해상도를 설정하는 메서드
    private void SetResolution(int b)
    {
        if (b == 0)
        {
            resolutionIndex -= 1;
        }
        else
        {
            resolutionIndex += 1;
        }

        if (resolutionIndex < 0 )
        {
            resolutionIndex = resolutionValues.Count-1;
        }
        else if (resolutionIndex == resolutionValues.Count)
        {
            resolutionIndex = 0;
        }

        int width = resolutionValues[resolutionIndex].Width; // 가로
        int height = resolutionValues[resolutionIndex].Height; // 세로
        textMeshPros[1].text = $"{width} X {height}";
        Screen.SetResolution(width, height, isFullscreen);
        DebugTemp.text += $"Resolution set to: {Screen.width}x{Screen.height}, Fullscreen: {Screen.fullScreen}";
        DebugTemp.text += "\n";
    }

    // 전체화면 토글
    private void ToggleFullscreen()
    {
        //bool isFullscreen = Screen.fullScreen;

        if (!isFullscreen) {
            textMeshPros[2].text = "켜짐";
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            textMeshPros[2].text = "꺼짐";
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        isFullscreen = !Screen.fullScreen;

        Screen.SetResolution(Screen.width, Screen.height, isFullscreen);  // 현재 해상도를 유지하며 전체화면/창모드 전환
        DebugTemp.text += $"Is Fullscreen: {isFullscreen}, FullScreenMode: {Screen.fullScreenMode}, Resolution: {Screen.currentResolution.width}x{Screen.currentResolution.height}";
        DebugTemp.text += "\n";
    }

    // 화면 밝기 설정
    public void SetBrightness(float value)
    {
        // Clamp를 사용하여 값이 0과 0.6 사이를 벗어나지 않도록 보장
        float clampedValue = Mathf.Clamp(value, 0f, 0.6f);

        // 패널 이미지의 컬러 값 업데이트
        Color panelColor = brightnessPanel.color;
        panelColor.a = 0.6f - clampedValue;
        brightnessPanel.color = panelColor;

        // 밝기 조정 (간단한 방법은 색상 매트릭스를 사용하거나 카메라의 밝기 조정)
        //RenderSettings.ambientLight = new Color(value, value, value);  // 밝기 조정 (컬러 값)
        PlayerPrefs.SetFloat("Brightness", clampedValue);  // 저장
    }

    // 배경음 설정
    private void SetBackgroundSound(float value)
    {
        float volume = Mathf.Approximately(value, 0.0f) ? -80f : Mathf.Log10(value) * 20;
        audioMixer.SetFloat("BackgroundVolume", Mathf.Max(volume, -80));
        PlayerPrefs.SetFloat("BackgroundVolume", value);
    }

    // 효과음 설정
    private void SetEffectsSound(float value)
    {
        float volume = Mathf.Approximately(value, 0.0f) ? -80f : Mathf.Log10(value) * 20;
        audioMixer.SetFloat("EffectsVolume", Mathf.Max(volume, -80));
        PlayerPrefs.SetFloat("EffectsVolume", value);
    }

    // 화면 진동 설정
    private void SetVibration(int level)
    {
        vibrationLevel = level;
        PlayerPrefs.SetInt("VibrationLevel", vibrationLevel);  // 저장

        string[] vibrationLevels = { "x0.3", "x0.7", "x1.0", "x1.3", "x1.6" };
        textMeshPros[3].text = vibrationLevels[level];
    }
}

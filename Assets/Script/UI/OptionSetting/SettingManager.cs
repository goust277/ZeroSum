using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;  // 오디오 관련 제어
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;  // 언어 설정 관련 처리를 위해

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

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
    private bool isFullscreen = true;
    private bool isSettingOpen= true;
    private Transform childTransform;
    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스 제거
        }
    }

    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            if (resolution.width >= 1024 && Mathf.Approximately((float)resolution.width/resolution.height, 16.0f / 9.0f))
            {
                resolutionValues.Add(new ResolutionData(resolution.width, resolution.height, resolution.refreshRateRatio));
                Debug.Log("new ResolutionData(" + resolution.width + " ,"  + resolution.height + ")");
            }

        }


        childTransform = gameObject.transform.GetChild(0);

        language[0] = "한국어";
        language[1] = "영어";

        SettingOnOff();

        // 각 UI 요소에 이벤트 리스너 추가
        languageButton[0].onClick.AddListener(ChangeLanguage);
        languageButton[1].onClick.AddListener(ChangeLanguage);

        resolutionButton[1].onClick.AddListener(() => SetResolution(0));
        resolutionButton[0].onClick.AddListener(() => SetResolution(1));

        fullscreenButton[0].onClick.AddListener(ToggleFullscreen);
        fullscreenButton[1].onClick.AddListener(ToggleFullscreen);

        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        backgroundSoundSlider.onValueChanged.AddListener(SetBackgroundSound);
        effectsSoundSlider.onValueChanged.AddListener(SetEffectsSound);

        // 진동 버튼들에 이벤트 리스너 추가
        for (int i = 0; i < vibrationButtons.Length; i++)
        {
            int index = i;
            vibrationButtons[i].onClick.AddListener(() => SetVibration(index));
        }

        // 초기 값 설정 (슬라이더 및 버튼 초기화)
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);  // 기본값 1.0
        backgroundSoundSlider.value = PlayerPrefs.GetFloat("BackgroundVolume", 1.0f);
        effectsSoundSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1.0f);
        vibrationLevel = PlayerPrefs.GetInt("VibrationLevel", 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SettingOnOff();
        }
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
        Screen.SetResolution(width, height, Screen.fullScreen);
        Debug.Log($"Screen.fullScreen : {Screen.fullScreen}");
        Debug.Log($"Resolution set to: {Screen.width}x{Screen.height}, Fullscreen: {Screen.fullScreen}");
    }

    // 전체화면 토글
    private void ToggleFullscreen()
    {
        //bool isFullscreen = Screen.fullScreen;
        isFullscreen = !isFullscreen;
        if (isFullscreen) {
            textMeshPros[2].text = "켜짐";
        }
        else
        {
            textMeshPros[2].text = "꺼짐";
        }

        Screen.SetResolution(Screen.width, Screen.height, !isFullscreen);  // 현재 해상도를 유지하며 전체화면/창모드 전환
    }

    // 화면 밝기 설정
    private void SetBrightness(float value)
    {
        // 밝기 조정 (간단한 방법은 색상 매트릭스를 사용하거나 카메라의 밝기 조정)
        RenderSettings.ambientLight = new Color(value, value, value);  // 밝기 조정 (컬러 값)
        PlayerPrefs.SetFloat("Brightness", value);  // 저장
    }

    // 배경음 설정
    private void SetBackgroundSound(float value)
    {
        // value가 0일 때 음량을 -80으로 설정
        if (Mathf.Approximately(value, 0.0f))
        {
            Debug.Log("Value is 0, setting background volume to -80");
            audioMixer.SetFloat("BackgroundVolume", -80);  // 소리가 안 나오는 값
        }
        else
        {
            // 최소 볼륨을 설정하여 값이 너무 낮게 가지 않도록 설정
            float volume = Mathf.Log10(value) * 20;
            volume = Mathf.Max(volume, -80); // 최소값을 -80으로 설정

            audioMixer.SetFloat("BackgroundVolume", volume);
        }

        // 설정된 값 저장
        PlayerPrefs.SetFloat("BackgroundVolume", value);
    }

    // 효과음 설정
    private void SetEffectsSound(float value)
    {
        // value가 0일 때 음량을 -80으로 설정
        if (Mathf.Approximately(value, 0.0f))
        {
            Debug.Log("Value is 0, setting background volume to -80");
            audioMixer.SetFloat("EffectsVolume", -80);  // 소리가 안 나오는 값
        }
        else
        {
            // 최소 볼륨을 설정하여 값이 너무 낮게 가지 않도록 설정
            float volume = Mathf.Log10(value) * 20;
            volume = Mathf.Max(volume, -80); // 최소값을 -80으로 설정

            audioMixer.SetFloat("EffectsVolume", volume);
        }

        // 설정된 값 저장
        PlayerPrefs.SetFloat("EffectsVolume", value);
    }

    // 화면 진동 설정
    private void SetVibration(int level)
    {
        vibrationLevel = level;
        PlayerPrefs.SetInt("VibrationLevel", vibrationLevel);  // 저장

        // 진동 강도에 따른 처리
        switch (vibrationLevel)
        {
            case 0:
                textMeshPros[3].text = "x0.3";
                // 진동 없음
                break;
            case 1:
                textMeshPros[3].text = "x0.7";
                // 약한 진동
                //Handheld.Vibrate();
                break;
            case 2:
                textMeshPros[3].text = "x1.0";
                // 중간 진동
                //Handheld.Vibrate();
                break;
            case 3:
                textMeshPros[3].text = "x1.3";
                // 강한 진동
                //Handheld.Vibrate();
                break;
            case 4:
                textMeshPros[3].text = "x1.6";
                // 가장 강한 진동
                //Handheld.Vibrate();
                break;
        }
    }
}

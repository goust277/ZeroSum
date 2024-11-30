using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;  // 오디오 관련 제어
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Security.AccessControl;  // 언어 설정 관련 처리를 위해

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

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
    //public AudioMixer audioMixer;  // 배경음과 효과음을 관리하는 믹서

    // 화면 진동 설정
    private int vibrationLevel = 0;

    // 변경 리스트
    private string[] language = new string[2];
    private int[][] resolutionValues = new int[3][];

    [SerializeField] private int resolutionIndex = 0;
    private int languageIndex = 0;
    private bool isFullscreen = true;
    private bool isSettingOpen= true;
    private Transform childTransform;
    private void Awake()
    {
        // Singleton Pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스 제거
        }
    }
    void Start()
    {
        childTransform = gameObject.transform.GetChild(0);

        language[0] = "한국어";
        language[1] = "영어";

        resolutionValues[0] = new int[2] { 1920, 1080 };  // 첫 번째 해상도
        resolutionValues[1] = new int[2] { 1600, 900 };   // en 번째 해상도
        resolutionValues[2] = new int[2] { 1280, 720 };   // 세 번째 해상도

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
            resolutionIndex = resolutionValues.Length-1;
        }
        else if (resolutionIndex == resolutionValues.Length)
        {
            resolutionIndex = 0;
        }

        int width = resolutionValues[resolutionIndex][0]; // 가로
        int height = resolutionValues[resolutionIndex][1]; // 세로
        textMeshPros[1].text = $"{width} X {height}";
        //Screen.SetResolution(width, height, Screen.fullScreen);

        Debug.Log($"Resolution set to: {width}x{height}");


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
        //audioMixer.SetFloat("BackgroundVolume", Mathf.Log10(value) * 20);  // 믹서에서 배경음 볼륨 조정
        PlayerPrefs.SetFloat("BackgroundVolume", value);  // 저장
    }

    // 효과음 설정
    private void SetEffectsSound(float value)
    {
        //audioMixer.SetFloat("EffectsVolume", Mathf.Log10(value) * 20);  // 믹서에서 효과음 볼륨 조정
        PlayerPrefs.SetFloat("EffectsVolume", value);  // 저장
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

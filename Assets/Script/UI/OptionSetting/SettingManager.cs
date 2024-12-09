using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;  // ����� ���� ����
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;  // ��� ���� ���� ó���� ����

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    // UI ��� ����
    [SerializeField] private Button[] languageButton = new Button[2];  // ��� ���� ��ư
    [SerializeField] private Button[] resolutionButton = new Button[2];  // �ػ� ���� ��ư
    [SerializeField] private Button[] fullscreenButton = new Button[2];  // ��üȭ�� ��ư
    [SerializeField] private Slider brightnessSlider;  // ȭ�� ��� �����̴�
    [SerializeField] private Slider backgroundSoundSlider;  // ����� �����̴�
    [SerializeField] private Slider effectsSoundSlider;  // ȿ���� �����̴�
    [SerializeField] private Button[] vibrationButtons = new Button[5];   // ȭ�� ���� ��ư�� (5��)
    [SerializeField] private TextMeshProUGUI[] textMeshPros = new TextMeshProUGUI[4];
    // ����� �ͼ� (�����, ȿ����)
    public AudioMixer audioMixer;  // ������� ȿ������ �����ϴ� �ͼ�

    // ȭ�� ���� ����
    private int vibrationLevel = 0;

    // ���� ����Ʈ
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
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �ν��Ͻ� ����
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

        language[0] = "�ѱ���";
        language[1] = "����";

        SettingOnOff();

        // �� UI ��ҿ� �̺�Ʈ ������ �߰�
        languageButton[0].onClick.AddListener(ChangeLanguage);
        languageButton[1].onClick.AddListener(ChangeLanguage);

        resolutionButton[1].onClick.AddListener(() => SetResolution(0));
        resolutionButton[0].onClick.AddListener(() => SetResolution(1));

        fullscreenButton[0].onClick.AddListener(ToggleFullscreen);
        fullscreenButton[1].onClick.AddListener(ToggleFullscreen);

        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        backgroundSoundSlider.onValueChanged.AddListener(SetBackgroundSound);
        effectsSoundSlider.onValueChanged.AddListener(SetEffectsSound);

        // ���� ��ư�鿡 �̺�Ʈ ������ �߰�
        for (int i = 0; i < vibrationButtons.Length; i++)
        {
            int index = i;
            vibrationButtons[i].onClick.AddListener(() => SetVibration(index));
        }

        // �ʱ� �� ���� (�����̴� �� ��ư �ʱ�ȭ)
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);  // �⺻�� 1.0
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


    // ��� ����
    private void ChangeLanguage()
    {
        // ��� ���� ���� (��: �ѱ���, ����� ����)
        // ���÷� �ܼ��� ���� ���ε� �ϴ� ������� ������ �� �ֽ��ϴ�.

        languageIndex = Math.Abs(languageIndex - 1);
        textMeshPros[0].text = language[languageIndex];

        //string currentScene = SceneManager.GetActiveScene().name;
        //SceneManager.LoadScene(currentScene);  // �� ���ε�� ��� ���� ����
    }

    // �ػ� ����
    // �ػ󵵸� �����ϴ� �޼���
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

        int width = resolutionValues[resolutionIndex].Width; // ����
        int height = resolutionValues[resolutionIndex].Height; // ����
        textMeshPros[1].text = $"{width} X {height}";
        Screen.SetResolution(width, height, Screen.fullScreen);
        Debug.Log($"Screen.fullScreen : {Screen.fullScreen}");
        Debug.Log($"Resolution set to: {Screen.width}x{Screen.height}, Fullscreen: {Screen.fullScreen}");
    }

    // ��üȭ�� ���
    private void ToggleFullscreen()
    {
        //bool isFullscreen = Screen.fullScreen;
        isFullscreen = !isFullscreen;
        if (isFullscreen) {
            textMeshPros[2].text = "����";
        }
        else
        {
            textMeshPros[2].text = "����";
        }

        Screen.SetResolution(Screen.width, Screen.height, !isFullscreen);  // ���� �ػ󵵸� �����ϸ� ��üȭ��/â��� ��ȯ
    }

    // ȭ�� ��� ����
    private void SetBrightness(float value)
    {
        // ��� ���� (������ ����� ���� ��Ʈ������ ����ϰų� ī�޶��� ��� ����)
        RenderSettings.ambientLight = new Color(value, value, value);  // ��� ���� (�÷� ��)
        PlayerPrefs.SetFloat("Brightness", value);  // ����
    }

    // ����� ����
    private void SetBackgroundSound(float value)
    {
        // value�� 0�� �� ������ -80���� ����
        if (Mathf.Approximately(value, 0.0f))
        {
            Debug.Log("Value is 0, setting background volume to -80");
            audioMixer.SetFloat("BackgroundVolume", -80);  // �Ҹ��� �� ������ ��
        }
        else
        {
            // �ּ� ������ �����Ͽ� ���� �ʹ� ���� ���� �ʵ��� ����
            float volume = Mathf.Log10(value) * 20;
            volume = Mathf.Max(volume, -80); // �ּҰ��� -80���� ����

            audioMixer.SetFloat("BackgroundVolume", volume);
        }

        // ������ �� ����
        PlayerPrefs.SetFloat("BackgroundVolume", value);
    }

    // ȿ���� ����
    private void SetEffectsSound(float value)
    {
        // value�� 0�� �� ������ -80���� ����
        if (Mathf.Approximately(value, 0.0f))
        {
            Debug.Log("Value is 0, setting background volume to -80");
            audioMixer.SetFloat("EffectsVolume", -80);  // �Ҹ��� �� ������ ��
        }
        else
        {
            // �ּ� ������ �����Ͽ� ���� �ʹ� ���� ���� �ʵ��� ����
            float volume = Mathf.Log10(value) * 20;
            volume = Mathf.Max(volume, -80); // �ּҰ��� -80���� ����

            audioMixer.SetFloat("EffectsVolume", volume);
        }

        // ������ �� ����
        PlayerPrefs.SetFloat("EffectsVolume", value);
    }

    // ȭ�� ���� ����
    private void SetVibration(int level)
    {
        vibrationLevel = level;
        PlayerPrefs.SetInt("VibrationLevel", vibrationLevel);  // ����

        // ���� ������ ���� ó��
        switch (vibrationLevel)
        {
            case 0:
                textMeshPros[3].text = "x0.3";
                // ���� ����
                break;
            case 1:
                textMeshPros[3].text = "x0.7";
                // ���� ����
                //Handheld.Vibrate();
                break;
            case 2:
                textMeshPros[3].text = "x1.0";
                // �߰� ����
                //Handheld.Vibrate();
                break;
            case 3:
                textMeshPros[3].text = "x1.3";
                // ���� ����
                //Handheld.Vibrate();
                break;
            case 4:
                textMeshPros[3].text = "x1.6";
                // ���� ���� ����
                //Handheld.Vibrate();
                break;
        }
    }
}

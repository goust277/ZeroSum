using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;  // ����� ���� ����
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Security.AccessControl;  // ��� ���� ���� ó���� ����

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

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
    //public AudioMixer audioMixer;  // ������� ȿ������ �����ϴ� �ͼ�

    // ȭ�� ���� ����
    private int vibrationLevel = 0;

    // ���� ����Ʈ
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
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �ν��Ͻ� ����
        }
    }
    void Start()
    {
        childTransform = gameObject.transform.GetChild(0);

        language[0] = "�ѱ���";
        language[1] = "����";

        resolutionValues[0] = new int[2] { 1920, 1080 };  // ù ��° �ػ�
        resolutionValues[1] = new int[2] { 1600, 900 };   // en ��° �ػ�
        resolutionValues[2] = new int[2] { 1280, 720 };   // �� ��° �ػ�

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
            resolutionIndex = resolutionValues.Length-1;
        }
        else if (resolutionIndex == resolutionValues.Length)
        {
            resolutionIndex = 0;
        }

        int width = resolutionValues[resolutionIndex][0]; // ����
        int height = resolutionValues[resolutionIndex][1]; // ����
        textMeshPros[1].text = $"{width} X {height}";
        //Screen.SetResolution(width, height, Screen.fullScreen);

        Debug.Log($"Resolution set to: {width}x{height}");


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
        //audioMixer.SetFloat("BackgroundVolume", Mathf.Log10(value) * 20);  // �ͼ����� ����� ���� ����
        PlayerPrefs.SetFloat("BackgroundVolume", value);  // ����
    }

    // ȿ���� ����
    private void SetEffectsSound(float value)
    {
        //audioMixer.SetFloat("EffectsVolume", Mathf.Log10(value) * 20);  // �ͼ����� ȿ���� ���� ����
        PlayerPrefs.SetFloat("EffectsVolume", value);  // ����
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

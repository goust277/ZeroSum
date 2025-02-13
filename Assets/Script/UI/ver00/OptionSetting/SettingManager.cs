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
    // UI ��� ����
    [SerializeField] private Button resolutionButton;  // �ػ� ���� ��ư
    [SerializeField] private Button fullscreenButton;  // ��üȭ�� ��ư
    [SerializeField] private Slider backgroundSoundSlider;  // ����� �����̴�
    [SerializeField] private Slider effectsSoundSlider;  // ȿ���� �����̴�
    [SerializeField] private Button vibrationButtons;   // ȭ�� ���� ��ư�� (5��)
    [SerializeField] private TextMeshProUGUI[] textMeshPros = new TextMeshProUGUI[4];
    // ����� �ͼ� (�����, ȿ����)
    public AudioMixer audioMixer;  // ������� ȿ������ �����ϴ� �ͼ�


    // ȭ�� ���� ����
    private int vibrationLevel = 0;

    // ���� ����Ʈ
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
        ToggleFullscreen();
        //Screen.SetResolution(1920, 1080, true);
        childTransform = gameObject.transform.GetChild(0);


        SettingOnOff();

        // �� UI ��ҿ� �̺�Ʈ ������ �߰�
        resolutionButton.onClick.AddListener(() => SetResolution(0));
        fullscreenButton.onClick.AddListener(ToggleFullscreen);

        //brightnessSlider.onValueChanged.AddListener(SetBrightness);
        backgroundSoundSlider.onValueChanged.AddListener(SetBackgroundSound);
        effectsSoundSlider.onValueChanged.AddListener(SetEffectsSound);

        vibrationButtons.onClick.AddListener(() => SetVibration(0));


        // �ʱ� �� ���� (�����̴� �� ��ư �ʱ�ȭ)
        //brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);  // �⺻�� 1.0
        backgroundSoundSlider.value = PlayerPrefs.GetFloat("BackgroundVolume", 1.0f);
        effectsSoundSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1.0f);
        vibrationLevel = PlayerPrefs.GetInt("VibrationLevel", 0);

    }


    public void SettingOnOff()
    {
        bool newState = !childTransform.gameObject.activeSelf;

        if (!isSettingOpen)
        {
            Debug.Log("�ɼ� ������");
            childTransform.gameObject.SetActive(newState);
            isSettingOpen = true;
        }
        else
        {
            Debug.Log("�ɼ� �ݰܿ�");
            childTransform.gameObject.SetActive(newState);
            isSettingOpen = false;
        }
    }
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
        Screen.SetResolution(width, height, isFullscreen);
        DebugTemp.text += $"Resolution set to: {Screen.width}x{Screen.height}, Fullscreen: {Screen.fullScreen}";
        DebugTemp.text += "\n";
    }

    // ��üȭ�� ���
    private void ToggleFullscreen()
    {
        //bool isFullscreen = Screen.fullScreen;

        if (!isFullscreen) {
            textMeshPros[2].text = "����";
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            textMeshPros[2].text = "����";
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        isFullscreen = !Screen.fullScreen;

        Screen.SetResolution(Screen.width, Screen.height, isFullscreen);  // ���� �ػ󵵸� �����ϸ� ��üȭ��/â��� ��ȯ
        DebugTemp.text += $"Is Fullscreen: {isFullscreen}, FullScreenMode: {Screen.fullScreenMode}, Resolution: {Screen.currentResolution.width}x{Screen.currentResolution.height}";
        DebugTemp.text += "\n";
    }

    // ����� ����
    private void SetBackgroundSound(float value)
    {
        float volume = Mathf.Approximately(value, 0.0f) ? -80f : Mathf.Log10(value) * 20;
        audioMixer.SetFloat("BackgroundVolume", Mathf.Max(volume, -80));
        PlayerPrefs.SetFloat("BackgroundVolume", value);
    }

    // ȿ���� ����
    private void SetEffectsSound(float value)
    {
        float volume = Mathf.Approximately(value, 0.0f) ? -80f : Mathf.Log10(value) * 20;
        audioMixer.SetFloat("EffectsVolume", Mathf.Max(volume, -80));
        PlayerPrefs.SetFloat("EffectsVolume", value);
    }

    private void SetVibration(int level)
    {
        vibrationLevel = level;
        PlayerPrefs.SetInt("VibrationLevel", vibrationLevel);  // ����

        string[] vibrationLevels = { "x0.3", "x0.7", "x1.0", "x1.3", "x1.6" };
        textMeshPros[3].text = vibrationLevels[level];
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("UI menu Resource")]
    [SerializeField] private Button fullscreenButton;
    [SerializeField] private Slider backgroundSoundSlider;
    [SerializeField] private Slider effectsSoundSlider;
    [SerializeField] private Button vibrationButtons;

    [Header("Option Resource")]
    [SerializeField] private TextMeshProUGUI[] textMeshPros = new TextMeshProUGUI[3]; // [0]: Fullscreen, [1]: Vibrate, [2]: BGM, [3]: Effect

    public AudioMixer audioMixer;

    private const bool defaultIsFullscreen = true;
    private const float defaultBackgroundVolume = 1f;
    private const float defaultEffectsVolume = 1f;

    private int vibrationLevel = 2;
    private bool isFullscreen;
    private bool isSettingOpen = true;

    [SerializeField] private Button optionResetbtn;
    [SerializeField] private GameObject optionSettingUI;

    private const string BG_KEY = "BackgroundVolume";
    private const string EF_KEY = "EffectsVolume";

    private void Awake()
    {
        float bgValue = PlayerPrefs.GetFloat(BG_KEY, defaultBackgroundVolume);
        float efValue = PlayerPrefs.GetFloat(EF_KEY, defaultEffectsVolume);

        SetVolume("BackgroundVolume", BG_KEY, bgValue, textMeshPros[2]);
        SetVolume("EffectsVolume", EF_KEY, efValue, textMeshPros[3]);

        backgroundSoundSlider.SetValueWithoutNotify(bgValue);
        effectsSoundSlider.SetValueWithoutNotify(efValue);
    }

    void Start()
    {
        
        isFullscreen = Screen.fullScreen;

        SettingOnOff();

        fullscreenButton.onClick.AddListener(ToggleFullscreen);
        optionResetbtn.onClick.AddListener(OnClickResetBtn);

        backgroundSoundSlider.onValueChanged.AddListener(
            value => SetVolume("BackgroundVolume", BG_KEY, value, textMeshPros[2])
        );

        effectsSoundSlider.onValueChanged.AddListener(
            value => SetVolume("EffectsVolume", EF_KEY, value, textMeshPros[3])
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

        backgroundSoundSlider.value = defaultBackgroundVolume;
        SetVolume("BackgroundVolume", BG_KEY, defaultBackgroundVolume, textMeshPros[2]);

        effectsSoundSlider.value = defaultEffectsVolume;
        SetVolume("EffectsVolume", EF_KEY, defaultEffectsVolume, textMeshPros[3]);

        textMeshPros[0].text = "켜짐";
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        isFullscreen = Screen.fullScreen;
    }

    public void SettingOnOff()
    {
        bool newState = !optionSettingUI.gameObject.activeSelf;

        if (!isSettingOpen)
        {
            optionSettingUI.gameObject.SetActive(newState);
            isSettingOpen = true;
        }
        else
        {
            optionSettingUI.gameObject.SetActive(newState);
            isSettingOpen = false;
        }
    }

    private void ToggleFullscreen()
    {
        audioSource.Play();

        isFullscreen = Screen.fullScreen;
        if (!isFullscreen)
        {
            textMeshPros[0].text = "켜짐";
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            textMeshPros[0].text = "꺼짐";
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        isFullscreen = !Screen.fullScreen;
        Screen.SetResolution(Screen.width, Screen.height, isFullscreen);
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
        textMeshPros[1].text = vibrationLevels[vibrationLevel];
    }

    public void playEffectSound()
    {
        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;  // 화면 밝기 슬라이더

    public PostProcessProfile brightness;
    public PostProcessLayer layer;
    AutoExposure exposure;

    // Start is called before the first frame update
    void Start()
    {
        brightness.TryGetSettings(out exposure);
    }

    public void SetBrightness(float value)
    {

        if (value != 0)
        {
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = 0.05f;
        }

        // 밝기 조정 (간단한 방법은 색상 매트릭스를 사용하거나 카메라의 밝기 조정)
        //RenderSettings.ambientLight = new Color(value, value, value);  // 밝기 조정 (컬러 값)
        //PlayerPrefs.SetFloat("Brightness", value);  // 저장
    }
}

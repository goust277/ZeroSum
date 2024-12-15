using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;  // ȭ�� ��� �����̴�

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

        // ��� ���� (������ ����� ���� ��Ʈ������ ����ϰų� ī�޶��� ��� ����)
        //RenderSettings.ambientLight = new Color(value, value, value);  // ��� ���� (�÷� ��)
        //PlayerPrefs.SetFloat("Brightness", value);  // ����
    }
}

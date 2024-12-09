using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    public Image Panel;     //���̵� �ƿ��� ���� ȭ��
    float currentTime = 0;  //���� �ð�
    float fadeTime = 2;  //���̵�ƿ��� ����� �ð�

    IEnumerator fadeIn()
    {
        Panel.gameObject.SetActive(true);
        Color alpha = Panel.color;
        while (alpha.a > 0)
        {
            currentTime += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(1, 0, currentTime);
            Panel.color = alpha;
            yield return null;
        }
    }

    IEnumerator fadeOut()
    {
        Panel.gameObject.SetActive(true);
        Color alpha = Panel.color;
        while (alpha.a < 1)
        {
            currentTime += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(0, 1, currentTime);
            Panel.color = alpha;
            yield return null;
        }
        SceneManager.LoadScene("PlayScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(fadeIn());
    }
}

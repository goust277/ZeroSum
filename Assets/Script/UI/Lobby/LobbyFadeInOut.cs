using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyFadeInOut : MonoBehaviour
{
    public Image fadeCanvas; // ���� ȭ�� �̹��� (UI)
    public AudioSource bgm; // ��� ����� �ҽ�
    public float fadeDuration = 2f; // ���̵� �ƿ� ���� �ð�

    private void Start()
    {
        Color tempColor = fadeCanvas.color;
        tempColor.a = 0f; // ������ �� ����
        fadeCanvas.color = tempColor;
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float timer = 0f;
        float startVolume = bgm.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alphaValue = timer / fadeDuration;

            // ���� ȭ�� ���� ��ο����� ����
            fadeCanvas.color = new Color(fadeCanvas.color.r, fadeCanvas.color.g, fadeCanvas.color.b, alphaValue);

            // ��� ������ ���̱�
            bgm.volume = Mathf.Lerp(startVolume, 0f, alphaValue);

            yield return null;
        }

        bgm.volume = 0f;

        // �� ��ȯ ����
        SceneManager.LoadScene(sceneName);
    }
}

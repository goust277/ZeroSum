using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyFadeInOut : MonoBehaviour
{
    public Image fadeCanvas; // 검은 화면 이미지 (UI)
    public AudioSource bgm; // 브금 오디오 소스
    public float fadeDuration = 2f; // 페이드 아웃 지속 시간

    private void Start()
    {
        Color tempColor = fadeCanvas.color;
        tempColor.a = 0f; // 시작할 때 투명
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

            // 검은 화면 점점 어두워지게 설정
            fadeCanvas.color = new Color(fadeCanvas.color.r, fadeCanvas.color.g, fadeCanvas.color.b, alphaValue);

            // 브금 서서히 줄이기
            bgm.volume = Mathf.Lerp(startVolume, 0f, alphaValue);

            yield return null;
        }

        bgm.volume = 0f;

        // 씬 전환 실행
        SceneManager.LoadScene(sceneName);
    }
}

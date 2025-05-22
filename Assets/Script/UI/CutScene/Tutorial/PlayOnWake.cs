using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayOnWake : MonoBehaviour
{
    public Image fadePanel;
    float fadeTime = 1.0f;  //페이드아웃이 진행될 시간
    float currentTime = 0;

    [Header("virtualCamera Resources")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform playerTransform;

    [Header("CutsceneManager Resources")]
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private PlayableDirector director;

    void Start()
    {
        StartCoroutine(FadeIn());
        StartCoroutine(StartCutsceneDelayed());
    }

    IEnumerator StartCutsceneDelayed()
    {
        yield return new WaitForSeconds(0.3f);

        // 현재 카메라 위치와 회전값을 가져와서 버츄얼 카메라에 적용
        Vector3 currentCameraPosition = Camera.main.transform.position;
        Quaternion currentCameraRotation = Camera.main.transform.rotation;

        // 버츄얼 카메라의 위치와 회전값을 현재 카메라 값으로 설정
        virtualCamera.transform.position = currentCameraPosition;
        virtualCamera.transform.rotation = currentCameraRotation;


        if (director != null)
        {
            cutsceneManager.PlayCutscene(director);
        }

        yield break;
    }

    IEnumerator FadeIn()
    {
        fadePanel.gameObject.SetActive(true);
        Color alpha = fadePanel.color;
        while (alpha.a > 0)
        {
            currentTime += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(1, 0, currentTime);
            fadePanel.color = alpha;
            yield return null;
        }
    }

}

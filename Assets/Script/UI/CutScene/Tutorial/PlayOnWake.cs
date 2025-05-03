using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayOnWake : MonoBehaviour
{
    public Image fadePanel;
    [SerializeField] private PlayableDirector director;
    float fadeTime = 2;  //���̵�ƿ��� ����� �ð�
    float currentTime = 0;

    [Header("virtualCamera Resources")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        StartCoroutine(StartCutsceneDelayed());
    }

    IEnumerator StartCutsceneDelayed()
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

        // ���� ī�޶� ��ġ�� ȸ������ �����ͼ� ����� ī�޶� ����
        Vector3 currentCameraPosition = Camera.main.transform.position;
        Quaternion currentCameraRotation = Camera.main.transform.rotation;

        // ����� ī�޶��� ��ġ�� ȸ������ ���� ī�޶� ������ ����
        virtualCamera.transform.position = currentCameraPosition;
        virtualCamera.transform.rotation = currentCameraRotation;

        director.Play();
    }

}

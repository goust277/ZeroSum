using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class BeforeEvUp : MonoBehaviour
{
    [SerializeField] private GameObject num5Canvas;
    [SerializeField] public PlayableDirector director;
    private bool hasPlayed = false; // ������ ��� ����
    [SerializeField] private BoxCollider2D ev;

    [Header("virtualCamera Resources")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Fadeout Light")]
    [SerializeField] private Light2D light2D;
    [SerializeField] private float fadeDuration = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            Debug.Log("BeforeEvUp - OnTriggerEnter2D");
            ev.enabled = false;
            num5Canvas.gameObject.SetActive(false); // �ȳ��� ��Ȱ��ȭ
            hasPlayed = true;

            // ���� ī�޶� ��ġ�� ȸ������ �����ͼ� ����� ī�޶� ����
            Vector3 currentCameraPosition = Camera.main.transform.position;
            Quaternion currentCameraRotation = Camera.main.transform.rotation;

            // ����� ī�޶��� ��ġ�� ȸ������ ���� ī�޶� ������ ����
            virtualCamera.transform.position = currentCameraPosition;
            virtualCamera.transform.rotation = currentCameraRotation;

            StartCoroutine(GrowAndFade());
            director.Play();
        }
    }

    private IEnumerator GrowAndFade()
    {

        float timer = 0f;
        float startFalloff = 1f;
        float endFalloff = 0.2f;

        // Step: Grow + Falloff ��ȭ
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            light2D.pointLightOuterRadius = Mathf.Lerp(0f, 10f, t);
            light2D.falloffIntensity = Mathf.Lerp(startFalloff, endFalloff, t);
            // intensity�� �ǵ帮�� ����
            yield return null;
        }

        light2D.enabled = false;
    }

}
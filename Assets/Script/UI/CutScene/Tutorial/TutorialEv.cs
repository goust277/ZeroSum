using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialEv : MonoBehaviour
{
    [SerializeField] public PlayableDirector director;
    [SerializeField] private Mission mission;
    [SerializeField] private BoxCollider2D ev;

    [Header("CutScene Resources")]
    [SerializeField] private GameObject directorCanvas;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private bool isClear;

    // Update is called once per frame
    void Update()
    {
        if (mission.isClear)
        {
            if (!isClear)
            {
                isClear = true;
                ev.enabled = true;
                directorCanvas.SetActive(false);
                Invoke("PlayCutScene", 1.0f);
            }
        }
    }
    void PlayCutScene()
    {

        // ���� ī�޶� ��ġ�� ȸ������ �����ͼ� ����� ī�޶� ����
        Vector3 currentCameraPosition = Camera.main.transform.position;
        Quaternion currentCameraRotation = Camera.main.transform.rotation;

        // ����� ī�޶��� ��ġ�� ȸ������ ���� ī�޶� ������ ����
        virtualCamera.transform.position = currentCameraPosition;
        virtualCamera.transform.rotation = currentCameraRotation;
        director.Play();
    }
}

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
    [SerializeField] private RectTransform uiTextTransform;
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

                Invoke("PlayCutScene", 1.0f);
            }
        }
    }
    void PlayCutScene()
    {

        // 현재 카메라 위치와 회전값을 가져와서 버츄얼 카메라에 적용
        Vector3 currentCameraPosition = Camera.main.transform.position;
        Quaternion currentCameraRotation = Camera.main.transform.rotation;

        // 버츄얼 카메라의 위치와 회전값을 현재 카메라 값으로 설정
        virtualCamera.transform.position = currentCameraPosition;
        virtualCamera.transform.rotation = currentCameraRotation;

        uiTextTransform.position = playerTransform.position + new Vector3(0.5f, 2.3f, 0);

        directorCanvas.SetActive(false);
        director.Play();
    }
}

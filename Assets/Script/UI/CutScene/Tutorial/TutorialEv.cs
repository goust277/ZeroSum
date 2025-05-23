using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialEv : MonoBehaviour
{
    [Header("CutsceneManager Resources")]
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private PlayableDirector director;

    [Header("Direct Resources")]
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
                directorCanvas.SetActive(false);
                PlayCutScene();
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

        
        if (director != null)
        {
            director.stopped += ElevatorOn;
            cutsceneManager.PlayCutscene(director);
        }
    }

    void ElevatorOn(PlayableDirector obj)
    {
        ev.enabled = true;
        director.stopped -= ElevatorOn;
    }

}

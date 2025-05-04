using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleCutScene : MonoBehaviour
{
    [SerializeField] public PlayableDirector director;

    [Header("Resources")]
    [SerializeField] private GameObject num1Obj;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private bool hasPlayed = false; // 여러번 재생 방지

    void Start()
    {
        PlayableAsset asset = director.playableAsset;
        AnimationTrack hudTrack = null;

        foreach (var track in asset.outputs)
        {
            if (track.streamName == "Hud")
            {
                hudTrack = track.sourceObject as AnimationTrack;
                break;
            }
        }

        if (hudTrack != null)
        {
            director.SetGenericBinding(hudTrack, GameStateManager.Instance.hudUI);
        }
        else
        {
            Debug.LogWarning("HUD 못찾음.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            Invoke("CanvasTrnasform", 1.0f);

            num1Obj.gameObject.SetActive(false); // 대사창 비활성화
            //cutsceneManager.PlayCutscene(director);
            hasPlayed = true;
            director.Play();
        }
    }
    void CanvasTrnasform()
    {
        // 현재 카메라 위치와 회전값을 가져와서 버츄얼 카메라에 적용
        Vector3 currentCameraPosition = Camera.main.transform.position;
        Quaternion currentCameraRotation = Camera.main.transform.rotation;

        // 버츄얼 카메라의 위치와 회전값을 현재 카메라 값으로 설정
        virtualCamera.transform.position = currentCameraPosition;
        virtualCamera.transform.rotation = currentCameraRotation;
    }
}

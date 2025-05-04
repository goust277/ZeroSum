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

    private bool hasPlayed = false; // ������ ��� ����

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
            Debug.LogWarning("HUD ��ã��.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            Invoke("CanvasTrnasform", 1.0f);

            num1Obj.gameObject.SetActive(false); // ���â ��Ȱ��ȭ
            //cutsceneManager.PlayCutscene(director);
            hasPlayed = true;
            director.Play();
        }
    }
    void CanvasTrnasform()
    {
        // ���� ī�޶� ��ġ�� ȸ������ �����ͼ� ����� ī�޶� ����
        Vector3 currentCameraPosition = Camera.main.transform.position;
        Quaternion currentCameraRotation = Camera.main.transform.rotation;

        // ����� ī�޶��� ��ġ�� ȸ������ ���� ī�޶� ������ ����
        virtualCamera.transform.position = currentCameraPosition;
        virtualCamera.transform.rotation = currentCameraRotation;
    }
}

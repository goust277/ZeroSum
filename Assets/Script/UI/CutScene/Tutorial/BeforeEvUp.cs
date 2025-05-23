using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

public class BeforeEvUp : MonoBehaviour
{
    [SerializeField] private GameObject num5Canvas;

    [Header("CutsceneManager Resources")]
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private PlayableDirector director;

    private bool hasPlayed = false; // 여러번 재생 방지
    [SerializeField] public BoxCollider2D ev;

    [Header("virtualCamera Resources")]
    [SerializeField] private Transform playerTransform;

    [Header("Fadeout Light")]
    [SerializeField] private Light2D light2D;
    [SerializeField] private float fadeDuration = 5f;

    [Header("Before LastCutScene")]
    [SerializeField] private MissionDoorManager tutorialMissionManager;
    [SerializeField] private MissionDoorManager realMissionDoorManager;

    [Header("Tutorial End")]
    [SerializeField] private GameObject cutSceneTrigger;
    private GameObject skipper;

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

        skipper = GameObject.Find("SkipDetect");

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            Debug.Log("BeforeEvUp - OnTriggerEnter2D");
            ev.enabled = false;
            num5Canvas.SetActive(false); // 안내문 비활성화
            hasPlayed = true;

            gameObject.GetComponent<Collider2D>().enabled = false;

            director.stopped += OnTutorialEnd;

            StartCoroutine(GrowAndFade());
            if (director != null)
            {
                cutsceneManager.PlayCutscene(director);
            }
        }
    }

    private void OnTutorialEnd(PlayableDirector director)
    {
        cutSceneTrigger.SetActive(false);
        skipper.GetComponent<TutorialSkipper>().ConnectPause();

        skipper.SetActive(false);
    }

    public IEnumerator GrowAndFade()
    {

        float timer = 0f;
        float startFalloff = 1f;
        float endFalloff = 0.2f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            light2D.pointLightOuterRadius = Mathf.Lerp(0f, 10f, t);
            light2D.falloffIntensity = Mathf.Lerp(startFalloff, endFalloff, t);
            // intensity는 건드리지 않음
            yield return null;
        }

        light2D.enabled = false;
        tutorialMissionManager.enabled = false;
        realMissionDoorManager.enabled = true;
    }

}
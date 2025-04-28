using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BattleCutScene : MonoBehaviour
{
    [SerializeField] public PlayableDirector director;

    [Header("Resources")]
    [SerializeField] private GameObject num1Obj;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform uiTextTransform;

    private bool hasPlayed = false; // 여러번 재생 방지

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
        uiTextTransform.position = playerTransform.position + new Vector3(0.5f, 2.3f, 0);
    }
}

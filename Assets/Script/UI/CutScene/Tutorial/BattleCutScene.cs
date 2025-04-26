using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BattleCutScene : MonoBehaviour
{
    [SerializeField] public PlayableDirector director;
    //[SerializeField] private CutsceneManager cutsceneManager;

    [Header("Resources")]
    [SerializeField] private GameObject num1Obj;
    [SerializeField] private GameObject num2;

    private bool hasPlayed = false; // ������ ��� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            num1Obj.gameObject.SetActive(false); // ���â ��Ȱ��ȭ
            //cutsceneManager.PlayCutscene(director);
            hasPlayed = true;
            director.Play();
        }
    }
}

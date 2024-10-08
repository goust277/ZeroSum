using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))  // 플레이어가 범위 안에 들어왔을 때
        {
            dialogueManager.EnableInteraction();  // GameController에 상호작용 가능 신호를 보냄
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // 플레이어가 범위 밖으로 나갔을 때
        {
            dialogueManager.DisableInteraction();  // GameController에 상호작용 종료 신호를 보냄
        }
    }
}

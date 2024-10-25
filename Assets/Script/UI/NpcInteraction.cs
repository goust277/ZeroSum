using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    [SerializeField] private PlayerConversation playerConversation;
    [SerializeField] private GameObject interactPrompt; // 상호작용 키 표시 UI
    [SerializeField] private int NPCID = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))  // 플레이어가 범위 안에 들어왔을 때
        {
            interactPrompt.SetActive(true); // 상호작용 프롬프트 UI 활성화
            playerConversation.isInteracting = true;  // GameController에 상호작용 가능 신호를 보냄
            playerConversation.CollisionNPC = NPCID;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // 플레이어가 범위 밖으로 나갔을 때
        {
            interactPrompt.SetActive(false); // 상호작용 프롬프트 UI 비활성화
            playerConversation.isInteracting = false;  // GameController에 상호작용 종료 신호를 보냄
        }
    }
}

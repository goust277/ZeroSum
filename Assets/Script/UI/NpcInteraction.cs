using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    [SerializeField] private PlayerConversation playerConversation;
    [SerializeField] private GameObject interactPrompt; // ��ȣ�ۿ� Ű ǥ�� UI
    [SerializeField] private int NPCID = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))  // �÷��̾ ���� �ȿ� ������ ��
        {
            interactPrompt.SetActive(true); // ��ȣ�ۿ� ������Ʈ UI Ȱ��ȭ
            playerConversation.isInteracting = true;  // GameController�� ��ȣ�ۿ� ���� ��ȣ�� ����
            playerConversation.CollisionNPC = NPCID;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // �÷��̾ ���� ������ ������ ��
        {
            interactPrompt.SetActive(false); // ��ȣ�ۿ� ������Ʈ UI ��Ȱ��ȭ
            playerConversation.isInteracting = false;  // GameController�� ��ȣ�ۿ� ���� ��ȣ�� ����
        }
    }
}

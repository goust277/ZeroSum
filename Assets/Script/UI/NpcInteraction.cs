using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))  // �÷��̾ ���� �ȿ� ������ ��
        {
            dialogueManager.EnableInteraction();  // GameController�� ��ȣ�ۿ� ���� ��ȣ�� ����
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // �÷��̾ ���� ������ ������ ��
        {
            dialogueManager.DisableInteraction();  // GameController�� ��ȣ�ۿ� ���� ��ȣ�� ����
        }
    }
}

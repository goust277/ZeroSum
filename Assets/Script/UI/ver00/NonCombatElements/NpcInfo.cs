using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[ExecuteInEditMode]
public class NpcInfo : MonoBehaviour
{
    private PlayerUIInteract playerConversation;
    [SerializeField] private GameObject interactPrompt; // ��ȣ�ۿ� Ű ǥ�� UI
    [SerializeField] private int NPCID = 1;
    [SerializeField] private TextMeshProUGUI nameText;

    //��ȣ�ۿ��, �׵θ� ǥ��
    public Color color = Color.white;
    [Range(0, 16)]
    public int outlineSize = 3;
    private SpriteRenderer spriteRenderer;

    private NPCInfo nNPCInfo;


    private void Start()
    {
        playerConversation ??= FindAnyObjectByType<PlayerUIInteract>();

        interactPrompt.SetActive(false);
        UpdateOutline(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // �÷��̾ ���� �ȿ� ������ ��
        {
            nNPCInfo ??= FindObjectOfType<DialogueManager>().GetNPC(NPCID);
            OnEnable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // �÷��̾ ���� ������ ������ ��
        {

            OnDisable();
        }
    }

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateOutline(true);
        interactPrompt.SetActive(true); // ��ȣ�ۿ� ������Ʈ UI Ȱ��ȭ
        if(playerConversation != null)
        {
            playerConversation.isInteracting = true;  // GameController�� ��ȣ�ۿ� ���� ��ȣ�� ����
            playerConversation.CollisionNPC = NPCID;
        }
        nameText.text = nNPCInfo?.NPCname;
    }

    void OnDisable()
    {
        interactPrompt.SetActive(false); // ��ȣ�ۿ� ������Ʈ UI ��Ȱ��ȭ
        if (playerConversation != null)
        {
            playerConversation.isInteracting = false;  // GameController�� ��ȣ�ۿ� ���� ��ȣ�� ����
        }
        UpdateOutline(false);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }

}

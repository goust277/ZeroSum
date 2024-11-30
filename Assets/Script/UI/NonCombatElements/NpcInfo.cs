using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[ExecuteInEditMode]
public class NpcInfo : MonoBehaviour
{
    private PlayerUIInteract playerConversation;
    [SerializeField] private GameObject interactPrompt; // 상호작용 키 표시 UI
    [SerializeField] private int NPCID = 1;
    [SerializeField] private TextMeshProUGUI nameText;

    //상호작용시, 테두리 표시
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
        if (collision.CompareTag("Player"))  // 플레이어가 범위 안에 들어왔을 때
        {
            nNPCInfo ??= FindObjectOfType<DialogueManager>().GetNPC(NPCID);
            OnEnable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // 플레이어가 범위 밖으로 나갔을 때
        {

            OnDisable();
        }
    }

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateOutline(true);
        interactPrompt.SetActive(true); // 상호작용 프롬프트 UI 활성화
        if(playerConversation != null)
        {
            playerConversation.isInteracting = true;  // GameController에 상호작용 가능 신호를 보냄
            playerConversation.CollisionNPC = NPCID;
        }
        nameText.text = nNPCInfo?.NPCname;
    }

    void OnDisable()
    {
        interactPrompt.SetActive(false); // 상호작용 프롬프트 UI 비활성화
        playerConversation.isInteracting = false;  // GameController에 상호작용 종료 신호를 보냄
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

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FarmingDoor : BaseInteractable
{
    [SerializeField] private GameObject spriteChanger;
    //public Sprite[] sprites; // 변경할 스프라이트 배열

    [SerializeField] private bool isUse = false; // 플레이어가 근처에 있는지 여부

    [Header("Auto Independent Resources")]
    [SerializeField] private GameObject[] dropItemList;
    [SerializeField] private GameObject guide;
    [SerializeField] private Animator animator; // Animator 컴포넌트 참조

    [Header("Monster Door")]
    [SerializeField] private GameObject monsterDoor;


    private SpriteChanger spriteChangerScript; // SpriteChanger 스크립트 참조
    private Collider2D doorCollider;

    [Header("FarmingDoorInteract")]
    [SerializeField] private FarmingDoorInteract doorInteract;


    void Start()
    {
        //spriteChanger.SetActive(false);
        spriteChangerScript = spriteChanger.GetComponent<SpriteChanger>();
        doorCollider = GetComponent<Collider2D>(); // 콜라이더 참조
        animator = GetComponent<Animator>();

        GameObject player = GameObject.Find("Player");

        if (player == null)
        {
            Debug.Log("Player not found");
        }

        doorInteract = FindObjectOfType<FarmingDoorInteract>();
        if (doorInteract == null)
        {
            Debug.LogError("FarmingDoorInteract not found");
        }
    }

    private void OpenDoor()
    {
        
        Invoke("ReopenDoor", 1.0f);
    }

    public void DoorChang()
    {
        if (monsterDoor != null)
        {
            monsterDoor.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    private void ReopenDoor()
    {
        doorInteract?.SetInvisible(true);
    }

    public void ReceiveDropIndex(int dropIndex)
    {
        Instantiate(dropItemList[dropIndex], transform.position, Quaternion.identity);
       //DoorChang();
    }

    public override void Exe()
    {
        if(!isUse)
        {
            Invoke("OpenDoor", 2.0f);
            animator.SetTrigger("Open");
            spriteChanger.SetActive(true); // 활성화하면 자동으로 코루틴 실행됨
            guide.SetActive(false);
            doorCollider.enabled = false;
            isUse = true;

            doorInteract?.SetInvisible(false);

        }

    }
}

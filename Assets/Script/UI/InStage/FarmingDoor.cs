using System.Collections;
using UnityEngine;

public class FarmingDoor : MonoBehaviour
{
    [SerializeField] private GameObject spriteChanger;
    public Sprite[] sprites; // 변경할 스프라이트 배열

    [SerializeField] private bool isTriggerEnter = false; // 플레이어가 근처에 있는지 여부
    [SerializeField] private GameObject[] dropItemList;
    [SerializeField] private Animator animator; // Animator 컴포넌트 참조

    private SpriteChanger spriteChangerScript; // SpriteChanger 스크립트 참조
    private Collider2D doorCollider;

    void Start()
    {
        spriteChanger.SetActive(false);
        spriteChangerScript = spriteChanger.GetComponent<SpriteChanger>();
        doorCollider = GetComponent<Collider2D>(); // 콜라이더 참조
    }

    private void OpenDoor()
    {
        animator.SetTrigger("Open");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player") && !isTriggerEnter)
        {
            isTriggerEnter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            isTriggerEnter = false;
        }
    }

    private void Update()
    {
        if (isTriggerEnter && Input.GetKeyUp(KeyCode.UpArrow)) // 플레이어가 근처에서 키 입력
        {
            Invoke("OpenDoor", 2.0f);
            spriteChanger.SetActive(true); // 활성화하면 자동으로 코루틴 실행됨
            doorCollider.enabled = false;
            isTriggerEnter = false;
        }
    }

    public void ReceiveDropIndex(int dropIndex)
    {
        Instantiate(dropItemList[dropIndex], transform.position, Quaternion.identity);
    }
}

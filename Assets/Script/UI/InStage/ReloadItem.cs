using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReloadItem : MonoBehaviour
{
    private bool isCollision = false;
    // Start is called before the first frame update

    [SerializeField] TextMeshProUGUI amountText;

    private void Start()
    {
        amountText.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.root.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {
            isCollision = true;
            Collider2D objCollider = GetComponent<Collider2D>();  // 
            objCollider.enabled = false;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();  // Rigidbody2D 참조

            rb.isKinematic = true;    //중력 & 물리적 반응 제거
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            amountText.gameObject.SetActive(true);
            amountText.text = "+" + Ver01_DungeonStatManager.Instance.TakeReloadItem();

            //Instantiate(AmountText, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.5f); // 1초 후 삭제
        }
    }
}

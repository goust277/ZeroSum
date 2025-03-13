using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painkiller : MonoBehaviour
{
    private bool isCollision = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.collider.CompareTag("Player") + "부딪힘");

        if (other.collider.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {
            isCollision = true;
            Collider2D objCollider = GetComponent<Collider2D>();  // 
            Physics2D.IgnoreCollision(objCollider, other.collider, true);
            other.gameObject.GetComponent<PlayerHP>().GetPainKiller(30.0f);
            Destroy(gameObject, 0.5f); // 0.5초 후 삭제
        }
    }
}

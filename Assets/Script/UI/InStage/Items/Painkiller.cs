using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painkiller : BaseItem
{
    private bool isCollision = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other.collider.CompareTag("Player") + "부딪힘");

        if (other.transform.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {
            isCollision = true;
            Collider2D objCollider = GetComponent<Collider2D>();  // 
            objCollider.enabled = false;

            PlaySound();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();  // Rigidbody2D 참조

            rb.isKinematic = true;    //중력 & 물리적 반응 제거
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                PlayerHP playerHP = playerObj.GetComponent<PlayerHP>();
                if (playerHP != null)
                {
                    playerHP.GetPainKiller(30.0f);
                }
            }
            Destroy(gameObject, 0.5f); // 0.5초 후 삭제
        }
    }
}

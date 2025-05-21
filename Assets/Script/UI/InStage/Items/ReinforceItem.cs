using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;

public class ReinforceItem : BaseItem
{
    private bool isCollision = false;


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.root.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {

            isCollision = true;
            Collider2D objCollider = GetComponent<Collider2D>();  // 
            objCollider.enabled = false;

            PlaySound();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();  // Rigidbody2D 참조

            rb.isKinematic = true;    //중력 & 물리적 반응 제거
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            GameStateManager.Instance.TakeReinforcementItem();
            Ver01_DungeonStatManager.Instance.UpdateHUD();
            Destroy(gameObject, 0.5f); // 0.5초 후 삭제
        }
    }

}

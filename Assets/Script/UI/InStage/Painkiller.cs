using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painkiller : MonoBehaviour
{
    private bool isCollision = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {
            isCollision = true;
            other.gameObject.GetComponent<PlayerHP>().GetPainKiller(30.0f);
            Destroy(gameObject, 0.5f); // 0.5초 후 삭제
        }
    }
}

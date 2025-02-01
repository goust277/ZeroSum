using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class ReinforceItem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player")) // 충돌한 오브젝트의 Collider 비교
        {
            GameStateManager.Instance.GetReinforcementItem();
            Destroy(gameObject, 1f); // 1초 후 삭제
        }
    }

}

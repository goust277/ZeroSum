using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;

public class ReinforceItem : MonoBehaviour
{
    private bool isCollision = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {

            isCollision = true;
            GameStateManager.Instance.TakeReinforcementItem();
            Ver01_DungeonStatManager.Instance.UpdateHUD();
            Destroy(gameObject, 0.5f); // 0.5초 후 삭제
        }
    }

}

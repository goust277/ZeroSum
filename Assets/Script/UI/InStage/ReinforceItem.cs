using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;

public class ReinforceItem : MonoBehaviour
{
    private Ver01_DungeonStatManager dungeonStatManager;
    private bool isCollision = false;

    private void Start()
    {
        dungeonStatManager ??= FindObjectsOfType<Ver01_DungeonStatManager>(true).FirstOrDefault();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {
            isCollision = true;
            GameStateManager.Instance.GetReinforcementItem();
            dungeonStatManager.UpdateHUD();
            Destroy(gameObject, 0.5f); // 1초 후 삭제
        }
    }

}

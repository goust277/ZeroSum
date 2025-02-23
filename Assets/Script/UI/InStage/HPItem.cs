using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPItem : MonoBehaviour
{
    private Ver01_DungeonStatManager dungeonStatManager;
    private bool isCollision = false;

    //private void Start()
    //{
    //    dungeonStatManager ??= FindObjectsOfType<Ver01_DungeonStatManager>(true).FirstOrDefault();
    //}
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {
            dungeonStatManager = other.gameObject.GetComponent<Ver01_DungeonStatManager>();
            isCollision = true;
            dungeonStatManager.GetHPItem();
            Destroy(gameObject, 0.5f); // 0.5초 후 삭제
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ATK : MonoBehaviour
{
    private bool isTriggerEnter = false;
    private SpriteRenderer spriteRenderer;
    private Ver01_DungeonStatManager dungeonStatManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dungeonStatManager ??= FindObjectsOfType<Ver01_DungeonStatManager>(true).FirstOrDefault();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggerEnter)
        {
            isTriggerEnter = true;

            dungeonStatManager.TakeDamage();
            //GameStateManager.Instance.GetHit();
            StartCoroutine(ResetAfterDelay());
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        spriteRenderer.color = Color.gray; // 색상을 회색으로 변경
        yield return new WaitForSeconds(1f); // 3초 대기
        spriteRenderer.color = Color.red; // 색상을 빨간색으로 변경
        isTriggerEnter = false; // 다시 타격 가능하게 변경
    }
}


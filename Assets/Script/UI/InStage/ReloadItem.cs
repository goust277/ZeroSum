using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReloadItem : MonoBehaviour
{
    private Ver01_DungeonStatManager dungeonStatManager;
    private bool isCollision = false;
    // Start is called before the first frame update

    [SerializeField] GameObject AmountText;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {
            dungeonStatManager = other.gameObject.GetComponent<Ver01_DungeonStatManager>();

            isCollision = true;
            dungeonStatManager.GetReloadItem();
            AmountText.gameObject.GetComponent<TextMeshPro>().text = "" + dungeonStatManager.GetDamageValue();
            Instantiate(AmountText, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.5f); // 1초 후 삭제
        }
    }
}

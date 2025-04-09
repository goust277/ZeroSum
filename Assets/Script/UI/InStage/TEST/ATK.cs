using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ATK : MonoBehaviour
{
    private bool isTriggerEnter = false;
    private SpriteRenderer spriteRenderer;
    //private Ver01_DungeonStatManager dungeonStatManager;

    [SerializeField] private List<GameObject> invisibleObj;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //dungeonStatManager ??= FindObjectsOfType<Ver01_DungeonStatManager>(true).FirstOrDefault();

        invisibleObj.Add(FindObjectOfType<PlayerInput>().gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggerEnter)
        {
            isTriggerEnter = true;
            Debug.Log($"대상: {other.name}");

            // 부모 찾기
            Transform playerTransform = other.transform;
            while (playerTransform.parent != null)
            {
                playerTransform = playerTransform.parent; // 부모로 이동
            }

            GameObject player = playerTransform.gameObject;
            invisibleObj.Add(player?.transform.Find("Collider")?.gameObject);
            invisibleObj.Add(player?.transform.Find("Sprite")?.gameObject);


            StartCoroutine(ShowObjects());
        }
    }

    private IEnumerator ShowObjects()
    {
        spriteRenderer.color = Color.gray; // 색상을 회색으로 변경
        yield return new WaitForSeconds(1f); // 3초 대기
        spriteRenderer.color = Color.red; // 색상을 빨간색으로 변경
        isTriggerEnter = false; // 다시 타격 가능하게 변경
    }
}


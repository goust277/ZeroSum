using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class FarmingDoor : MonoBehaviour
{
    private bool isTriggerEnter = false;
    [SerializeField] private GameObject dropItem;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTriggerEnter)
            {
                isTriggerEnter = true;
                spriteRenderer.color = Color.gray; // 색상을 회색으로 변경
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }
        }
    }
}
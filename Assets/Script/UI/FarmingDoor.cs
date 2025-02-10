using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class FarmingDoor : MonoBehaviour
{
    private bool isTriggerEnter = false;
    [SerializeField] private GameObject dropItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTriggerEnter)
            {
                isTriggerEnter = true;
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }
        }
    }
}
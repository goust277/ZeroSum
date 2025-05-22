using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_Num1Scene : CutSceneBase
{
    [SerializeField] private Collider2D[] evs;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            Debug.Log("BeforeEvUp - OnTriggerEnter2D");
        }
    }
}

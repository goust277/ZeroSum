using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingDoorActive : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            animator.SetBool("Active", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            animator.SetBool("Active", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.OnJumpInitiated += HandleJumpAnimation;
    }

    void Update()
    {
        if (playerMovement.isMove)
            animator.SetBool("Move", true);
        else
            animator.SetBool("Move", false);

        if (rb.velocity.y != 0)
            animator.SetBool("Fall", true);
        else
            animator.SetBool("Fall", false);
            
            

    }

    private void OnDisable()
    {
        if (playerMovement != null)
        {
            playerMovement.OnJumpInitiated -= HandleJumpAnimation;
        }
    }

    private void HandleJumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

}

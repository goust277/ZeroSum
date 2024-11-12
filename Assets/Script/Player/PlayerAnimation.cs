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

        playerMovement.OnJumpInitiated += JumpAnimation; //���� �̺�Ʈ
        playerMovement.OnDashInitiated += DashAnimation; //�뽬 �̺�Ʈ
    }

    void Update()
    {
        if (playerMovement.isMove) // ������ �ִϸ��̼�
            animator.SetBool("Move", true);
        else
            animator.SetBool("Move", false);

        if (rb.velocity.y != 0 && !playerMovement.isGrounded) // �������� ��� �ִϸ��̼�
            animator.SetBool("Fall", true);
        else
            animator.SetBool("Fall", false);
            
            

    }

    private void OnDisable() // �̺�Ʈ ����
    {
        if (playerMovement != null)
        {
            playerMovement.OnJumpInitiated -= JumpAnimation;
            playerMovement.OnDashInitiated -= DashAnimation;
        }
    }

    private void JumpAnimation() // ���� �ִϸ��̼�
    {
        animator.SetTrigger("Jump");
    }

    private void DashAnimation() // �뽬 �ִϸ��̼�
    {
        animator.SetTrigger("Dash");
    }
}

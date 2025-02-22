using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    [SerializeField] private RuntimeAnimatorController[] controller;
    [SerializeField] private bool OnBattleArea;

    private bool isMoveStart;
    private bool isDownStart;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.OnJumpInitiated += JumpAnimation; //���� �̺�Ʈ
        playerMovement.OnDashInitiated += DashAnimation; //�뽬 �̺�Ʈ
    }

    private void Start()
    {
        if (OnBattleArea)
            animator.runtimeAnimatorController = controller[0];
        isDownStart = false;
    }
    void Update()
    {
        if (playerMovement.isMove)
        {
            if (!isMoveStart)
            {
                isMoveStart = true;
                animator.SetTrigger("WalkStart");
            }
            animator.SetBool("Move", true);
            if (playerMovement.isDown)
            {

            }
            else
            {

            }
        }
        else
        {
            if (isMoveStart)
            {
                isMoveStart = false;
                animator.SetBool("Move", false);
            }
        }
        if (rb.velocity.y != 0 && !playerMovement.isGrounded) // �������� ��� �ִϸ��̼�
            animator.SetBool("Fall", true);
        else
            animator.SetBool("Fall", false);


        if (playerMovement.isDown)
        {
            if (!isDownStart)
            {
                isDownStart = true;
                animator.SetTrigger("DownStart");
            }
            animator.SetBool("Down", true);
        }
        else
        {
            animator.SetBool("Down", false);
            isDownStart = false;
        }
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
        animator.SetTrigger("Parrying");
    }
}

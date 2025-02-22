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

        playerMovement.OnJumpInitiated += JumpAnimation; //점프 이벤트
        playerMovement.OnDashInitiated += DashAnimation; //대쉬 이벤트
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
        if (rb.velocity.y != 0 && !playerMovement.isGrounded) // 떨어지는 모션 애니메이션
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

    private void OnDisable() // 이벤트 관리
    {
        if (playerMovement != null)
        {
            playerMovement.OnJumpInitiated -= JumpAnimation;
            playerMovement.OnDashInitiated -= DashAnimation;
        }
    }

    private void JumpAnimation() // 점프 애니메이션
    {
        animator.SetTrigger("Jump");
    }

    private void DashAnimation() // 대쉬 애니메이션
    {
        animator.SetTrigger("Parrying");
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private bool OnBattleArea;

    private bool isMoveStart;
    private bool isDownStart;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.OnJumpInitiated += JumpAnimation; //점프 이벤트
        playerMovement.OnDashInitiated += DashAnimation; //대쉬 이벤트
        playerMovement.OnTrueChanged += LandingAnimation;
        //playerMovement.OnBoolChanged += LandingAnimation;
    }

    private void Start()
    {
        isDownStart = false;
    }
    void Update()
    {
        if (playerMovement.isMove)
        {
            if (!isMoveStart)
            {
                isMoveStart = true;
                if (!playerMovement.isGrounded)
                    animator.SetTrigger("WalkStart");

            }
            if (playerMovement.isRun)
            {
                if(animator.GetBool("Move"))
                {
                    animator.SetBool("Move", false);
                }
                if(!animator.GetBool("Run"))
                { 
                    animator.SetBool("Run", true);

                }
            }
            else
            {
                if (!animator.GetBool("Move"))
                {
                    animator.SetBool("Move", true);
                }
                if (animator.GetBool("Run"))
                {
                    animator.SetBool("Run", false);
                }
            }

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

        if(Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("Attack");
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("Dying");
        }
    }

    private void OnDisable() // 이벤트 관리
    {
        if (playerMovement != null)
        {
            playerMovement.OnJumpInitiated -= JumpAnimation;
            playerMovement.OnDashInitiated -= DashAnimation;
            playerMovement.OnTrueChanged -= LandingAnimation;
        }
    }

    private void JumpAnimation() // 점프 애니메이션
    {
        Debug.Log("Jump");
        animator.SetTrigger("Jump");
    }

    private void DashAnimation() // 대쉬 애니메이션
    {
        animator.SetTrigger("Parrying");
    }

    private void LandingAnimation()
    {
        Debug.Log("Landing");
        animator.SetTrigger("Landing");
    }
}

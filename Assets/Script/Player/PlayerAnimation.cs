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

        playerMovement.OnJumpInitiated += JumpAnimation; //���� �̺�Ʈ
        playerMovement.OnDashInitiated += DashAnimation; //�뽬 �̺�Ʈ
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

        if(Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("Attack");
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("Dying");
        }
    }

    private void OnDisable() // �̺�Ʈ ����
    {
        if (playerMovement != null)
        {
            playerMovement.OnJumpInitiated -= JumpAnimation;
            playerMovement.OnDashInitiated -= DashAnimation;
            playerMovement.OnTrueChanged -= LandingAnimation;
        }
    }

    private void JumpAnimation() // ���� �ִϸ��̼�
    {
        Debug.Log("Jump");
        animator.SetTrigger("Jump");
    }

    private void DashAnimation() // �뽬 �ִϸ��̼�
    {
        animator.SetTrigger("Parrying");
    }

    private void LandingAnimation()
    {
        Debug.Log("Landing");
        animator.SetTrigger("Landing");
    }
}

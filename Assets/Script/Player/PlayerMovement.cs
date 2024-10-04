using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveDirection;
    private Rigidbody2D rb;


    [Header("이동")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("점프")]
    private bool isJumping; // 점프 중인지 확인
    [SerializeField] private float initialjumpForce = 7f; // 초기 점프 힘
    [SerializeField] private float maxJumpDuration = 0.3f; // 점프 지속 시간
    [SerializeField] private float fallMultiplier = 2.5f; // 
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float gravityScale = 5f; // 중력 값
    [SerializeField] private int extraJump = 1; // 추가 점프
    private float jumpTimeCounter;


    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (moveDirection != Vector2.zero)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        if (isJumping && jumpTimeCounter > 0)
        {
            /*float jumpForce = Mathf.Lerp(0, initialjumpForce, jumpTimeCounter);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpTimeCounter); */ // 2차 함수

            rb.velocity = new Vector2(rb.velocity.x, initialjumpForce * jumpTimeCounter);
            jumpTimeCounter -= Time.deltaTime / maxJumpDuration; // 1차 함수
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * gravityScale * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !isJumping)
        {
            rb.velocity += Vector2.up * gravityScale * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input != null) 
        {
            moveDirection = new Vector2 (input.x, input.y);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && (IsGrounded() || extraJump > 0))
        {
            isJumping = true;
            jumpTimeCounter =1f;
            rb.velocity = new Vector2(rb.velocity.x, initialjumpForce);

            if (!IsGrounded())
                extraJump--;
        }
        else if (context.canceled)
        {
            isJumping = false;
        }
    }


    private bool IsGrounded()
    {
        


        return true; // 임시로 항상 true 반환
    }

}
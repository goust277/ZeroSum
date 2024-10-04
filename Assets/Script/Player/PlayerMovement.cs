
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveDirection;
    private Rigidbody2D rb;


    [Header("이동")]
    [SerializeField] private float moveSpeed = 5f;
    private bool isMove;

    [Header("점프")]
    private bool isJumping; // 점프 중인지 확인
    [SerializeField] private float initialjumpForce = 7f; // 초기 점프 힘
    [SerializeField] private float maxJumpDuration = 0.3f; // 점프 지속 시간
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float gravityScale = 5f; // 중력 값
    [SerializeField] private int extraJump = 1; // 추가 점프
    private int extraJumpCurr = 0;

    [SerializeField] private float coyoteTime = 0.3f; // 코요태 시간
    private float coyoteTimeCurr = 0f;
    private float jumpTimeCounter;

    [Header("바닥체크")]
    private bool isGrounded;
    [SerializeField] private LayerMask groundLayer; // 바닥 레이어
    [SerializeField] private float groundBoxOffset = 0f; // 바닥 판정 오프셋
    [SerializeField] private Vector2 groundBox = Vector2.zero; // 바닥 판정 박스
    [SerializeField] private float groundCheckDistance = 0.5f; // 바닥 판정 거리


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Vector2 center = transform.position;
        center.y += groundBoxOffset;
        Gizmos.DrawCube(center, groundBox);
    }
#endif

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    // Update is called once per frame
    private void Update()
    {
        GroundCheck();
    }
    private void FixedUpdate()
    {
        if (moveDirection != Vector2.zero)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime, rb.velocity.y);
        }
        if (isJumping && jumpTimeCounter > 0)
        {
            /*float jumpForce = Mathf.Lerp(0, initialjumpForce, jumpTimeCounter);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpTimeCounter); */ // 2차 함수

            rb.velocity = new Vector2(rb.velocity.x, initialjumpForce);
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
            isMove = true;
            moveDirection = new Vector2 (input.x, 0);
        }
        else
            isMove = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && (isGrounded || extraJumpCurr < extraJump))
        {
            isJumping = true;
            jumpTimeCounter =1f;
            rb.velocity = new Vector2(rb.velocity.x, initialjumpForce);

            if (!isGrounded)
                extraJumpCurr++;
        }
        else if (context.canceled)
        {
            isJumping = false;
        }
    }


    private void GroundCheck()
    {
        Vector2 groundBoxCenter = transform.position;
        groundBoxCenter.y = groundBoxOffset;

        if (!Physics2D.BoxCast(groundBoxCenter, groundBox, 0f, Vector2.down, groundCheckDistance, groundLayer))
        {
            if (coyoteTimeCurr < coyoteTime)
                coyoteTimeCurr += Time.deltaTime;
            else
                isGrounded = false;
        }
        else if (rb.velocity.y <= 0)
        {
            isGrounded = true;
            coyoteTimeCurr = 0f;
            extraJumpCurr = 0;
        }

    }

}
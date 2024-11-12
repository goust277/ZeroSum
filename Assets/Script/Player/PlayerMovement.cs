
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private bool isPortalReady = false;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Animator animator;

    [HideInInspector] public event System.Action OnJumpInitiated; // 점프 이벤트
    [HideInInspector] public event System.Action OnDashInitiated; // 대쉬 이벤트

    [Header("이동")]
    [SerializeField] private float moveSpeed = 5f; //이동속도
    [HideInInspector] public bool isMove; // 이동 중 인지 확인
    private bool moveRight;

    [Header("점프")]
    [SerializeField] private float initialjumpForce = 7f; // 초기 점프 힘
    [SerializeField] private float maxJumpDuration = 0.3f; // 점프 지속 시간
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float gravityScale = 5f; // 중력 값
    [SerializeField] private int extraJump = 1; // 추가 점프
    private float currY_velocity;
    private bool isJumping; // 점프 중인지 확인
    private int extraJumpCurr = 0;

    [SerializeField] private float coyoteTime = 0.3f; // 코요태 시간
    private float coyoteTimeCurr = 0f;
    private float jumpTimeCounter;


    [Header("대쉬")]
    [SerializeField] private float dashPower = 3f; // 대쉬 순간 힘
    [SerializeField] private float dashDuration = 0.3f; // 대쉬 지속시간
    [SerializeField] private float dashCoolTime = 1f; // 대쉬 쿨타임
    private bool isDashReady;

    [SerializeField] private float dashcurrentCoolTime; //현재 대쉬 쿨타임
    private float dashTime; // 대쉬하고 있는 시간
    private bool canDash; // 대쉬 가능한지 확인
    [SerializeField] private bool isDashing; // 대쉬 중 인지 확인

    [Header("바닥체크")]
    [SerializeField] private LayerMask groundLayer; // 바닥 레이어
    [SerializeField] private float groundBoxOffset = 0f; // 바닥 판정 오프셋
    [SerializeField] private Vector2 groundBox = Vector2.zero; // 바닥 판정 박스
    [SerializeField] private float groundCheckDistance = 0.5f; // 바닥 판정 거리
    [HideInInspector] public bool isGrounded; // 바닥인지 확인

#if UNITY_EDITOR
    private void OnDrawGizmos() // 플레이어 바닥 판정 확인
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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        GroundCheck();

        if (isGrounded && !isMove)
        { rb.bodyType = RigidbodyType2D.Static; }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    private void FixedUpdate()
    {

        if (moveDirection != Vector2.zero) // 움직임
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime, rb.velocity.y);

            if (moveDirection == Vector2.right && moveRight)
            {
                Flip();
            }
            else if (moveDirection == Vector2.left && !moveRight)
            {
                Flip();
            }
        }
        else
        {
            isMove = false;
        }

        Jump();
        Dash();
    }

    private void Jump()// 점프
    {
        if (isJumping && jumpTimeCounter > 0)
        {
            float jumpForce = Mathf.Lerp(0, initialjumpForce, jumpTimeCounter);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpTimeCounter);
            if (rb.velocity.y < 3f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                isJumping = false;
            } // 점프 2차 함수

            //rb.velocity = new Vector2(rb.velocity.x, initialjumpForce);// 1차 함수
            jumpTimeCounter -= Time.deltaTime / maxJumpDuration;
        }

        if (rb.velocity.y < 0 && !isDashing)
        {
            rb.velocity += Vector2.up * gravityScale * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !isJumping && !isDashing)
        {
            rb.velocity += Vector2.up * gravityScale * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }



    private void Flip() // 플레이어 좌우 회전
    {
        moveRight = !moveRight;
        Vector2 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    public void OnMove(InputAction.CallbackContext context) // 플레이어 이동 입력
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input != null && !isDashing)
        {
            isMove = true;
            moveDirection = new Vector2(input.x, 0);
        }
        else
        {

        }
    }

    public void OnJump(InputAction.CallbackContext context) // 플레이어 점프
    {
        if (context.started && (isGrounded || extraJumpCurr < extraJump))
        {
            isJumping = true;
            jumpTimeCounter = 1f;
            rb.velocity = new Vector2(rb.velocity.x, initialjumpForce);
            coyoteTimeCurr = coyoteTime;

            if (!isGrounded)
                extraJumpCurr++;

            OnJumpInitiated?.Invoke();
        }
        else if (context.canceled)
        {
            isJumping = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context) // 플레이어 대쉬
    {
        if (context.started && canDash && isDashReady)
        {
            StartDash();
            OnDashInitiated?.Invoke();
        }
    }

    private void Dash()// 대쉬
    {
        if (isDashing)
        {
            Dashing();
        }
        if (!canDash)
        {
            dashcurrentCoolTime += Time.deltaTime;

            if (dashcurrentCoolTime >= dashCoolTime)
            {
                canDash = true;
            }
        }
    }

    private void StartDash() // 대쉬 시작
    {
        isDashing = true;
        canDash = false;
        isJumping = false;

        currY_velocity = rb.velocity.y;

        dashTime = 0f;
        dashcurrentCoolTime = 0f;
    }

    private void Dashing() // 대쉬 중
    {
        dashTime += Time.deltaTime;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0);
        rb.gravityScale = 0f;

        if (dashTime >= dashDuration)
        {
            EndDash();
        }
    }

    private void EndDash() // 대쉬 끝
    {
        isDashing = false;
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = gravityScale;

        if (!isGrounded)
        {
            isDashReady = false;
        }
    }

    private void GroundCheck()
    {
        Vector2 groundBoxCenter = transform.position;
        groundBoxCenter.y += groundBoxOffset;

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

            if (!isDashReady)
                isDashReady = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Portal")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                collision.GetComponent<Portal>().OnPortal();
                Debug.Log("E");
            }

        }
    }
}
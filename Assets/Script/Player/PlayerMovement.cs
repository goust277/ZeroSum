
using System;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private bool isPortalReady = false;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private PlayerSwordAttack playerSword;

    public event System.Action OnJumpInitiated;
    public event System.Action OnDashInitiated; 
    public event Action OnTrueChanged;
    public event Action OnStand;


    [Header("Sprite")]
    //[SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject sprite;

    [Header("move")]
    [SerializeField] private float moveSpeed = 5f;
    [HideInInspector] public bool isMove;
    private float moveDelay = 0.02f;
    private float moveTime = 0f;
    private Vector2 input;
    private float lastDirectionX = 0f;
    [HideInInspector] public bool moveLeft;

    [Header("run")]
    [SerializeField] private float runMoveSpeed = 8f;
    [HideInInspector] public bool isRun = false;

    [Header("jump")]
    [SerializeField] private float initialjumpForce = 7f;
    [SerializeField] private float maxJumpDuration = 0.3f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float gravityScale = 5f;
    [SerializeField] private int extraJump = 1;
    private float currY_velocity;
    private bool isJumping;
    private int extraJumpCurr = 0;

    [SerializeField] private float coyoteTime = 0.3f;
    private float coyoteTimeCurr = 0f;
    private float jumpTimeCounter;


    [Header("parring")]
    [SerializeField] private float dashPower = 3f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashCoolTime = 1f;
    private bool isDashReady;

    [SerializeField] private float dashcurrentCoolTime;
    private float dashTime;
    private bool canDash;
    [SerializeField] private bool isDashing;

    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundBoxOffset = 0f;
    [SerializeField] private Vector2 groundBox = Vector2.zero;
    [SerializeField] private float groundCheckDistance = 0.5f;
    [HideInInspector] public bool isGrounded; 
    private bool wasGrounded;

    [HideInInspector] public bool isDown;

    [Header("Collider")]
    [SerializeField] private GameObject standCollider;
    [SerializeField] private GameObject downCollider;


    private Transform originalParent;
    private Vector3 lastPlatformPosition;
    private PlayerGunAttack playerGun;


    private bool isButtonReleased = false; // 버튼 릴리스 상태 추적
    private bool wasAttacking = false; // 이전 프레임 공격 상태

    private bool isDownBtn = false;
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
        playerSword = GetComponent<PlayerSwordAttack>();
        playerGun = GetComponent<PlayerGunAttack>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isDown = false;

        OnTrueChanged += OnLanding;
    }

    // Update is called once per frame
    [Obsolete]
    private void Update()
    {
        GroundCheck();
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        if (rb.velocity == Vector2.zero)
        {
            //isRun = false;
        }

        bool isCurGround = isGrounded;

        if (!wasGrounded && isGrounded)
        {
            OnTrueChanged?.Invoke();
        }

        wasGrounded = isCurGround;

        if (isDown)
        {
            if(!downCollider.active)
            {
                standCollider.SetActive(false);
                downCollider.SetActive(true);
            }
        }
        else
        {
            if (!standCollider.active)
            {
                standCollider.SetActive(true);
                downCollider.SetActive(false);
            }

        }

        if (!isAttack() && isButtonReleased)
        {
            if (isDownBtn)
            {
                isDown = true;
                isButtonReleased = false;
            }
            else
            {
                if (isDown)
                {
                    OnStand?.Invoke();
                    isDown = false;
                    isButtonReleased = false;
                }
            }

        }

    }
    private void FixedUpdate()
    {
        if (!isAttack())
        {
            if (isMove)//moveDirection != Vector2.zero) // ������
            {
                if(isRun)
                {
                    rb.velocity = new Vector2(moveDirection.x * runMoveSpeed, rb.velocity.y);
                }
                else
                {
                    //transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
                    if (moveDirection.x != 0)
                    {
                            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);

                    }
                }


                if (moveDirection == Vector2.right && moveLeft)
                {
                    Flip();
                }
                else if (moveDirection == Vector2.left && !moveLeft)
                {
                    Flip();
                }
            }
            else
            {
                //isMove = false;
            }

            Jump();
            Dash();
        }
    }

    private void OnDisable()
    {
        OnTrueChanged -= OnLanding;
    }
    private void Jump()// ����
    {
        if (isAttack())
        {
            return;
        }
        if (isJumping && jumpTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, initialjumpForce);
            jumpTimeCounter -= Time.unscaledDeltaTime;

            // velocity.y 임계값도 TimeScale 영향 없이 고정
            if (rb.velocity.y < 3f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                isJumping = false;
            }

            // 점프 시간 카운터 감소 (Time.unscaledDeltaTime 사용)
            jumpTimeCounter -= Time.unscaledDeltaTime / maxJumpDuration;
        }

        // 중력 적용 (FixedUpdate에서 Time.fixedDeltaTime 사용)
        if (rb.velocity.y < 0 && !isDashing)
        {
            rb.velocity += Vector2.up * gravityScale * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !isJumping && !isDashing)
        {
            rb.velocity += Vector2.up * gravityScale * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    private void Flip() // �÷��̾� �¿� ȸ��
    {
        moveLeft = !moveLeft;
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        
        if (!moveLeft)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            
        }

        //float currentPosition = sprite.transform.localPosition.x; // ���� ��ġ ��������
        //currentPosition *= -1; // x�� �� ����
        //sprite.transform.localPosition = new Vector3(currentPosition, 0, 0);
        ////////////////////////////////////////////////////////////////////////////
        //Vector2 currentScale = transform.localScale;
        //currentScale.x *= -1;
        //transform.localScale = currentScale;
    }

    public void Mo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isMove = true;
        }
        if (context.canceled)
        {
            isMove = false;
            //isRun = false;
        }
    }
    public void OnMove(InputAction.CallbackContext context) 
    {
        input = context.ReadValue<Vector2>();

        if (!isDashing && input.y == 0 && input.x != 0)
        {
            isMove = true;
            moveDirection = new Vector2(input.x, 0);

            if (Mathf.Sign(input.x) != Mathf.Sign(lastDirectionX) && lastDirectionX != 0)
            {

            }

            lastDirectionX = input.x;
        }

    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.started && (isGrounded || extraJumpCurr < extraJump) && !isAttack())
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

    public void OnDash(InputAction.CallbackContext context) // �÷��̾� �뽬
    {
        if (context.started && canDash && isDashReady && !isAttack())
        {
            StartDash();
            OnDashInitiated?.Invoke();
        }
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        //if (isAttack())
        //{
        //    return;
        //}
        //if (context.started)
        //{
        //    isDown = true;
        //}
        //if (context.canceled && isDown)
        //{
        //    OnStand?.Invoke();
        //    isDown = false;
        //}


        if (context.started )
        {
            isButtonReleased = true;
            isDownBtn = true;
        }
        if (context.canceled && isDownBtn)
        {
            isButtonReleased = true;
            isDownBtn = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isRun = true;
        }
        if(context.canceled)
        {
            isRun = false;
        }
    }

    private void Dash()// �뽬
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

    private void StartDash() // �뽬 ����
    {
        isDashing = true;
        canDash = false;
        isJumping = false;

        dashTime = 0f;
        dashcurrentCoolTime = 0f;
    }

    private void Dashing() // �뽬 ��
    {
        dashTime += Time.deltaTime;
        if(moveLeft)
            rb.velocity = new Vector2(transform.localScale.x * dashPower * -1, rb.velocity.y);
        else
            rb.velocity = new Vector2(transform.localScale.x * dashPower, rb.velocity.y);
        rb.gravityScale = 0f;

        if (dashTime >= dashDuration)
        {
            EndDash();
        }
    }

    private void EndDash() // �뽬 ��
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
        if(collision.CompareTag("MovingBlock"))
        {
            transform.parent = collision.transform;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingBlock"))
        {
            if (transform.parent != null)
                transform.parent = null;
        }
    }
    private bool isAttack()
    {
        if (playerSword.isAttack || playerGun.isAttack)
        {
            return true;
        }
        return false;
    }

    private void OnLanding()
    {
        isGrounded = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
    }

    private void OnEnable()
    {
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
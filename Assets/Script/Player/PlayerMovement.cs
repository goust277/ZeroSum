
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
        if (isJumping && jumpTimeCounter > 0)
        {
            float jumpForce = Mathf.Lerp(0, initialjumpForce, jumpTimeCounter);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpTimeCounter);
            if (rb.velocity.y < 3f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                isJumping = false;
            }

            //rb.velocity = new Vector2(rb.velocity.x, initialjumpForce);
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
        if(context.started)
        {
            Debug.Log("Press");
            isDown = true;
        }
        if (context.canceled)
        {
            Debug.Log("Release");
            OnStand?.Invoke();
            isDown = false;
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
}
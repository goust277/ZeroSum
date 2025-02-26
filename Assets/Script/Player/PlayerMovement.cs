
using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private bool isPortalReady = false;
    [SerializeField] private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerSwordAttack playerSword;
    private SpriteRenderer sprite;

    [HideInInspector] public event System.Action OnJumpInitiated; // ���� �̺�Ʈ
    [HideInInspector] public event System.Action OnDashInitiated; // �뽬 �̺�Ʈ

    [Header("�̵�")]
    [SerializeField] private float moveSpeed = 5f; //�̵��ӵ�
    [HideInInspector] public bool isMove; // �̵� �� ���� Ȯ��
    private float moveDelay = 0.02f;
    private float moveTime = 0f;
    private Vector2 input;
    private float lastDirectionX = 0f;
   private bool moveLeft;

    [Header("�޸���")]
    [SerializeField] private float runMoveSpeed = 8f;
    [HideInInspector] public bool isRun = false;

    [Header("����")]
    [SerializeField] private float initialjumpForce = 7f; // �ʱ� ���� ��
    [SerializeField] private float maxJumpDuration = 0.3f; // ���� ���� �ð�
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float gravityScale = 5f; // �߷� ��
    [SerializeField] private int extraJump = 1; // �߰� ����
    private float currY_velocity;
    private bool isJumping; // ���� ������ Ȯ��
    private int extraJumpCurr = 0;

    [SerializeField] private float coyoteTime = 0.3f; // �ڿ��� �ð�
    private float coyoteTimeCurr = 0f;
    private float jumpTimeCounter;


    [Header("�뽬")]
    [SerializeField] private float dashPower = 3f; // �뽬 ���� ��
    [SerializeField] private float dashDuration = 0.3f; // �뽬 ���ӽð�
    [SerializeField] private float dashCoolTime = 1f; // �뽬 ��Ÿ��
    private bool isDashReady;

    [SerializeField] private float dashcurrentCoolTime; //���� �뽬 ��Ÿ��
    private float dashTime; // �뽬�ϰ� �ִ� �ð�
    private bool canDash; // �뽬 �������� Ȯ��
    [SerializeField] private bool isDashing; // �뽬 �� ���� Ȯ��

    [Header("�ٴ�üũ")]
    [SerializeField] private LayerMask groundLayer; // �ٴ� ���̾�
    [SerializeField] private float groundBoxOffset = 0f; // �ٴ� ���� ������
    [SerializeField] private Vector2 groundBox = Vector2.zero; // �ٴ� ���� �ڽ�
    [SerializeField] private float groundCheckDistance = 0.5f; // �ٴ� ���� �Ÿ�
    [HideInInspector] public bool isGrounded; // �ٴ����� Ȯ��

    [Header("�ɱ�")]
    public bool isDown;

#if UNITY_EDITOR
    private void OnDrawGizmos() // �÷��̾� �ٴ� ���� Ȯ��
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
        sprite = GetComponent<SpriteRenderer>();
        playerSword = GetComponent<PlayerSwordAttack>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isDown = false;
    }

    // Update is called once per frame
    private void Update()
    {
        GroundCheck();

    }
    private void FixedUpdate()
    {
        if (!isAttack())
        {
            if (isMove)//moveDirection != Vector2.zero) // ������
            {
                if(isRun)
                {
                    transform.Translate(moveDirection * runMoveSpeed * Time.deltaTime);
                }
                else
                {
                    //transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
                    rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
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
            } // ���� 2�� �Լ�

            //rb.velocity = new Vector2(rb.velocity.x, initialjumpForce);// 1�� �Լ�
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
        sprite.flipX = true;
        if (!moveLeft)
        {
            sprite.flipX = false;
        }
        //Vector2 currentScale = transform.localScale;
        //currentScale.x *= -1;
        //transform.localScale = currentScale;
    }

    public void OnMove(InputAction.CallbackContext context) // �÷��̾� �̵� �Է�
    {
        input = context.ReadValue<Vector2>();

        // �Է��� ��ȿ�ϰ� �뽬 ���� �ƴϸ� y���� 0�� ��� �̵� Ȱ��ȭ
        if (!isDashing && input.y == 0 && input.x != 0)
        {
            isMove = true;
            moveDirection = new Vector2(input.x, 0);

            // ���� ��ȯ ���� (x ���� ��ȣ�� �ٲ������ Ȯ��)
            if (Mathf.Sign(input.x) != Mathf.Sign(lastDirectionX) && lastDirectionX != 0)
            {
                Debug.Log("���� ��ȯ!");
                // ���� ��ȯ �� �߰� ������ ó���� �� �ֽ��ϴ�.
            }

            // ���� x ���� ���� ����
            lastDirectionX = input.x;
        }

        // �Է��� ������ ������ ���� �̵� ����
        if (input == Vector2.zero)
        {
            isMove = false;
            isRun = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context) // �÷��̾� ����
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

        currY_velocity = rb.velocity.y;

        dashTime = 0f;
        dashcurrentCoolTime = 0f;
    }

    private void Dashing() // �뽬 ��
    {
        dashTime += Time.deltaTime;
        if(moveLeft)
            rb.velocity = new Vector2(transform.localScale.x * dashPower * -1, 0);
        else
            rb.velocity = new Vector2(transform.localScale.x * dashPower, 0);
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
        if (collision.name == "Portal")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                collision.GetComponent<Portal>().OnPortal();
            }

        }
        if (collision.CompareTag("InteractDoor"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                collision.GetComponent<InteractDoor>().OnInteract();
            }
        }
    }

    private bool isAttack()
    {
        if (playerSword.isAttack)
        {
            return true;
        }
        return false;
    }
}
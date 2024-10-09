
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Animator animator;
    [HideInInspector] public event System.Action OnJumpInitiated;

    [Header("�̵�")]
    [SerializeField] private float moveSpeed = 5f;
    [HideInInspector] public bool isMove;
    private bool moveRight;

    [Header("����")]
    private bool isJumping; // ���� ������ Ȯ��
    [SerializeField] private float initialjumpForce = 7f; // �ʱ� ���� ��
    [SerializeField] private float maxJumpDuration = 0.3f; // ���� ���� �ð�
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float gravityScale = 5f; // �߷� ��
    [SerializeField] private int extraJump = 1; // �߰� ����
    private int extraJumpCurr = 0;

    [SerializeField] private float coyoteTime = 0.3f; // �ڿ��� �ð�
    private float coyoteTimeCurr = 0f;
    private float jumpTimeCounter;

    [Header("�ٴ�üũ")]
    [SerializeField] private LayerMask groundLayer; // �ٴ� ���̾�
    [SerializeField] private float groundBoxOffset = 0f; // �ٴ� ���� ������
    [SerializeField] private Vector2 groundBox = Vector2.zero; // �ٴ� ���� �ڽ�
    [SerializeField] private float groundCheckDistance = 0.5f; // �ٴ� ���� �Ÿ�
    private bool isGrounded;

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
        animator = GetComponent<Animator>();
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

        if (isJumping && jumpTimeCounter > 0)
        {
            float jumpForce = Mathf.Lerp(0, initialjumpForce, jumpTimeCounter);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpTimeCounter); 
            if (rb.velocity.y < 3f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                isJumping = false;
            } // 2�� �Լ�

            //rb.velocity = new Vector2(rb.velocity.x, initialjumpForce);// 1�� �Լ�
            jumpTimeCounter -= Time.deltaTime / maxJumpDuration;
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

    private void Flip()
    {
        moveRight = !moveRight;
        Vector2 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
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
        {

        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && (isGrounded || extraJumpCurr < extraJump))
        {
            isJumping = true;
            jumpTimeCounter =1f;
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
        }

    } 

}
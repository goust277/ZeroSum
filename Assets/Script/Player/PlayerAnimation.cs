using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerGunAttack playerGunAttack;
    private PlayerSwordAttack playerSwordAttack;
    private PlayerHP playerHp;
    private Rigidbody2D rb;

    private bool isDownStart;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerGunAttack = GetComponent<PlayerGunAttack>();
        playerSwordAttack = GetComponent<PlayerSwordAttack>();
        playerHp = GetComponent<PlayerHP>();

        playerMovement.OnJumpInitiated += JumpAnimation; //���� �̺�Ʈ
        playerMovement.OnDashInitiated += DashAnimation; //�뽬 �̺�Ʈ
        playerMovement.OnTrueChanged += LandingAnimation;
        playerMovement.OnStand += StandAnimation;
        playerGunAttack.OnGunAttack += GunAttackAnimation;
        playerGunAttack.OnFirstGunAttack += FirstGunAttackAnimation;
        playerSwordAttack.OnSwordAttack += SwordAttack;
        playerSwordAttack.OnSwordSecAttack += SwordSecAttack;
        playerHp.OnDying += Dying;
    }

    private void Start()
    {
        isDownStart = false;
    }
    void Update()
    {
        if (playerMovement.isMove)
        {
            if (!animator.GetBool("Move"))
                animator.SetBool("Move", true);
        }
        else
        {
            if (animator.GetBool("Move"))
                animator.SetBool("Move", false);
        }

        if (playerMovement.isRun)
        {
            if (!animator.GetBool("Run"))
                animator.SetBool("Run", true);
        }
        else
        {
            if (animator.GetBool("Run"))
                animator.SetBool("Run", false);
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
    }

    private void OnDisable() // �̺�Ʈ ����
    {
        if (playerMovement != null)
        {
            playerMovement.OnJumpInitiated -= JumpAnimation;
            playerMovement.OnDashInitiated -= DashAnimation;
            playerMovement.OnTrueChanged -= LandingAnimation;
            playerMovement.OnStand -= StandAnimation;
            playerGunAttack.OnGunAttack -= GunAttackAnimation;
            playerGunAttack.OnFirstGunAttack -= FirstGunAttackAnimation;
            playerSwordAttack.OnSwordAttack -= SwordAttack;
            playerSwordAttack.OnSwordSecAttack -= SwordSecAttack;
            playerHp.OnDying -= Dying;
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
        animator.SetTrigger("Landing");
    }

    private void StandAnimation()
    {
        animator.SetTrigger("Stand");
    }

    private void GunAttackAnimation()
    {
        animator.SetTrigger("GunAttack");
    }

    private void FirstGunAttackAnimation()
    {
        animator.SetTrigger("GunAttackStart");
    }

    private void SwordAttack()
    {
        animator.SetTrigger("Attack");
    }

    private void SwordSecAttack()
    {
        animator.SetTrigger("Attack2");
    }
    private void Dying()
    {
        animator.SetTrigger("Dying");
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
    }
}

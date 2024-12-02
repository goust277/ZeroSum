using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private int comboStep = 0; // ���� �޺� �ܰ�
    private float comboTimer = 0f; // �޺� Ÿ�̸�
    [SerializeField] private float maxComboDelay = 1f; // ���� ���� �Է� ���� �ð�
    [SerializeField] private float attackDelay = 0.5f; // ���� ���� �ð�

    private bool isAttacking = false; // ���� �� ����
    private bool isFirstAtk = false; // ù ��������
    private float attackDelayTimer = 0f; //���� ���� Ÿ�̸�

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        else if (comboTimer <= 0)
        {
            // Ÿ�̸Ӱ� ������ �޺� �ʱ�ȭ
            comboStep = 0;
            isFirstAtk = false;
        }

        //if (isAttacking)
        //{
        //    attackDelayTimer -= Time.deltaTime;
        //    if (attackDelayTimer <= 0)
        //    {
        //        ExecuteAttack();
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.A)) 
        {
            animator.SetTrigger("AttackUp");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("AttackDown");
        }
    }

    public void OnAttack(InputAction.CallbackContext context) // �÷��̾� �뽬
    {
        if (context.started)
        {
            PerformCombo();
        }
    }

    private void PerformCombo()
    {
        comboStep++;
        comboTimer = maxComboDelay; // Ÿ�̸� �ʱ�ȭ

        if (comboStep == 1)
        {
            animator.SetTrigger("Attack1");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack2");
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("Attack3");
            comboStep = 0; // ������ ���� �� �ʱ�ȭ
        }
    }

    private void StartCombo()
    {
        if (isAttacking)
        {
            if (attackDelayTimer <= 0)
            {
                comboStep++;
            }
        }
        
        if (!isFirstAtk)
        {
            comboStep = 1;
            isFirstAtk = true;
        }
        
        comboTimer = maxComboDelay; // �޺� �Է� ���� �ð� �ʱ�ȭ

        if (comboStep > 3)
        {
            comboStep = 1; // �ִ� �޺� �ܰ�� 3�ܰ�, ���� �ʱ�ȭ
        }
        
        isAttacking = true;
        attackDelayTimer = attackDelay; // ���� Ÿ�̸� ����
    }
    private void ExecuteAttack()
    {
        isAttacking = false; // ���� �Ϸ� ó��
        

        // ���� �޺� �ܰ迡 �´� �ִϸ��̼� Ʈ���� ����
        if (comboStep == 1)
        {
            animator.SetTrigger("Attack1");
            Debug.Log("1�ܰ� ���� ����");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack2");
            Debug.Log("2�ܰ� ���� ����");
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("Attack3");
            Debug.Log("3�ܰ� ���� ����");
            comboStep = 0; // ������ ���� �� �ʱ�ȭ
        }
    }
}

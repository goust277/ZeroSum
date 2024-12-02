using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private int comboStep = 0; // 현재 콤보 단계
    private float comboTimer = 0f; // 콤보 타이머
    [SerializeField] private float maxComboDelay = 1f; // 다음 공격 입력 가능 시간
    [SerializeField] private float attackDelay = 0.5f; // 공격 지연 시간

    private bool isAttacking = false; // 공격 중 인지
    private bool isFirstAtk = false; // 첫 공격인지
    private float attackDelayTimer = 0f; //공격 지연 타이머

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
            // 타이머가 끝나면 콤보 초기화
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

    public void OnAttack(InputAction.CallbackContext context) // 플레이어 대쉬
    {
        if (context.started)
        {
            PerformCombo();
        }
    }

    private void PerformCombo()
    {
        comboStep++;
        comboTimer = maxComboDelay; // 타이머 초기화

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
            comboStep = 0; // 마지막 공격 후 초기화
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
        
        comboTimer = maxComboDelay; // 콤보 입력 가능 시간 초기화

        if (comboStep > 3)
        {
            comboStep = 1; // 최대 콤보 단계는 3단계, 이후 초기화
        }
        
        isAttacking = true;
        attackDelayTimer = attackDelay; // 지연 타이머 시작
    }
    private void ExecuteAttack()
    {
        isAttacking = false; // 공격 완료 처리
        

        // 현재 콤보 단계에 맞는 애니메이션 트리거 실행
        if (comboStep == 1)
        {
            animator.SetTrigger("Attack1");
            Debug.Log("1단계 공격 실행");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack2");
            Debug.Log("2단계 공격 실행");
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("Attack3");
            Debug.Log("3단계 공격 실행");
            comboStep = 0; // 마지막 공격 후 초기화
        }
    }
}

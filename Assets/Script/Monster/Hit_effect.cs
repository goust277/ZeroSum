using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_effect : MonoBehaviour
{
    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            float animTime = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, animTime);
        }
        else
        {
            Destroy(gameObject, 1f); // �ִϸ����Ͱ� ������ �⺻������ 1�� �� ����
        }
    }
}

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
            Destroy(gameObject, 1f); // 애니메이터가 없으면 기본적으로 1초 후 삭제
        }
    }
}

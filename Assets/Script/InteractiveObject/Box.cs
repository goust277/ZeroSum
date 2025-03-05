using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDamageAble
{
    private int hitCount;
    [Header("박스 애니메이션")]
    [SerializeField] private Animator animator;

    private bool isOpen;
    void Start()
    {
        hitCount = 0;
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hitCount == 2)
        {
            if (!isOpen)
                isOpen = true;
        }
    }

    public void Damage(int value)
    {
        if(!isOpen)
        {
            hitCount++;
            animator.SetInteger("Hit", hitCount);
            animator.SetTrigger("IsHit");
        }

    }

    public void OpenBox()
    {
        gameObject.SetActive(false);
    }
}

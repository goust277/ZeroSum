using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDamageAble
{
    public int hitCount;
    [Header("drop items")]
    [SerializeField] private GameObject[] dropItemList;

    [Header("박스 애니메이션")]
    [SerializeField] private Animator animator;

    [Header("박스 오디오")]
    [SerializeField] private AudioSource audioSource;

    private bool isOpen;
    void Start()
    {
        hitCount = 0;
        isOpen = false;
    }

    public void Damage(int value)
    {
        if(!isOpen)
        {
            hitCount++;
            animator.SetInteger("Hit", hitCount);
            animator.SetTrigger("IsHit");
        }

        if (hitCount == 2)
        {
            isOpen = true;
        }
    }

    public void OpenBox()
    {
        int dropIndex = Random.Range(0, dropItemList.Length);
        audioSource.Play();
        Instantiate(dropItemList[dropIndex], transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}

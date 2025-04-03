using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EvAniamation : MonoBehaviour
{
    private float startAnimation;
    private float rand = 0f;
    private bool isAnimationStart;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        startAnimation = Random.Range(0, 2);
        animator.enabled = false;
        isAnimationStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startAnimation > rand) 
        {
            rand += Time.deltaTime;
        }
        else if (startAnimation <= rand)
        {
            if (!isAnimationStart)
            {
                StartAnimaion();
            }
        }
    }

    private void StartAnimaion()
    {
        animator.enabled = true;
        isAnimationStart = true;
    }
}

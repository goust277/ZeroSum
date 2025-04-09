using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvBtn : BaseInteractable
{
    [SerializeField] private ControlEv target;
    [SerializeField] private int num;

    [SerializeField] private Animator animator;

    private void Update()
    {
        if (target.isUp)
        {
            if(!animator.GetBool("Up"))
            {
                animator.SetBool("Up",true);
                animator.SetBool("Down", false);
            }
        }
        else
        {
            if (!animator.GetBool("Down"))
            {
                animator.SetBool("Down", true);
                animator.SetBool("Up", false);
            }
        }
    }
    public override void Exe()
    {
        if (target == null)
            return;
        target.btnNum = num;
        target.Exe();

        Debug.Log("버튼 상호작용");
    }
}

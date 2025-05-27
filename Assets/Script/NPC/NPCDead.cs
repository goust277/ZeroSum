using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDead : MonoBehaviour, IState<NPCController>
{
    [SerializeField] private NPCController controller;
    public void OperateEnter(NPCController sender)
    {
        controller = sender;
        controller.animator.SetTrigger("Die");
    }

    public void OperateExit(NPCController sender)
    {

    }

    public void OperateUpdate(NPCController sender)
    {
        controller.rb.velocity = new Vector2(0, 0);
    }
}

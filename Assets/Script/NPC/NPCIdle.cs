using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : MonoBehaviour, IState<NPCController>
{
    [SerializeField] private NPCController controller;
    public void OperateEnter(NPCController sender)
    {
        controller = sender;
        controller.animator.SetBool("Idle", true);
    }

    public void OperateExit(NPCController sender)
    {
        controller.animator.SetBool("Idle", false);
    }

    public void OperateUpdate(NPCController sender)
    {
        controller.rb.velocity = new Vector2(controller.moveDirection.x * 0, controller.rb.velocity.y);
    }
}

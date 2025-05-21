using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRun : MonoBehaviour, IState<NPCController>
{
    [SerializeField] private NPCController controller;
    public void OperateEnter(NPCController sender)
    {
        controller = sender;

        controller.animator.SetBool("Run", true);
    }

    public void OperateExit(NPCController sender)
    {
        controller.animator.SetBool("Run", false);
    }

    public void OperateUpdate(NPCController sender)
    {
        controller.rb.velocity = new Vector2(controller.moveDirection.x * controller.runSpeed, controller.rb.velocity.y);

    }

}

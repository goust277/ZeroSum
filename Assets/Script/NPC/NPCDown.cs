using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDown : MonoBehaviour, IState<NPCController>
{
    [SerializeField] private NPCController controller;
    public void OperateEnter(NPCController sender)
    {
        controller = sender;
        controller.animator.SetBool("Down2", true);
    }

    public void OperateExit(NPCController sender)
    {
        controller.animator.SetBool("Down2", false);
        controller.isDowned = false;
    }

    public void OperateUpdate(NPCController sender)
    {
        
    }

}

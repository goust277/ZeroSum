using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTeleport : MonoBehaviour, IState<NPCController>
{
    [SerializeField] private NPCController controller;
    public void OperateEnter(NPCController sender)
    {
        controller = sender;

        Vector3 tel = new Vector3(controller.player.transform.position.x, controller.player.transform.position.y + 0.1f, controller.player.transform.position.z);
        controller.transform.position = tel;
    }

    public void OperateExit(NPCController sender)
    {
        
    }

    public void OperateUpdate(NPCController sender)
    {
        
    }


}

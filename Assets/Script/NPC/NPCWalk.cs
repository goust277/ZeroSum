using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWalk : MonoBehaviour, IState<NPCController>
{
    [SerializeField] private NPCController controller;
    public void OperateEnter(NPCController sender)
    {
        controller = sender;
        controller.animator.SetBool("Walk", true);
    }

    public void OperateExit(NPCController sender)
    {
        controller.animator.SetBool("Walk", false);
    }

    public void OperateUpdate(NPCController sender)
    {
        controller.rb.velocity = new Vector2(controller.moveDirection.x * controller.walkSpeed, controller.rb.velocity.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

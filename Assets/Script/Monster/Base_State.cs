using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected StateMachine stateMachine;

    public BaseState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void Execute()
    {

    }

    public virtual void Exit()
    {

    }
}

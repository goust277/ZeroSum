using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RightArm : BaseState
{
    private WorldTree boss;

    public RightArm(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        boss.anim.SetBool("Right_atk", true);
        boss.Right_atk.enabled = true;
        boss.rightArmDamage.gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public override void Execute()
    {
        AnimatorStateInfo animInfo = boss.anim.GetCurrentAnimatorStateInfo(0);

        if (animInfo.IsName("Right_Atk") && animInfo.normalizedTime >= 0.75f)
        {
            stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        boss.anim.SetBool("Right_atk", false);
        boss.Right_atk.enabled = false;
        boss.rightArmDamage.gameObject.GetComponent<Collider2D>().enabled = true;
    }
}

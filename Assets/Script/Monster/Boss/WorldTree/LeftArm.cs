using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArm : BaseState
{
    private WorldTree boss;

    public LeftArm(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        boss.anim.SetBool("Left_atk", true);
        boss.Left_atk.enabled = true;
        boss.leftArmDamage.gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public override void Execute()
    {
        AnimatorStateInfo animInfo = boss.anim.GetCurrentAnimatorStateInfo(0);

        if (animInfo.IsName("Left_Atk") && animInfo.normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        boss.anim.SetBool("Left_atk", false);
        boss.Left_atk.enabled = false;
        boss.leftArmDamage.gameObject.GetComponent<Collider2D>().enabled = true;
    }
}

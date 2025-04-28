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
            // 발악 상태인지 확인
            if (boss.isDying && boss.finalBurst.Count > 0)
            {
                if (boss.burstIndex < boss.finalBurst.Count - 1)
                {
                    boss.burstIndex++;
                    stateMachine.ChangeState(boss.patternPause);
                }
                else
                {
                    stateMachine.ChangeState(boss.idleState); // 모든 발악 패턴 종료
                }
            }
            else
            {
                // 일반 전투 흐름
                if (boss.pattern2 != null)
                {
                    stateMachine.ChangeState(boss.patternPause);
                }
                else
                {
                    stateMachine.ChangeState(boss.idleState);
                }
            }
        }
    }

    public override void Exit()
    {
        boss.anim.SetBool("Right_atk", false);
        boss.Right_atk.enabled = false;
        boss.rightArmDamage.gameObject.GetComponent<Collider2D>().enabled = true;
    }
}

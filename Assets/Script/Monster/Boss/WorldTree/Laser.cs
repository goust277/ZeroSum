using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Laser : BaseState
{
    private WorldTree boss;
    private float duration = 0.4f;
    private float timer;

    private Vector3 targetEulerAngles = new Vector3(0, 0, 120);
    private Quaternion startRotation;
    private Quaternion endRotation;

    public Laser(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        timer = 0f;
        startRotation = boss.Eye_laser.transform.rotation;
        endRotation = Quaternion.Euler(targetEulerAngles);
        boss.Eye_laser.SetActive(true);
    }

    public override void Execute()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        boss.Eye_laser.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

        if (t >= 1.0f)
        {
            boss.Eye_laser.SetActive(false);
            boss.Eye_laser_col.SetActive(true);
        }

        if(timer >= 0.85f)
        {
            boss.Eye_laser_col.SetActive(false);
            stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        boss.Eye_laser.transform.rotation = startRotation;
        boss.Eye_laser.SetActive(false);
    }
}

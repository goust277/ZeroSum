using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleArm : BaseState
{
    private WorldTree boss;
    private float timer;
    private float duration = 3.0f;

    public bool boosted = false;

    private Transform player;
    private Transform armObject;
    private float moveSpeed = 1.0f;

    public MiddleArm(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
        this.armObject = boss.MiddleArm.transform;
        this.player = GameObject.FindWithTag("Player")?.transform;
    }

    public override void Enter()
    {
        timer = duration;
        boss.anim.SetBool("Middle_atk", true);
        if (player != null && armObject != null)
        {
            Vector3 newPosition = new Vector3(player.position.x, armObject.position.y, armObject.position.z);
            armObject.position = newPosition;
            armObject.gameObject.SetActive(true);
        }
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (player != null && armObject != null && timer > 0f)
        {
            Vector3 targetPos = new Vector3(player.position.x, armObject.position.y, armObject.position.z);
            armObject.position = Vector3.Lerp(armObject.position, targetPos, Time.deltaTime * moveSpeed);
        }

        if (timer <= 2f)
        {
            boss.laser.SetActive(true); // 레이저 활성화 시점
        }

        if (timer <= 0f)
        {
            boss.laser.SetActive(false);

            stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        boss.anim.SetBool("Middle_atk", false);
        if (armObject != null)
            armObject.gameObject.SetActive(false);
    }
}

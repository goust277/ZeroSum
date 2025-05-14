using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedDrop : BaseState
{
    private WorldTree boss;
    private float timer;
    private float duration = 3.0f;

    private Transform player;
    private Transform armObject;
    private float moveSpeed = 1.0f;
    private bool hasSpawned = false; 

    public SeedDrop(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
        this.player = GameObject.FindWithTag("Player")?.transform;
        this.armObject = boss.MiddleArm.transform;
    }

    public override void Enter()
    {
        timer = duration;
        hasSpawned = false;
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

        if (!hasSpawned && timer <= 2f)
        {
            hasSpawned = true;
            Quaternion spawnRotation = Quaternion.Euler(0f, 0f, -90f);
            Vector2 spawnPosition = armObject.position + Vector3.down * 1.5f;
            GameObject.Instantiate(boss.seed, spawnPosition, spawnRotation);
        }

        if (timer <= 0f)
        {
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

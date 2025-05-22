using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout_Chase : BaseState
{
    private Scout scout;
    private float timer = 1.0f;

    public Scout_Chase(StateMachine stateMachine, Scout monster) : base(stateMachine)
    {
        this.scout = monster;
    }

    public override void Enter()
    {
        Debug.Log("추적 상태");
        scout.PlayMoveSound(1.2f);
        scout.anim.SetBool("isWalk", true);
        
        if(!scout.seeMark)
        scout.mark.gameObject.SetActive(true);
        
        scout.moveSpeed = 2.5f;
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            scout.mark.gameObject.SetActive(false);
        }
        
        if(!scout.canMove)
        {
            return;
        }
        
        if (Mathf.Abs(scout.player.transform.position.x - scout.transform.position.x) >= 9f 
            || Mathf.Abs(scout.player.transform.position.y - scout.transform.position.y) >= 2f)
        {
            stateMachine.ChangeState(new Scout_Patrol(stateMachine, scout));
            return;
        }

        if ((scout.player.position.x - scout.transform.position.x) >= 0.2f)
        {
            scout.sprite.flipX = true;
            scout.detect.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if ((scout.player.position.x - scout.transform.position.x) <= 0.2f)
        {
            scout.sprite.flipX = false;
            scout.detect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (scout.attackRange > Mathf.Abs(scout.player.position.x - scout.transform.position.x))
        {
            if (scout.CanEnterAttackState())
            {
                stateMachine.ChangeState(new Scout_Ready(stateMachine, scout));
                return;
            }
        }
        //float offsetX = scout.sprite.flipX ? -5f : 5f;

        Vector3 targetPosition = new Vector3(scout.player.position.x /*+ offsetX*/, scout.transform.position.y, 0);
        scout.transform.position = Vector3.MoveTowards(scout.transform.position, targetPosition, scout.moveSpeed * Time.deltaTime);
    }
    public override void Exit()
    {
        scout.StopMoveSound();
        scout.anim.SetBool("isWalk", false);
        scout.moveSpeed = 2;
        timer = 1f;
    }
}

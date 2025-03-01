using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class S_Attack : BaseState
{
    private Spider s;
    private float timer = 0;

    public S_Attack(StateMachine stateMachine, Spider monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        int dir = 0;
        timer = 0;
        s.attack.gameObject.SetActive(true);
        s.anim.SetBool("isAttack", true);
        if (s.transform.position.x >= s.player.position.x)
        {
            s.sprite.flipX = true;
            s.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
            dir = -1;
        }

        else if(s.transform.position.x < s.player.position.x)
        {
            s.sprite.flipX = false;
            s.transform.rotation = Quaternion.Euler(0f, 0f, -45f);
            dir = 1;
        }
        s.rb.velocity = new Vector2(s.dashRange * dir, s.dashRange);
    }

    public override void Execute()
    {
        timer += Time.deltaTime;

        if(timer >= 0.4f)
        {
            stateMachine.ChangeState(new S_Chase(stateMachine, s));
            return;
        }
    }

    public override void Exit()
    {
        s.attack.gameObject.SetActive(false);
        s.canAttack = true;
        s.attackCooldown = 3f;
        s.anim.SetBool("isWalk", true);
        s.anim.SetBool("isAttack", false);
        s.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}

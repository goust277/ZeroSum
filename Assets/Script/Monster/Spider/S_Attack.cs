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
        s.PlayAttackSound();
        s.attack.gameObject.SetActive(true);
        s.anim.SetBool("isAttack", true);
        if (s.transform.position.x >= s.player.position.x)
        {
            s.sprite.flipX = true;
            dir = -1;
        }

        else if(s.transform.position.x < s.player.position.x)
        {
            s.sprite.flipX = false;
            dir = 1;
        }
        s.rb.velocity = new Vector2(s.dashRange * dir, s.dashRange);
    }

    public override void Execute()
    {
        timer += Time.deltaTime;

        if(timer >= 0.5f)
        {
            s.gameObject.SetActive(false);
        }
    }

    public override void Exit()
    {

    }
}

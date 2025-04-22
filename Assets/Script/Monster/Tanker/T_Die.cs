using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Die : BaseState
{
    Tanker tanker;
    float timer = 0;

    public T_Die(StateMachine stateMachine, Tanker monster) : base(stateMachine)
    {
        this.tanker = monster;
    }

    public override void Enter()
    {
        timer = 2f;
        tanker.isDie = true;

        if ((tanker.player.position.x - tanker.transform.position.x) >= 0.2f)
        {
            tanker.sprite.flipX = true;
        }

        if ((tanker.player.position.x - tanker.transform.position.x) <= 0.2f)
        {
            tanker.sprite.flipX = false;
        }
        //tanker.SpawnHitEffect();

        tanker.anim.SetTrigger("isDie");
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;
        AnimatorStateInfo stateInfo = tanker.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("T_die") && stateInfo.normalizedTime >= 0.98f)
        {
            tanker.gameObject.SetActive(false);
        }
        else if(timer <= 0)
        {
            tanker.gameObject.SetActive(false);
        }
    }
    public override void Exit()
    {

    }
}

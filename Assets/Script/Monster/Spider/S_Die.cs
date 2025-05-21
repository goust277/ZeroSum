using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Die : BaseState
{
    private Spider s;
    private float timer = 0.5f;
    public S_Die(StateMachine stateMachine, Spider monster) : base(stateMachine)
    {
        this.s = monster;
    }

    public override void Enter()
    {
        s.isDie = true;
        
        s.PlayDeathSound();
        s.anim.SetTrigger("isDie");

        if ((s.player.position.x - s.transform.position.x) >= 0.2f)
        {
            s.sprite.flipX = true;
        }

        if ((s.player.position.x - s.transform.position.x) <= 0.2f)
        {
            s.sprite.flipX = false;
        }
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            s.gameObject.SetActive(false);
        }
        AnimatorStateInfo stateInfo = s.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("S_die") && stateInfo.normalizedTime >= 0.98f)
        {
            s.gameObject.SetActive(false);
        }
    }
}

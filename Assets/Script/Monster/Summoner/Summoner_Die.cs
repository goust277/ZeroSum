using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_Die : BaseState
{
    private Summoner summoner;
    private float timer = 0.5f;
    public Summoner_Die(StateMachine stateMachine, Summoner monster) : base(stateMachine)
    {
        this.summoner = monster;
    }

    public override void Enter()
    {
        summoner.isDie = true;


        summoner.anim.SetTrigger("isDie");

        if ((summoner.player.position.x - summoner.transform.position.x) >= 0.2f)
        {
            summoner.sprite.flipX = true;
        }

        if ((summoner.player.position.x - summoner.transform.position.x) <= 0.2f)
        {
            summoner.sprite.flipX = false;
        }
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            summoner.gameObject.SetActive(false);
        }
        AnimatorStateInfo stateInfo = summoner.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Summoner_die") && stateInfo.normalizedTime >= 0.98f)
        {
            summoner.gameObject.SetActive(false);
        }
    }
}

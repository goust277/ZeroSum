using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTree_Die : BaseState
{
    private WorldTree boss;
    private float fadeDuration = 3.0f;
    private float timer;
    private SpriteRenderer[] renderers;

    public WorldTree_Die(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        timer = fadeDuration;
        renderers = boss.GetComponentsInChildren<SpriteRenderer>();
    }

    public override void Execute()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            foreach (var renderer in renderers)
            {
                if (renderer != null)
                {
                    Color c = renderer.color;
                    c.a = alpha;
                    renderer.color = c;
                }
            }
        }
        else
        {
            boss.gameObject.SetActive(false);
        }
    }

    public override void Exit() { }
}

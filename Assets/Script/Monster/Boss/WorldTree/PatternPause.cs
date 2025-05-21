//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PatternPause : BaseState
//{
//    private WorldTree boss;
//    private float pauseTime = 0.25f;
//    private float timer;

//    public PatternPause(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
//    {
//        this.boss = boss;
//    }

//    public override void Enter()
//    {
//        timer = pauseTime;
//    }

//    public override void Execute()
//    {
//        timer -= Time.deltaTime;

//        if (timer <= 0f)
//        {
//            if (boss.isDying && boss.finalBurst.Count > 0)
//            {
//                var next = boss.finalBurst[boss.burstIndex];
//                stateMachine.ChangeState(next);
//            }
//            else
//            {
//                if (boss.pattern2 != null)
//                {
//                    stateMachine.ChangeState(boss.pattern2);
//                }
//                else
//                {
//                    stateMachine.ChangeState(boss.idleState);
//                }

//                boss.pattern1 = null;
//                boss.pattern2 = null;
//            }
//        }
//    }

//    public override void Exit() { }
//}

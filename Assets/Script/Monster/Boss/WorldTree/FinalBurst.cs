//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FinalBurst : BaseState
//{
//    private WorldTree boss;

//    public FinalBurst(StateMachine stateMachine, WorldTree boss) : base(stateMachine)
//    {
//        this.boss = boss;
//    }

//    public override void Enter()
//    {
//        boss.PrepareFinalBurst();
//        stateMachine.ChangeState(boss.finalBurst[boss.burstIndex]);
//    }

//    public override void Execute()
//    {

//    }

//    public override void Exit()
//    {

//    }
//}

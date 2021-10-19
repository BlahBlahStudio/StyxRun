using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : FSM<MonsterScript>
{
    MonsterScript obj;
    public WalkState(MonsterScript obj)
    {
        this.obj = obj;
    }
    public override void Begin()
    {
        Debug.Log("몬스터 움직임 시작");
        obj.motionAnimation.SetBool("Walk", true);
    }

    public override void Exit()
    {
        Debug.Log("몬스터 움직임 종료");
        obj.motionAnimation.SetBool("Walk", false);
        obj.MoveEndAction();
    }
    public override void Run()
    {
        if (obj.isLeftMoveInput)
        {
            obj.MoveLeftAction();
        }
        else if (obj.isRightMoveInput)
        {
            obj.MoveRightAction();
        }
        else if (obj.isAttack)
        {
            obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Attack]);
        }
        else
        {
            obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Idle]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FSM<MonsterScript>
{
    MonsterScript obj;
    public IdleState(MonsterScript obj)
    {
        this.obj = obj;
    }
    public override void Begin()
    {
        //Debug.Log("몬스터 생성 숨쉬는중 ");
        obj.motionAnimation.SetBool("Idle", true);
    }

    public override void Exit()
    {
        //Debug.Log("몬스터 숨쉬는중 끝");
        obj.motionAnimation.SetBool("Idle", false);

    }
    public override void Run()
    {
        // Debug.Log("몬스터 숨쉬는중");
        if (obj.isHit)
        {
            if (Vector2.Distance(GameManager.Instance.player.transform.position, obj.transform.position) > obj.attackSize*3)
            {
                obj.isHit = false;
            }
        }
        if (obj.isLeftMoveInput)
        {
            obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Walk]);
        }
        if(obj.isRightMoveInput)
        {
            obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Walk]);
        }
        if (obj.isAttack)
        {
            obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Attack]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : FSM<MonsterScript>
{
    MonsterScript obj;
    public HitState(MonsterScript obj)
    {
        this.obj = obj;
    }
    public override void Begin()
    {
        //Debug.Log("몬스터 공격 당함 시작");
        obj.isHit = true;
        obj.motionAnimation.SetTrigger("Hit");
        obj.StartCoroutine(CheckAnimationState());
    }

    public override void Exit()
    {
       // Debug.Log("몬스터 당함 종료");
        if (obj.hp <= 0)
        {
           // obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Die]);
        }
    }
    public override void Run()
    {
        obj.UnitHitEvent();
    }
    IEnumerator CheckAnimationState()
    {
        while (!obj.motionAnimation.GetCurrentAnimatorStateInfo(0)
        .IsName("Hit"))
        {
            //전환 중일 때 실행되는 부분
            yield return null;
        }

        while (obj.motionAnimation.GetCurrentAnimatorStateInfo(0)
        .normalizedTime < 1)
        {
            //애니메이션 재생 중 실행되는 부분
            yield return null;
        }
        obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Idle]);
        //애니메이션 완료 후 실행되는 부분

    }

}

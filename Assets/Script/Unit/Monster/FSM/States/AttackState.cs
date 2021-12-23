using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSM<MonsterScript>
{
    MonsterScript obj;
    Coroutine co;
    public AttackState(MonsterScript obj)
    {
        this.obj = obj;
       
    }
    public override void Begin()
    {
        //Debug.Log("몬스터 공격 수행");
        obj.motionAnimation.SetBool("Attack", true);
        co=obj.StartCoroutine(CheckAnimationState());
    }

    public override void Exit()
    {
        //Debug.Log("몬스터 공격 종료");
        obj.StopCoroutine(co);
        obj.motionAnimation.SetBool("Attack", false);
    }
    public override void Run()
    {
        //Debug.Log("몬스터 공격 중");
    }
    IEnumerator CheckAnimationState()
    {
        while (!obj.motionAnimation.GetCurrentAnimatorStateInfo(0)
        .IsName("Attack"))
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
        //Debug.Log("공격하다 숨쉬기로 이동");
        obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Idle]);
        //애니메이션 완료 후 실행되는 부분

    }
}

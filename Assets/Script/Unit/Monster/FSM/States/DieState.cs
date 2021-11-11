using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : FSM<MonsterScript>
{
    MonsterScript obj;
    public DieState(MonsterScript obj)
    {
        this.obj = obj;
    }
    public override void Begin()
    {
        Debug.Log("몬스터 사망 시작");
        obj.motionAnimation.SetTrigger("Die");
        obj.StartCoroutine(CheckAnimationState());
    }

    public override void Exit()
    {
        Debug.Log("몬스터 사망 종료");
        Object.Destroy(obj.gameObject);
    }
    public override void Run()
    {

    }
    IEnumerator CheckAnimationState()
    {
        while (!obj.motionAnimation.GetCurrentAnimatorStateInfo(0)
        .IsName("Die"))
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

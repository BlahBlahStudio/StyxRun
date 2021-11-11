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
        //Debug.Log("���� ���� ���� ����");
        obj.isHit = true;
        obj.motionAnimation.SetTrigger("Hit");
        obj.StartCoroutine(CheckAnimationState());
    }

    public override void Exit()
    {
       // Debug.Log("���� ���� ����");
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
            //��ȯ ���� �� ����Ǵ� �κ�
            yield return null;
        }

        while (obj.motionAnimation.GetCurrentAnimatorStateInfo(0)
        .normalizedTime < 1)
        {
            //�ִϸ��̼� ��� �� ����Ǵ� �κ�
            yield return null;
        }
        obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Idle]);
        //�ִϸ��̼� �Ϸ� �� ����Ǵ� �κ�

    }

}

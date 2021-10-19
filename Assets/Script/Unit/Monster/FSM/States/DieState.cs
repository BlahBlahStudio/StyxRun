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
        Debug.Log("���� ��� ����");
        obj.motionAnimation.SetTrigger("Die");
        obj.StartCoroutine(CheckAnimationState());
    }

    public override void Exit()
    {
        Debug.Log("���� ��� ����");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSM<MonsterScript>
{
    MonsterScript obj;
    public AttackState(MonsterScript obj)
    {
        this.obj = obj;
       
    }
    public override void Begin()
    {
        Debug.Log("���� ���� ����");
        obj.motionAnimation.SetBool("Attack", true);
        obj.StartCoroutine(CheckAnimationState());
    }

    public override void Exit()
    {
        Debug.Log("���� ���� ����");
        obj.motionAnimation.SetBool("Attack", false);
    }
    public override void Run()
    {
        //Debug.Log("���� ���� ��");
    }
    IEnumerator CheckAnimationState()
    {
        while (!obj.motionAnimation.GetCurrentAnimatorStateInfo(0)
        .IsName("Attack"))
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
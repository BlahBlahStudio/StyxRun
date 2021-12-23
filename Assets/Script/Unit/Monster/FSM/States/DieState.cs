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
            obj.motionAnimation.SetTrigger("Die");
            yield return null;
        }

        while (obj.motionAnimation.GetCurrentAnimatorStateInfo(0)
        .normalizedTime < 1)
        {
            //�ִϸ��̼� ��� �� ����Ǵ� �κ�
            if (!obj.motionAnimation.GetCurrentAnimatorStateInfo(0)
        .IsName("Die"))
            {
                //��ȯ ���� �� ����Ǵ� �κ�
                obj.motionAnimation.SetTrigger("Die");
            }
                yield return null;
        }
        Debug.Log("���");
        Object.Destroy(obj.gameObject);
        //�ִϸ��̼� �Ϸ� �� ����Ǵ� �κ�

    }
}

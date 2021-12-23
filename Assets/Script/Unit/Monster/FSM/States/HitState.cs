using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : FSM<MonsterScript>
{
    MonsterScript obj;
    Coroutine co;
    public HitState(MonsterScript obj)
    {
        this.obj = obj;
    }
    public override void Begin()
    {
        //Debug.Log("���� ���� ���� ����");
        obj.isHit = true;
        GameManager.Instance.OnTargetUI(obj);
        if (obj.hp <= 0)
        {
            obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Die]);
            return;
        }
        obj.motionAnimation.SetTrigger("Hit");
        co = obj.StartCoroutine(CheckAnimationState());
    }

    public override void Exit()
    {
        // Debug.Log("���� ���� ����");
        obj.StopCoroutine(co);
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
            if (obj.hp <= 0)
            {
                obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Die]);
                yield break;
            }
            yield return null;
        }
            if (obj.hp <= 0)
            {
                obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Die]);
                yield break;
            }
        Debug.Log("�´ٰ� ������� �̵�");
        obj.fsmMachine.Change(obj.states[(int)MonsterScript.MyState.Idle]);
        //�ִϸ��̼� �Ϸ� �� ����Ǵ� �κ�

    }

}

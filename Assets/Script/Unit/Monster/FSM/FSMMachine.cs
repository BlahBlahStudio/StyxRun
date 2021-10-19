using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMMachine<T>
{
    // Start is called before the first frame update
    private T own;
    public FSM<T> nowState = null;
    public FSM<T> preState = null;
    public void Run()
    {
        if (nowState != null)
        {
            nowState.Run();
        }
    }
    public void Begin()
    {
        if (nowState != null)
        {
            nowState.Begin();
        }
    }
    public void Exit()
    {
        nowState.Exit();
        preState = nowState;
        nowState = null;
    }
    public void Change(FSM<T> state)
    {
        //변화희망 상태가 기존 상태이면 무시
        if (nowState == state)
        {
            //return;
        }
        //기존 상태 종료
        if (nowState != null)
        {
            nowState.Exit();
        }
        //이전 상태에 기존상태 할당
        preState = nowState;
        nowState = state;
        nowState.Begin();
    }
    public void SetState(FSM<T> state)
    {
        //own = owner;
        nowState = state;
        Debug.Log("state 설정");
    }
}

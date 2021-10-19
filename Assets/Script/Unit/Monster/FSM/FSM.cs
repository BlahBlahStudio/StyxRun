using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class FSM<T>
{
    public T owner;
    abstract public void Begin();
    abstract public void Run();
    abstract public void Exit();
}
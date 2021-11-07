using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackScript<T> : MonoBehaviour
{
    public T own;
    public void Start()
    {
        own = GetComponentInParent<T>();
    }
    public abstract void EffectOn();
    public abstract void AttackEvent();
}

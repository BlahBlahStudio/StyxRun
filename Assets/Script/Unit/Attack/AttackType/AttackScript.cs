using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackScript
{
    public UnitScript own;
    public AttackScript()
    {

    }
    public abstract void EffectOn();
    public abstract void AttackEvent();
}

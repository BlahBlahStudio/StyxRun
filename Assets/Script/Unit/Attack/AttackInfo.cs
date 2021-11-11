using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfo
{
    GameObject own;
    public float damage;
    public AttackInfo(GameObject own,float damage)
    {
        this.own = own;
        this.damage = damage;
    }
}

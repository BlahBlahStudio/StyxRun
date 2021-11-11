using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackScript : AttackScript
{
    public RangedAttackScript(UnitScript own)
    {
        this.own = own;
    }
    public override void AttackEvent()
    {
        Vector3 offset =own.throwPoint;
        var obj = Object.Instantiate(GameManager.Instance.throwObjectList[own.GetEquipWeapon().throwObject], own.transform.position + offset, Quaternion.identity);
        ThrowObjectScript throwScript=obj.GetComponent<ThrowObjectScript>();
        throwScript.dir = own.dir;
        throwScript.speed = own.GetEquipWeapon().throwSpeed;
        throwScript.target = new Target(GameManager.Instance.player.transform.position);
    }

    public override void EffectOn() 
    {
        
    }

}

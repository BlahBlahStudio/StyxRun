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
        var obj = Object.Instantiate(own.GetEquipWeapon().throwObject,own.muzzle.transform.position, Quaternion.identity);
        ThrowObjectScript throwScript=obj.GetComponent<ThrowObjectScript>();
        throwScript.dir = own.dir;
        throwScript.speed = own.GetEquipWeapon().throwSpeed;
        throwScript.attackInfo = new AttackInfo(own.gameObject, own.damage);
        throwScript.owner=own;
        if (own is PlayerScript)
        {
            var degree = ((PlayerScript)own).hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
            Vector3 pos = ((PlayerScript)own).weaponPivot.transform.position;
            pos.x += Mathf.Cos(degree) * 3;
            pos.y += Mathf.Sin(degree) * 3;
            throwScript.target = new Target(pos);
            
        }
        else
        {
            throwScript.target = new Target(((PlayerScript)GameManager.Instance.player).hand.transform.position);
        }
    }

    public override void EffectOn() 
    {
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackScript : AttackScript<RangedMonsterScript>
{
    public override void AttackEvent()
    {
        Vector3 offset = own.throwPoint;
        if (own.dir == MyDir.left)
        {
            offset.x *= -1;
        }
        var obj = Instantiate(own.throwObj, own.transform.position + offset, Quaternion.identity);
        ThrowObjectScript throwScript=obj.GetComponent<ThrowObjectScript>();
        throwScript.dir = own.dir;
        throwScript.speed = own.throwSpeed;
        throwScript.target = new Target(GameManager.Instance.player.transform.position);
    }

    public override void EffectOn()
    {
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackScript : AttackScript
{
    public GameObject attackEffect;
    public float rad;
    public SwordAttackScript()
    {

    }
        public SwordAttackScript(UnitScript own)
    {
        this.own = own;
       
        
        //this.rad = rad;
    }
    public override void EffectOn()
    {
        this.attackEffect = own.equipWeapon.attackEffect;
        this.rad = own.equipWeapon.size;
        if (own is PlayerScript)
        {
            Vector3 pos = ((PlayerScript)own).hand.transform.position;
            var degree = ((PlayerScript)own).hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
            pos.x += Mathf.Cos(degree) * rad;
            pos.y += Mathf.Sin(degree) * rad;

            var obj = Object.Instantiate(attackEffect, pos, Quaternion.Euler(((PlayerScript)own).hand.transform.rotation.eulerAngles));
            if (((PlayerScript)own).dir == MyDir.left)
            {
                var tmp = obj.transform.localScale;
                tmp.y *= -1;
                obj.transform.localScale = tmp;
            }
            else
            {

            }
        }
    }
    public override void AttackEvent()
    {
        if (own is PlayerScript)
        {
            Vector3 pos = ((PlayerScript)own).hand.transform.position;
            var degree = ((PlayerScript)own).hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
            pos.x += Mathf.Cos(degree) * ((PlayerScript)own).attackPos;
            pos.y += Mathf.Sin(degree) * ((PlayerScript)own).attackPos;
            var units = Physics2D.OverlapCircleAll(pos, ((PlayerScript)own).attackSize, ((PlayerScript)own).attackTargetLayer);
            foreach (var unit in units)
            {
                if (!unit.isTrigger)
                {
                    continue;
                }
                    var attackInfo = new AttackInfo(own.gameObject, own.damage);
                    unit.GetComponent<UnitScript>().UnitHit(attackInfo);

            }
        }
    }
}

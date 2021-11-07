using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackScript : AttackScript<PlayerScript>
{
    public GameObject attackEffect;
    public float rad;

    public override void EffectOn()
    {
        Vector3 pos = own.hand.transform.position;
        var degree = own.hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
        pos.x += Mathf.Cos(degree) * rad;
        pos.y += Mathf.Sin(degree) * rad;
        
        var obj=Instantiate(attackEffect, pos, Quaternion.Euler(own.hand.transform.rotation.eulerAngles));
        if (own.dir == MyDir.left)
        {
            var tmp = obj.transform.localScale;
            tmp.y *= -1;
            obj.transform.localScale = tmp;
        }
        else
        {

        }
    }
    public override void AttackEvent()
    {
        Vector3 pos = own.hand.transform.position;
        var degree = own.hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
        pos.x += Mathf.Cos(degree) * own.attackPos;
        pos.y += Mathf.Sin(degree) * own.attackPos;
        var units= Physics2D.OverlapCircleAll(pos, own.attackSize,own.attackTargetLayer);
        foreach(var unit in units)
        {
            unit.GetComponent<UnitScript>().UnitHit(own.damage);
        }
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying) { 
        Vector3 pos = own.hand.transform.position;
        var degree = own.hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
        pos.x += Mathf.Cos(degree) * own.attackPos;
        pos.y += Mathf.Sin(degree) * own.attackPos;
        Gizmos.color = new Color(0, 0, 1, 0.3f);
        Gizmos.DrawSphere(pos, own.attackSize);
    }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackScript : MonoBehaviour
{
    public PlayerScript player;
    public GameObject attackEffect;
    public float rad;
    public void EffectOn()
    {
        Vector3 pos = player.hand.transform.position;
        var degree = player.hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
        pos.x += Mathf.Cos(degree) * rad;
        pos.y += Mathf.Sin(degree) * rad;
        
        var obj=Instantiate(attackEffect, pos, Quaternion.Euler(player.hand.transform.rotation.eulerAngles));
        if (player.dir == MyDir.left)
        {
            var tmp = obj.transform.localScale;
            tmp.y *= -1;
            obj.transform.localScale = tmp;
        }
        else
        {

        }

    }
    public void Attack()
    {
        Vector3 pos = player.hand.transform.position;
        var degree = player.hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
        pos.x += Mathf.Cos(degree) * player.attackPos;
        pos.y += Mathf.Sin(degree) * player.attackPos;
        var units= Physics2D.OverlapCircleAll(pos, player.attackSize,player.attackTargetLayer);
        foreach(var unit in units)
        {
            unit.GetComponent<UnitScript>().UnitHit();
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 pos = player.hand.transform.position;
        var degree = player.hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
        pos.x += Mathf.Cos(degree) * player.attackPos;
        pos.y += Mathf.Sin(degree) * player.attackPos;
        Gizmos.color =new Color(0, 0, 1,0.3f);
        Gizmos.DrawSphere(pos, player.attackSize);

    }
}

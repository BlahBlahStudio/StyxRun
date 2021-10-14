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
    private void OnDrawGizmos()
    {

    }
}

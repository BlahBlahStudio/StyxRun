using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public UnitScript own;
    AttackScript order;
    public void EffectOn()
    {
        own.equipWeapon.behaviour.EffectOn();
    }
    public void AttackEvent()
    {
        own.equipWeapon.behaviour.AttackEvent();
    }
    public void SetOrder(AttackScript order)
    {
        Debug.Log("order");
        this.order = order;
    }
    public void SetOwner(UnitScript unit)
    {
        own = unit;
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (own.gameObject == GameManager.Instance.player.gameObject)
            {
                var player = (PlayerScript)own;
                Vector3 pos = player.hand.transform.position;
                var degree = player.hand.transform.rotation.eulerAngles.z * (Mathf.PI / 180);
                pos.x += Mathf.Cos(degree) * player.attackPos;
                pos.y += Mathf.Sin(degree) * player.attackPos;
                Gizmos.color = new Color(0, 0, 1, 0.3f);
                Gizmos.DrawSphere(pos, player.attackSize);
            }
        } 
    }
}

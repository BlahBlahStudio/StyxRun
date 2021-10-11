using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackScript : MonoBehaviour
{
    public PlayerScript player;
    public GameObject attackEffect;
    public void EffectOn()
    {
        Vector3 pos= player.hand.transform.position;
        Debug.Log($"Sin: {Mathf.Sin(player.hand.transform.rotation.z*(180/Mathf.PI))} Cos:{Mathf.Cos(player.hand.transform.rotation.z * (180 / Mathf.PI))}");
        Instantiate(attackEffect, pos, Quaternion.Euler(player.hand.transform.rotation.eulerAngles));
    }
}

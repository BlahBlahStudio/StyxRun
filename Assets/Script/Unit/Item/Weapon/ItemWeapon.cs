using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : Item
{
    public enum ItemDetailType
    {
        Sword,
        Arrow,
        Gun
    }
    public int index;
    public AttackScript behaviour;
    public ItemDetailType detailType;
    public float damage;
    public GameObject attackEffect;
    public int throwObject;
    public int throwSpeed;
    public ItemWeapon(int index,ItemType type,ItemDetailType detailType ,string name,Sprite icon,AttackScript behaviour,float damage)
    {
        this.index = index;
        this.detailType = detailType;
        this.damage = damage;
        this.type = type;
        this.name = name;
        this.icon = icon;
        this.behaviour = behaviour;
    }
    public ItemWeapon(int index, ItemType type, ItemDetailType detailType, string name, Sprite icon, AttackScript behaviour,int throwObejct,int throwSpeed,float damage)
    {
        this.index = index;
        this.detailType = detailType;
        this.damage = damage;
        this.type = type;
        this.name = name;
        this.icon = icon;
        this.behaviour = behaviour;
    }
}

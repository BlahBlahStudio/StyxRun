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

    public AttackScript behaviour;
    public ItemDetailType detailType;
    public float damage;
    public GameObject attackEffect;
    public GameObject throwObject;
    public float throwSpeed;
    public float size;
    public string image;
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
    public ItemWeapon(int index,UnitScript owner)
    {
        this.name = DataManager.GetData(DBList.Item, index, "Name");
        this.damage = float.Parse(DataManager.GetData(DBList.Item, index, "Damage"));
        //index로 공격 내용 조절////////////////
        this.behaviour = DataManager.GetBehaviourList(int.Parse(DataManager.GetData(DBList.Item, index, "Behaviour")), owner);
        this.type = ItemType.Weapon;
        this.detailType = (ItemDetailType)(int.Parse(DataManager.GetData(DBList.Item, index, "TypeDetail")));
        this.icon = null;//DataManager.GetData(DBList.Item, index, "Icon");
        this.size=float.Parse(DataManager.GetData(DBList.Item, index, "Size"));
        this.attackEffect= GameManager.Instance.effectList[int.Parse(DataManager.GetData(DBList.Item, index, "AttackEffect"))];
        int throwObjIndex = int.Parse(DataManager.GetData(DBList.Item, index, "ThrowObject"));
        if (throwObjIndex >= 0) {
            this.throwObject = GameManager.Instance.throwObjectList[throwObjIndex];
        }
        this.throwSpeed = float.Parse(DataManager.GetData(DBList.Item, index, "ThrowSpeed"));
        this.image= DataManager.GetData(DBList.Item, index, "Image");
        this.nickName= DataManager.GetData(DBList.Item, index, "NickName");
        ////////////////////////////////////////
    }
}

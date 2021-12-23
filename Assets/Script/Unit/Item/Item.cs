using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Food,
    Etc
}
public abstract class Item
{
    public int index;
    public string name;
    public string nickName;
    public Sprite icon;
    public ItemType type;
}

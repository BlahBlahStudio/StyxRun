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
    public string name;
    public Sprite icon;
    public ItemType type;
}

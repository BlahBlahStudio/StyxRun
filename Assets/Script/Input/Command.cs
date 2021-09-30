using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Command
{
    public delegate void Msg();
    public Msg msg, unMsg;
    private string setKey;
    public GameObject owner;
    public Command(string txt)
    {
        msg = Init;
        unMsg = unInit;
        setKey = txt;
        owner = null;
    }
    public Command(string txt,Msg msg,Msg unMsg,GameObject owner)
    {
        this.msg = msg;
        this.unMsg = unMsg;
        setKey = txt;
        this.owner = owner;
    }
    public void Execute()
    {
        msg();
        Debug.Log(owner);
        if (owner != null)
            Debug.Log(setKey + " key is Activate, owner :" + owner.name);
    }
    public void unExecute()
    {
        unMsg();
        if (owner != null)
            Debug.Log(setKey + " key is InActive, owner :" + owner.name);
    }
    public void Init()
    {
        Debug.Log("Init Setting : " + setKey);
    }
    public void unInit()
    {
        Debug.Log("unInit Setting : " + setKey);
    }
}

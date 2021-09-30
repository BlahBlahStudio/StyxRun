using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    protected Rigidbody2D rigid;
    public float speed;
    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        
    }
    public void SetMoveKeys()
    {
        SetKey("A", MoveLeft,MoveLeftCancel);
        SetKey("D", MoveRight, MoveRightCancel);
    }
    public void SetKey(string key,Command.Msg msg,Command.Msg unMsg)
    {
        Command c = new Command(key,msg,unMsg,gameObject);
        InputManager.SetKey(key, c);
    }
    protected virtual void MoveUp()
    {
        Debug.Log(gameObject.name+" Up");
    }
    protected virtual void MoveUpCancel()
    {
        Debug.Log(gameObject.name + " Up");
    }
    protected virtual void MoveDown()
    {
        Debug.Log(gameObject.name + " Down");
    }
    protected virtual void MoveLeft()
    {
        Debug.Log(gameObject.name + " Left");
    }
    protected virtual void MoveLeftCancel()
    {
        Debug.Log(gameObject.name + " Left InActive");
    }
    protected virtual void MoveRight() 
    {
        Debug.Log(gameObject.name + " Right");
    }
    protected virtual void MoveRightCancel()
    {
        Debug.Log(gameObject.name + " Right InActive");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : UnitScript
{
    bool isMoveInput;
    // Start is called before the first frame update
    void Start()
    {
        SetMoveKeys();
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    private void FixedUpdate()
    {
        if (isMoveInput)
        {
            rigid.velocity = new Vector2(Input.GetAxis("Hor") * speed, rigid.velocity.y);
        }
    }
    protected override void MoveUp()
    {
        
    }
    protected override void MoveDown()
    {
      
    }
    protected override void MoveLeft()
    {
        isMoveInput = true;
    }
    protected override void MoveRight()
    {
        isMoveInput = true;
    }
    protected override void MoveLeftCancel()
    {
        isMoveInput = false;
        rigid.velocity = new Vector2(rigid.velocity.normalized.x, rigid.velocity.y);
    }
    protected override void MoveRightCancel()
    {
        isMoveInput = false;
        rigid.velocity = new Vector2(rigid.velocity.normalized.x, rigid.velocity.y);
    }
}

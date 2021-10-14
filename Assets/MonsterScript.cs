using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : UnitScript
{

    // Start is called before the first frame update
    void Start()
    {
        SetKey("C", MoveLeft, MoveLeftCancel);
        SetKey("V", MoveRight, MoveRightCancel);
        SetKey("B", Attack, AttackCancel);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }
    protected override void MoveLeft()
    {
        if (!UnitsOnLeftWall())
        {
            rigid.velocity = new Vector2(-Speed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }
    protected override void Attack()
    {
        base.Attack();
    }
    protected override void AttackCancel()
    {
        base.AttackCancel();
    }
    protected override void MoveLeftCancel()
    {
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
    }
    protected override void MoveRight()
    {
        if (!UnitsOnRightWall())
        {
            rigid.velocity = new Vector2(Speed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }
    protected override void MoveRightCancel()
    {
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
    }
    protected override void OnFloorEvent()
    {
        if (UnitIsOnFloor())
        {
            footColider.enabled = true;
            SetJumpPower(walkingJumpPower);
            Speed = GetSpeed("Walk");

        }
        else
        {
            SetJumpPower(jumpingJumpPower);
            Speed = GetSpeed("Jump");
        }
    }
}

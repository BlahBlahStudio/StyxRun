using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : UnitScript
{
    bool isLeftMoveInput;
    bool isRightMoveInput;
    bool isJumpInput;
    int isWall; // 1: 왼벽 2:오른벽 
    int jumpCnt;
    float sideWallGravity;

    // Start is called before the first frame update
    void Start()
    {
        SetMoveKeys();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWall == 0)
        {
            //어떠한 벽에도 붙지 않았을때
            sideWallGravity = 1;
        }
        else
        {
            if (sideWallGravity > 0.5f)
            {
                sideWallGravity -= sideWallGravityPer * Time.deltaTime;
            }
           
        }
    }


    private void FixedUpdate()
    {
        OnFloorEvent();
        if (isLeftMoveInput)
        {
            MovingLeft();
        }
        if (isRightMoveInput)
        {
            MovingRight();
        }
    }

    public void MovingLeft()
    {
        SetMotionDir(false);
        if (!UnitsOnLeftWall())
        {
            //왼쪽 이동
            isWall = 0;
            isLeftMoveInput = false;
            rigid.velocity = new Vector2(-Speed, rigid.velocity.y);
        }
        else
        {
            //벽에 붙었을때 왼쪽 이동
            if (!UnitIsOnFloor())
            {
                isWall = 1;
            }
            rigid.velocity = new Vector2(0, rigid.velocity.y < 0 ? rigid.velocity.y * sideWallGravity : rigid.velocity.y);
            jumpCnt = maxJump-1;
        }
        
    }
    public void MovingRight()
    {
        SetMotionDir(true);
        if (!UnitsOnRightWall())
        {
            isWall = 0;
            isRightMoveInput = false;
            rigid.velocity = new Vector2(Speed, rigid.velocity.y);
        }
        else
        {
            if (!UnitIsOnFloor())
            {
                isWall = 2;
            }

            rigid.velocity = new Vector2(0, rigid.velocity.y < 0 ? rigid.velocity.y * sideWallGravity : rigid.velocity.y);
            jumpCnt = maxJump - 1;
        }
    }
    public void Jump()
    {
            jumpCnt++;
            isJumpInput = true;
            footColider.enabled = false;
            rigid.velocity = new Vector2(rigid.velocity.x, GetJumpPower());
    }
    protected override void MoveUp()
    {
        if (!isJumpInput)
        {
            if (jumpCnt < maxJump)
            {
                Jump();
            }
        }
    }
    protected override void MoveUpCancel()
    {
        isJumpInput = false;
    }
    protected override void MoveDown()
    {

    }
    protected override void MoveDownCancel()
    {

    }
    protected override void MoveLeft()
    {
        isLeftMoveInput = true;
    }
    protected override void MoveRight()
    {
        isRightMoveInput = true;
    }
    protected override void MoveLeftCancel()
    {
        isLeftMoveInput = false;
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
    }
    protected override void MoveRightCancel()
    {
        isRightMoveInput = false;
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
    }
    protected override void OnFloorEvent()
    {
        if (UnitIsOnFloor())
        {
            footColider.enabled = true;
            jumpCnt = 0;
            isWall = 0;
            SetJumpPower(walkingJumpPower);
            Speed = GetSpeed("Walk");

        }
        else
        {
            //if (!UnitsOnLeftWall() && !UnitsOnRightWall())
            //{
            //    footColider.enabled = false;
            //}
            //else
            //{
            //    footColider.enabled = true;
            //}
            SetJumpPower(jumpingJumpPower);
            Speed = GetSpeed("Jump");
        }
    }
}

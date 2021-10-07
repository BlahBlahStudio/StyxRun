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
    bool isOnFloor;
    float sideWallGravity;

    [Header("공격 관련")]
    public GameObject hand;
    public Animator attackAnimation;

    [Header("모션 이동 관련")]
    public Animator motionAnimation;

    // Start is called before the first frame update
    void Start()
    {
        SetMoveKeys();
    }

    // Update is called once per frame
    void Update()
    {
     
        SetAngle(hand, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        SetSideWallGravity(isWall);
        OnFloorEvent();
    }

    private void FixedUpdate()
    {
  
        MovingLeft(isLeftMoveInput);
        MovingRight(isRightMoveInput);
    }
    #region 이동
    private void SetSideWallGravity(int isWall)
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
    public void MovingLeft(bool isLefting)
    {
        if (!isLefting)
        {
            return;
        }
        if (!UnitsOnLeftWall())
        {
            //왼쪽 이동
            motionAnimation.SetBool("Moving",true);
            isWall = 0;
            isLeftMoveInput = false;
            rigid.velocity = new Vector2(-Speed, rigid.velocity.y);
        }
        else
        {
            motionAnimation.SetBool("Moving", false);
            //벽에 붙었을때 왼쪽 이동
            if (!isOnFloor)
            {
                isWall = 1;
            }
            rigid.velocity = new Vector2(0, rigid.velocity.y < 0 ? rigid.velocity.y * sideWallGravity : rigid.velocity.y);
            jumpCnt = maxJump-1;
        }
        
    }
    public void MovingRight(bool isRighting)
    {
        if (!isRighting)
        {
            return;
        }
        if (!UnitsOnRightWall())
        {
            motionAnimation.SetBool("Moving", true);
            isWall = 0;
            isRightMoveInput = false;
            rigid.velocity = new Vector2(Speed, rigid.velocity.y);
        }
        else
        {
            motionAnimation.SetBool("Moving", false);
            if (!isOnFloor)
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
        motionAnimation.SetBool("Moving", false);
        isLeftMoveInput = false;
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
    }
    protected override void MoveRightCancel()
    {
        motionAnimation.SetBool("Moving", false);
        isRightMoveInput = false;
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
    }
    #endregion

    protected override void Attack()
    {
        attackAnimation.SetBool("Attack", true);
    }
    protected override void AttackCancel()
    {
        attackAnimation.SetBool("Attack", false);
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
            isOnFloor = true;
            
        }
        else
        {
            SetJumpPower(jumpingJumpPower);
            Speed = GetSpeed("Jump");
            isOnFloor = false;
        }
    }
    public void SetAngle(GameObject obj,Vector3 point)
    {
        float z = 180 - Mathf.Atan2(point.y - obj.transform.position.y, obj.transform.position.x - point.x) * 180 / Mathf.PI;
       if((z <= 90 && z >= 0) || (z > 270 && z < 360))
        {
            SetMotionDir(true);
            obj.transform.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            SetMotionDir(false);
            obj.transform.localScale=new Vector3(-1, -1, 0);
        }
        obj.transform.rotation=Quaternion.Euler(new Vector3(0, 0, z ));
        //Debug.Log(180-Mathf.Atan2(point.y - obj.transform.position.y, obj.transform.position.x - point.x)*180/Mathf.PI) ;
    }
}

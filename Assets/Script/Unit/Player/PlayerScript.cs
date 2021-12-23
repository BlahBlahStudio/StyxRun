using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : UnitScript
{
    bool isJumpInput;
    int isWall; // 1: 왼벽 2:오른벽 
    int jumpCnt;
    bool isOnFloor;
    float sideWallGravity;

    [Header("공격 관련")]
    public GameObject hand;
    public Animator attackAnimation;
    public Animator headAnimation;
    public float attackSize;
    public float attackPos;
    public LayerMask attackTargetLayer;
    public GameObject swordPivot;

    public Vector2 startVec;
    public Vector2 middleVec;
    public Vector2 endVec;
    public float tick;

    public GameObject weaponPivot;

    [Header("파티클")]
    public ParticleSystem particle;
    protected override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.myPlayerInfo.SetOwner(this);
        SetMoveKeys();
        EnableEquipWeapon();
        SetKey("Alpha1", tt1);
        SetKey("Alpha2", tt2);
        SetKey("Alpha3", tt3);
        SetKey("Alpha4", tt4);
    }
    public void tt1()
    {
        DisableEquipWeapon();
        SetEquipWeapon(0);
        EnableEquipWeapon();
    }
    public void tt2()
    {
        DisableEquipWeapon();
        SetEquipWeapon(1);
        EnableEquipWeapon();
    }
    public void tt3()
    {
        DisableEquipWeapon();
        SetEquipWeapon(2);
        EnableEquipWeapon();
    }
    public void tt4()
    {
        DisableEquipWeapon();
        SetEquipWeapon(3);
        EnableEquipWeapon();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        SetAngle(weaponPivot, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        SetSideWallGravity(isWall);
        //Debug.Log(GetEquipWeapon().throwObject);
    }
    private void FixedUpdate()
    {
        MovingLeft(isLeftMoveInput);
        MovingRight(isRightMoveInput);
  
    }
    #region 이동
    private void SetSideWallGravity(int isWall)
    {
        if (isWall == 0 || (isRightMoveInput == false && isLeftMoveInput == false))
        {
            //어떠한 벽에도 붙지 않았을때
            sideWallGravity = 1;
            motionAnimation.SetBool("Climbing", false);
            attackAnimation.SetBool("Climbing", false);
            particle.gameObject.SetActive(false);
        }
        else
        {
            AttackCancel();
            motionAnimation.SetBool("Climbing", true);
            attackAnimation.SetBool("Climbing", true);
            particle.gameObject.SetActive(true);
            if (sideWallGravity > 0.5f)
            {
                sideWallGravity -= sideWallGravityPer * Time.deltaTime;
            }
        }
    }
    public override void UnitHit(AttackInfo info)
    {
        hp -= info.damage;
        //애니메이션 이동
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
            motionAnimation.SetBool("Moving", true);
            isWall = 0;
            isLeftMoveInput = false;
            rigid.velocity = new Vector2(-Speed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y * sideWallGravity);
            if (rigid.velocity.y < 0)
            {
                //벽에 붙었을때 왼쪽 이동
                if (!isOnFloor)
                {
                    motionAnimation.SetBool("Moving", false);
                    isWall = 1;
                }
                jumpCnt = 0;
            }
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
            rigid.velocity = new Vector2(0, rigid.velocity.y * sideWallGravity);
            if (rigid.velocity.y < 0)
            {
                if (!isOnFloor)
                {
                    motionAnimation.SetBool("Moving", false);
                    isWall = 2;
                }
                jumpCnt = 0;
            }
        }
    }
    public void Jump()
    {
        jumpCnt++;
        isJumpInput = true;
        footColider.enabled = false;
        sideWallGravity = 1;
        isWall = 0;
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
        tick = 0;
        isLeftMoveInput = true;
    }
    protected override void MoveRight()
    {
        tick += 0.01f;
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
    public void DisableEquipWeapon()
    {
        var obj=transform.Find("Motion/Hands/SwordPivot/" + equipWeapon.image);
        if (obj != null)
        {
            obj.gameObject.SetActive(false);
            muzzle = null;
        }
    }
    public void EnableEquipWeapon()
    {
        var obj = transform.Find("Motion/Hands/SwordPivot/" + equipWeapon.image);
        if (obj != null)
        {
            obj.gameObject.SetActive(true);
            muzzle= obj.Find("Muzzle").gameObject;
            weaponPivot = obj.Find("Pivot").gameObject;
        }
        attackAnimation.SetInteger("WeaponType", (int)equipWeapon.detailType);
    }
    protected override void Attack()
    {
        if (isWall == 0)
        {
            attackAnimation.SetBool("Attack", true);
            headAnimation.SetBool("Attack", true);
        }
        else
        {
            AttackCancel();
        }
    }
    protected override void AttackCancel()
    {
        attackAnimation.SetBool("Attack", false);
        headAnimation.SetBool("Attack", false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isOnFloor = true;
        }
    }
    protected override void OnFloorEvent()
    {
        if (UnitIsOnFloor())
        {
            footColider.enabled = true;
            if (rigid.velocity.y <= 0)
            {
                jumpCnt = 0;
                isWall = 0;
                SetJumpPower(walkingJumpPower);
                Speed = GetSpeed("Walk");
            }

        }
        else
        {
            SetJumpPower(jumpingJumpPower);
            Speed = GetSpeed("Jump");
            isOnFloor = false;
        }
        motionAnimation.SetBool("OnGrounding", isOnFloor);
    }
    public void SetAngle(GameObject obj, Vector3 point)
    {
        if (isWall != 0)
        {
            if (isWall == 1)
            {
                SetMotionDir(MyDir.right);
            }
            if (isWall == 2)
            {
                SetMotionDir(MyDir.left);
            }
            hand.transform.localScale = new Vector3(1, 1, 0);
            hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return;
        }
        float z = 180 - Mathf.Atan2(point.y - obj.transform.position.y, obj.transform.position.x - point.x) * 180 / Mathf.PI;
        if ((z <= 90 && z >= 0) || (z > 270 && z < 360))
        {
            SetMotionDir(MyDir.right);
            hand.transform.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            SetMotionDir(MyDir.left);
            hand.transform.localScale = new Vector3(-1, -1, 0);
        }
        hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
        //Debug.Log(180-Mathf.Atan2(point.y - obj.transform.position.y, obj.transform.position.x - point.x)*180/Mathf.PI) ;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, GameManager.Instance.aim.transform.position);
            Vector2 cen = (GameManager.Instance.aim.transform.position - transform.position) / 2;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(cen, 0.2f);


            //var tarAngle = (Mathf.Acos(Vector2.Dot(Vector2.right, cen) / Vector3.Magnitude(cen)) * Mathf.Rad2Deg);

            Quaternion zRotation;
            if (GetMotionDir() == MyDir.right)
            {
                zRotation = Quaternion.Euler(0f, 0f, 90f);  // 회전각
            }
            else
            {
                zRotation = Quaternion.Euler(0f, 0f, -90f);  // 회전각
            }
            Vector2 nonVec = (zRotation * cen).normalized;
            nonVec += cen + (Vector2)transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, nonVec);
            Gizmos.DrawLine(nonVec, GameManager.Instance.aim.transform.position);
            Gizmos.DrawSphere(nonVec, 0.2f);

            //시작
            //중간점
            //끝
            Vector2 startVec = transform.position;
            Vector2 middleVec = nonVec;
            Vector2 endVec = GameManager.Instance.aim.transform.position;

            float startPoint, middlePoint, endPoint;

            startPoint =(Pows(endVec.y, startVec.x, middleVec.x)+ Pows(middleVec.y, endVec.x, startVec.x)+ Pows(startVec.y, middleVec.x, endVec.x))/(2*(
                Minus(startVec.x,endVec.y,middleVec.y)+ Minus(middleVec.x, startVec.y, endVec.y)+ Minus(endVec.x, middleVec.y, startVec.y)
                ));

            middlePoint = (startVec.y - middleVec.y) / (Mathf.Pow(startVec.x, 2) - Mathf.Pow(middleVec.x, 2)+(2*startPoint*(middleVec.x-startVec.x)));


            endPoint = middleVec.y - (middlePoint * Mathf.Pow((middleVec.x - startPoint),2));
            //포인트
            //Debug.Log(startPoint + "/" + middlePoint + "/" + endPoint);
            Vector2 lines = new Vector2(startVec.x+tick, (middlePoint*Mathf.Pow(((startVec.x+tick) - startPoint),2))+endPoint);
            
            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(lines, 0.2f);
        }

    }
    public float Pows(float first , float v1,float v2)
    {

        //Debug.Log(first * (Mathf.Pow(v1, 2) - Mathf.Pow(v2, 2)) +":::  "+ first + "///" + v1 + "///" + v2);
        return first * (Mathf.Pow(v1, 2) - Mathf.Pow(v2, 2));
    }
    public float Minus(float v0,float v1,float v2)
    {
        //Debug.Log(v0 * (v1 - v2) + "<>>>:::  " + v0 + "///" + v1 + "///" + v2);
        return v0 * (v1 - v2);
    }
}

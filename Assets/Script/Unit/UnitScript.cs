using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    protected Rigidbody2D rigid;
    private float _speed;
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }
    [Header("오브젝트 설정")]
    public GameObject motion;
    public Vector3 motionSize;

    [SerializeField]
    [Header("점프 관련")]
    private float jumpPower;
    public float jumpingJumpPower;
    public float walkingJumpPower;
    public float sideWallGravityPer;
    public int maxJump;

    [Header("속도 관련")]
    public float setSpeed;
    public float walkingSpeed;
    public float jumpingSpeed;

    [Header("충돌 및 벽")]
    public Collider2D footColider;
    public LayerMask floorLayer;
    public Vector3 groundChkPos;
    public Vector3 groundChkSize;
    public Vector3 leftWallChkPos;
    public Vector3 rightWallChkPos;
    public Vector3 wallChkSize;
    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        motionSize = motion.transform.localScale;
    }
    // Update is called once per frame
    public void SetMoveKeys()
    {
        SetKey("A", MoveLeft,MoveLeftCancel);
        SetKey("D", MoveRight, MoveRightCancel);
        SetKey("W", MoveUp, MoveUpCancel);
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
        Debug.Log(gameObject.name + " Up InActive");
    }
    protected virtual void MoveDown()
    {
        Debug.Log(gameObject.name + " Down");
    }
    protected virtual void MoveDownCancel()
    {
        Debug.Log(gameObject.name + " Down InActive");
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
    protected virtual void OnFloorEvent()
    {

    }
    protected virtual void SetMotionDir(bool dir)
    {
        if (dir)
        {
            motion.transform.localScale = motionSize;
        }
        else
        {
            motion.transform.localScale = new Vector3(-motionSize.x, motionSize.y, motionSize.z);
        }
    }
    public bool UnitsOnLeftWall()
    {
        var wall = Physics2D.OverlapBox(transform.position + leftWallChkPos, wallChkSize, 0f, floorLayer);
        if (wall != null)
        {
            var sp = wall.gameObject.GetComponent<SpriteRenderer>();
            Debug.DrawRay(new Vector3(wall.transform.position.x+(sp.bounds.size.x/2), wall.transform.position.y, 0), Vector3.left,Color.green);
            if(transform.position.x < wall.transform.position.x + (sp.bounds.size.x / 2) && transform.position.x > wall.transform.position.x - (sp.bounds.size.x / 2) )
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public bool UnitsOnRightWall()
    {
        var wall = Physics2D.OverlapBox(transform.position + rightWallChkPos, wallChkSize, 0f, floorLayer);
        if (wall != null)
        {
            var sp = wall.gameObject.GetComponent<SpriteRenderer>();
            Debug.DrawRay(new Vector3(wall.transform.position.x - (sp.bounds.size.x / 2), wall.transform.position.y, 0), Vector3.right, Color.green);
            if (transform.position.x < wall.transform.position.x + (sp.bounds.size.x / 2) && transform.position.x > wall.transform.position.x - (sp.bounds.size.x / 2) )
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public bool UnitIsOnFloor()
    {
        if (Mathf.Floor(rigid.velocity.y) > 0)
        {
            return false;
        }
        var floors = Physics2D.OverlapBoxAll(transform.position+groundChkPos, groundChkSize, 0f,floorLayer);  
        if (floors.Length>0)
        {
            Collider2D floor=floors[0];
            foreach (var c in floors)
            {
                //세워져있는 땅과 아래땅을 구분하기 위해 가장 밑에있는 땅을 찾는다
                if (c.transform.position.y<floor.transform.position.y)
                {
                    floor = c;
                }
            }
            var sp = floor.gameObject.GetComponent<SpriteRenderer>();
            Debug.DrawRay(new Vector3(floor.transform.position.x,floor.transform.position.y + (sp.bounds.size.y / 2*0.88f),0), Vector3.down*0.2f,Color.blue);
            if ((floor.transform.position.y + (sp.bounds.size.y / 2*0.88f)) < transform.position.y+groundChkPos.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public void SetJumpPower(float power)
    {
        jumpPower = power;
    }
    public float GetJumpPower()
    {
        return jumpPower;
    }
    public float GetSpeed(string type)
    {
        float nowSpeed = setSpeed;
        if (type == "Walk")
        {
            nowSpeed *= walkingSpeed;
        }
        else
        {
            nowSpeed *= jumpingSpeed;
        }
        return nowSpeed;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position + groundChkPos, groundChkSize);
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position + leftWallChkPos, wallChkSize);
        Gizmos.DrawCube(transform.position + rightWallChkPos, wallChkSize);
    }
}

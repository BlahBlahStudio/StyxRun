using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MyDir
{
    left, right
}
public class UnitScript : MonoBehaviour
{
    public MyDir dir;
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
    [Header("������Ʈ ����")]
    public GameObject motion;
    public Vector3 motionSize;
    public Vector3 startPos;

    [SerializeField]
    [Header("���� ����")]
    private float jumpPower;
    public float jumpingJumpPower;
    public float walkingJumpPower;
    public float sideWallGravityPer;
    public int maxJump;

    [Header("�ӵ� ����")]
    public float setSpeed;
    public float walkingSpeed;
    public float jumpingSpeed;

    [Header("�̵� ����")]
    public bool isLeftMoveInput;
    public bool isRightMoveInput;

    [Header("�浹 �� ��")]
    public Collider2D footColider;
    public LayerMask floorLayer;
    public GameObject myFloor;
    public Vector3 groundChkPos;
    public Vector3 groundChkSize;
    public Vector3 leftWallChkPos;
    public Vector3 rightWallChkPos;
    public Vector3 wallChkSize;

    [Header("���� �ִϸ��̼�")]
    public Animator motionAnimation;

    [Header("Ŀ��� ����")]
    public Dictionary<AIController.AIBehaviors, Command> cmdList;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        cmdList = new Dictionary<AIController.AIBehaviors, Command>();
        rigid = GetComponent<Rigidbody2D>();
        motionAnimation = motion.GetComponent<Animator>();
        motionSize = motion.transform.localScale;
        startPos = transform.position;
    }
    // Update is called once per frame
    public virtual void Update()
    {
        OnFloorEvent();

    }
    public void SetMoveKeys()
    {
        SetKey("A", MoveLeft, MoveLeftCancel);
        SetKey("D", MoveRight, MoveRightCancel);
        SetKey("W", MoveUp, MoveUpCancel);
        SetKey("Mouse0", Attack, AttackCancel);
    }
    public void SetKey(string key, Command.Msg msg, Command.Msg unMsg = null)
    {
        Command cmd = new Command(key, msg, unMsg, gameObject);
        InputManager.SetKey(key, cmd);
    }
    public void SetAICommand(AIController.AIBehaviors key, Command.Msg msg, Command.Msg unMsg = null)
    {

        Command cmd = new Command(key.ToString(), msg, unMsg, gameObject);
        cmdList.Add(key, cmd);
    }
    protected virtual void Attack()
    {
        motionAnimation.SetBool("Attack", true);
    }
    protected virtual void AttackCancel()
    {
        motionAnimation.SetBool("Attack", false);
    }
    protected virtual void MoveUp()
    {
        Debug.Log(gameObject.name + " Up");
    }
    protected virtual void Idle()
    {
        isLeftMoveInput = false;
        isRightMoveInput = false;
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
        isLeftMoveInput = true;
        //Debug.Log(gameObject.name + " Left");
    }
    protected virtual void MoveLeftCancel()
    {
        isLeftMoveInput = false;
        Debug.Log(gameObject.name + " Left InActive");
    }
    protected virtual void MoveRight()
    {
        isRightMoveInput = true;
        Debug.Log(gameObject.name + " Right");
    }
    protected virtual void MoveRightCancel()
    {
        isRightMoveInput = false;
        Debug.Log(gameObject.name + " Right InActive");
    }
    public virtual void UnitHit()
    {
        Debug.Log("Unit Hit Event Enter");
    }
    public virtual void UnitHitEvent()
    {
        Debug.Log("Unit is Hit So , SomeThing Event");
    }
    protected virtual void UnitDie()
    {
        Debug.Log("Unit Die Event Enter");
    }
    public virtual void UnitDieEvent()
    {
        Debug.Log("Unit is Die So , SomeThing Event");
    }
    protected virtual void OnFloorEvent()
    {

    }
    public virtual int InAttackRange()
    {
        return 0;
    }
    public virtual bool InFloorRangeLeft()
    {
        return true;
    }
    public virtual bool InFloorRangeRight()
    {
        return true;
    }
    protected virtual void SetMotionDir(bool dir)
    {
        if (dir)
        {
            motion.transform.localScale = motionSize;
            this.dir = MyDir.right;
        }
        else
        {
            this.dir = MyDir.left;
            motion.transform.localScale = new Vector3(-motionSize.x, motionSize.y, motionSize.z);
        }
    }
    public bool UnitsOnLeftWall()
    {
        var wall = Physics2D.OverlapBox(transform.position + leftWallChkPos, wallChkSize, 0f, floorLayer);
        if (wall != null)
        {
            var sp = wall.gameObject.GetComponent<SpriteRenderer>();
            Debug.DrawRay(new Vector3(wall.transform.position.x + ((sp.bounds.size.x / 2) + rightWallChkPos.x), wall.transform.position.y, 0), Vector3.left, Color.green);
            if (transform.position.x < wall.transform.position.x + ((sp.bounds.size.x / 2) + rightWallChkPos.x) && transform.position.x > wall.transform.position.x - ((sp.bounds.size.x / 2) + rightWallChkPos.x))
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
            Debug.DrawRay(new Vector3(wall.transform.position.x - ((sp.bounds.size.x / 2) + rightWallChkPos.x), wall.transform.position.y, 0), Vector3.right, Color.green);
            if (transform.position.x < wall.transform.position.x + ((sp.bounds.size.x / 2) + rightWallChkPos.x) && transform.position.x > wall.transform.position.x - ((sp.bounds.size.x / 2) + rightWallChkPos.x))
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
        var floors = Physics2D.OverlapBoxAll(transform.position + groundChkPos, groundChkSize, 0f, floorLayer);
        if (floors.Length > 0)
        {
            Collider2D floor = floors[0];
            foreach (var c in floors)
            {
                //�������ִ� ���� �Ʒ����� �����ϱ� ���� ���� �ؿ��ִ� ���� ã�´�
                if (c.transform.position.y < floor.transform.position.y)
                {
                    floor = c;
                }
            }
            var sp = floor.gameObject.GetComponent<SpriteRenderer>();
            Debug.DrawRay(new Vector3(floor.transform.position.x, floor.transform.position.y + (sp.bounds.size.y / 2 * 0.88f), 0), Vector3.down * 0.2f, Color.blue);
            if ((floor.transform.position.y + (sp.bounds.size.y / 2 * 0.88f)) < transform.position.y + groundChkPos.y)
            {
                myFloor = floor.gameObject;
                return true;
            }
            else
            {
                myFloor = null;
                return false;
            }
        }
        else
        {
            myFloor = null;
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
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.8f);
        Gizmos.DrawWireSphere(transform.position + groundChkPos, 0.1f);
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position + groundChkPos, groundChkSize);
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position + leftWallChkPos, wallChkSize);
        Gizmos.DrawCube(transform.position + rightWallChkPos, wallChkSize);
    }
}

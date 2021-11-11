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
    [Header("오브젝트 설정")]
    public GameObject motion;
    protected Vector3 motionSize;
    protected Vector3 startPos;

    [Header("유닛 옵션")]
    public float mhp;
    public float hp;
    public float damage;
    public int initWeapon;
    public ItemWeapon equipWeapon;
    public Vector3 throwPoint;


    [SerializeField]
    [Header("점프 관련")]
    private float jumpPower; //점프력
    public float jumpingJumpPower; //공중에서 점프력
    public float walkingJumpPower;  //바닥에서 점프력
    public float sideWallGravityPer; //벽타기 할때 작용될 중력
    public int maxJump; //최대 점프 횟수

    [Header("속도 관련")]
    public float setSpeed; // 이동 속도
    public float walkingSpeed; //바닥 이동 속도 비율
    public float jumpingSpeed; //공중 이동 속도 비율

    [Header("이동 관련")]
    public bool isLeftMoveInput;
    public bool isRightMoveInput;

    [Header("충돌 및 벽")]
    public Collider2D footColider; // 실제 바닥체크 콜라이더
    public LayerMask floorLayer; //바닥들의 레이어
    protected GameObject myFloor; //현재 내가 딛고 있는 바닥
    public Vector3 groundChkPos;
    public Vector3 groundChkSize;
    public Vector3 sideWallChkPos;
    public Vector3 sideWallChkSize;

    [HideInInspector]
    public Animator motionAnimation;

    [Header("공격 타입")]
    public AttackManager attackManager;

    [Header("커멘드 관련")]
    public Dictionary<AIController.AIBehaviors, Command> cmdList;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        attackManager = GetComponentInChildren<AttackManager>();
        cmdList = new Dictionary<AIController.AIBehaviors, Command>();
        rigid = GetComponent<Rigidbody2D>();
        motionAnimation = motion.GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        motionSize = motion.transform.localScale;
        startPos = transform.position;
        attackManager.SetOwner(this);
        SetEquipWeapon(initWeapon);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        OnFloorEvent();
        SetThrowPoint();
    }
    public ItemWeapon GetEquipWeapon()
    {
        return equipWeapon;
    }
    public void SetEquipWeapon(int index)
    {
        //무기 인덱스로 무기 정보 생성후 리턴
        equipWeapon = GameManager.Instance.GetItemWeapon(index, this);
    }
    public void SetMoveKeys()
    {
        SetKey("A", MoveLeft, MoveLeftCancel);
        SetKey("D", MoveRight, MoveRightCancel);
        SetKey("W", MoveUp, MoveUpCancel);
        SetKey("Mouse0", Attack, AttackCancel);
        //SetKey("X", () => { Debug.Log(DataManager.GetData(DBList.Item, 0, "Name")); }, null);
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
        //Debug.Log(gameObject.name + " Up");
    }
    protected virtual void Idle()
    {
        isLeftMoveInput = false;
        isRightMoveInput = false;
    }
    protected virtual void MoveUpCancel()
    {
        //Debug.Log(gameObject.name + " Up InActive");
    }
    protected virtual void MoveDown()
    {
       // Debug.Log(gameObject.name + " Down");
    }
    protected virtual void MoveDownCancel()
    {
        //Debug.Log(gameObject.name + " Down InActive");
    }
    protected virtual void MoveLeft()
    {
        isLeftMoveInput = true;
        //Debug.Log(gameObject.name + " Left");
    }
    protected virtual void MoveLeftCancel()
    {
        isLeftMoveInput = false;
        //Debug.Log(gameObject.name + " Left InActive");
    }
    protected virtual void MoveRight()
    {
        isRightMoveInput = true;
        //Debug.Log(gameObject.name + " Right");
    }
    protected virtual void MoveRightCancel()
    {
        isRightMoveInput = false;
        //Debug.Log(gameObject.name + " Right InActive");
    }
    public virtual void UnitHit(AttackInfo info)
    {

        //Debug.Log("Unit Hit Event Enter, Dmage:"+damage);
    }
    public virtual void UnitHitEvent()
    {
        //Debug.Log("Unit is Hit So , SomeThing Event");
    }
    protected virtual void UnitDie()
    {
        //Debug.Log("Unit Die Event Enter");
    }
    public virtual void UnitDieEvent()
    {
       // Debug.Log("Unit is Die So , SomeThing Event");
    }
    public MyDir FindUnitDirection(GameObject obj)
    {
        if (transform.position.x < obj.transform.position.x)
        {
            return MyDir.right;
        }
        else
        {
            return MyDir.left;
        }
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
    protected virtual void SetMotionDir(MyDir dir)
    {
        this.dir =dir;

        if (dir==MyDir.right)
        {
            motion.transform.localScale = motionSize;
        }
        else if (dir == MyDir.left)
        {
            motion.transform.localScale = new Vector3(-motionSize.x, motionSize.y, motionSize.z);
        }
    }
    protected virtual void SetThrowPoint()
    {

    }
    public bool UnitsOnLeftWall()
    {
        var wall = Physics2D.OverlapBox(transform.position + new Vector3(-sideWallChkPos.x,sideWallChkPos.y), sideWallChkSize, 0f, floorLayer);
        if (wall != null)
        {
            var sp = wall.gameObject.GetComponent<SpriteRenderer>();
            Debug.DrawRay(new Vector3(wall.transform.position.x + ((sp.bounds.size.x / 2) + sideWallChkPos.x), wall.transform.position.y, 0), Vector3.left, Color.green);
            if (transform.position.x < wall.transform.position.x + ((sp.bounds.size.x / 2) + sideWallChkPos.x) && transform.position.x > wall.transform.position.x - ((sp.bounds.size.x / 2) + sideWallChkPos.x))
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public bool UnitsOnRightWall()
    {
        var wall = Physics2D.OverlapBox(transform.position + sideWallChkPos, sideWallChkSize, 0f, floorLayer);
        if (wall != null)
        {
            var sp = wall.gameObject.GetComponent<SpriteRenderer>();
            Debug.DrawRay(new Vector3(wall.transform.position.x - ((sp.bounds.size.x / 2) + sideWallChkPos.x), wall.transform.position.y, 0), Vector3.right, Color.green);
            if (transform.position.x < wall.transform.position.x + ((sp.bounds.size.x / 2) + sideWallChkPos.x) && transform.position.x > wall.transform.position.x - ((sp.bounds.size.x / 2) + sideWallChkPos.x))
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
                //세워져있는 땅과 아래땅을 구분하기 위해 가장 밑에있는 땅을 찾는다
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
        Gizmos.DrawCube(transform.position + sideWallChkPos, sideWallChkSize);
        Gizmos.DrawCube(transform.position + new Vector3(-sideWallChkPos.x,sideWallChkPos.y), sideWallChkSize);
    }
}

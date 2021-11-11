using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : UnitScript
{

    public enum MyState
    {
        Idle, Walk, Attack, Hit, Die
    }

    public FSMMachine<MonsterScript> fsmMachine;
    public Dictionary<int,FSM<MonsterScript>> states;

    [Header("공격 관련")]
    public bool isAttack;
    public float attackSize;


    [Header("공격 받음 관련")]
    public bool isHit;
    // Start is called before the first frame update

    protected override void Awake()
    {
        
        base.Awake();
        fsmMachine = new FSMMachine<MonsterScript>();
        states = new Dictionary<int,FSM<MonsterScript>>();
        states.Add((int)MyState.Idle, new IdleState(this));
        states.Add((int)MyState.Walk, new WalkState(this));
        states.Add((int)MyState.Attack, new AttackState(this));
        states.Add((int)MyState.Hit, new HitState(this));
        states.Add((int)MyState.Die, new DieState(this));
        fsmMachine.SetState(states[(int)MyState.Idle]);
    }
    protected override void Start()
    {
        base.Start();
        SetAICommand(AIController.AIBehaviors.Idle, Idle, null);
        SetAICommand(AIController.AIBehaviors.MoveLeft, MoveLeft, MoveLeftCancel);
        SetAICommand(AIController.AIBehaviors.MoveRight, MoveRight, MoveRightCancel);
        SetAICommand(AIController.AIBehaviors.Attack, Attack, AttackCancel);
        SetAICommand(AIController.AIBehaviors.Find, Find,null);
        SetAICommand(AIController.AIBehaviors.FindAndAttack, FindAndAttack, AttackCancel);
        //SetKey("C", MoveLeft, MoveLeftCancel);
        //SetKey("V", MoveRight, MoveRightCancel);
        //SetKey("B", Attack, AttackCancel);
        //SetKey("N", UnitHit, null);
        //SetKey("M", UnitDie, null);
        fsmMachine.Begin();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        fsmMachine.Run();
    }
    public void Find()
    {
       SetMotionDir(FindUnitDirection(GameManager.Instance.player.gameObject));
    }
    public void FindAndAttack()
    {
        Find();
        Attack();
    }
    protected override void Idle()
    {
        base.Idle();
        isAttack=false;
    }
    public void MoveLeftAction()
    {
        SetMotionDir(MyDir.left);
        if (!UnitsOnLeftWall())
        {
            rigid.velocity = new Vector2(-Speed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }
    public void MoveRightAction()
    {
        SetMotionDir(MyDir.right);
        if (!UnitsOnRightWall())
        {
            rigid.velocity = new Vector2(Speed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }
    public void MoveEndAction()
    {
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
    }

    protected override void Attack()
    {
        isAttack = true;
    }
    protected override void AttackCancel()
    {
        isAttack = false;
    }

    public override void UnitHit(AttackInfo info)
    {
        base.UnitHit(info);
        if (!motionAnimation.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            hp -= info.damage;
            fsmMachine.Change(states[(int)MyState.Hit]);
        }
    }
    protected override void UnitDie()
    {
        base.UnitDie();
        fsmMachine.Change(states[(int)MyState.Die]);
    }
    public override void UnitDieEvent()
    {
        base.UnitDieEvent();
    }
    public override void UnitHitEvent()
    {
        base.UnitHitEvent();

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
    public virtual void AttackEvent()
    {

    }
    public override int InAttackRange()
    {
        if ((Vector2.Distance(GameManager.Instance.player.transform.position, transform.position) <= attackSize)
            )
        {
            if (GameManager.Instance.player.transform.position.x >transform.position.x)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        return 0;
    }
    public override bool InFloorRangeLeft()
    {
        if (myFloor != null)
        {
            SpriteRenderer floorSp =myFloor.GetComponent<SpriteRenderer>();
            if (myFloor.transform.position.x - ((floorSp.bounds.size.x / 2)*0.95) > transform.position.x)
            {
                return false;
            }
        }

        if (isHit && GameManager.Instance.player.transform.position.x > transform.position.x)
        {
            //공격 받은 상태라면 추격을 위해
            return false;
        }
        return true;
    }
    public override bool InFloorRangeRight()
    {
        if (myFloor != null)
        {
            SpriteRenderer floorSp =myFloor.GetComponent<SpriteRenderer>();
            if (myFloor.transform.position.x + ((floorSp.bounds.size.x / 2) * 0.95) < transform.position.x)
            {
                return false;
            }
        }
        if (isHit && GameManager.Instance.player.transform.position.x < transform.position.x)
        {
            //공격 받은 상태라면 추격을 위해
            return false;
        }
        return true;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawSphere(transform.position, attackSize);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target
{
    public GameObject obj;
    public Vector3 pos;


    public Target(GameObject obj)
    {
        this.obj = obj;
    }
    public Target(Vector3 pos)
    {
        this.pos = pos;
    }
    public Target(GameObject obj, Vector3 pos)
    {
        this.obj = obj;
        this.pos = pos;
    }
    public Vector3 GetTargetPos()
    {
        if (obj != null)
        {
            return obj.transform.position;
        }
        return Vector3.zero;
    }
    public Vector3 GetSetPos()
    {
        return pos;
    }
    public void ReSetPos(Vector3 pos)
    {
        this.pos = pos;
    }
}
public class HitTarget
{
    public enum Detected
    {
        Player,
        Floor,
        Monster,
        None
    }
    public UnitScript obj;
    public Detected state;
    public void SetState(Detected state)
    {
        this.state = state;
    }
    public Detected TagToState()
    {
        try
        {
            return (Detected)System.Enum.Parse(typeof(Detected), obj.tag);
        }
        catch
        {
            return Detected.None;
        }
    }
}
public class ThrowObjectScript : MonoBehaviour
{

    public float speed;
    public MyDir dir;

    [Header("공격 범위")]
    public Vector3 attackPos;
    public Vector3 attackSize;
    public LayerMask attackLayer;

    [HideInInspector]
    public float angle;
    [HideInInspector]
    public Vector3 startPosition;
    public Target target;

    public Animator anim;
    protected bool isThrowing;

    [Header("공격 정보")]
    public UnitScript owner;
    public AttackInfo attackInfo;
    public HitTarget.Detected myDetected;
    // Start is called before the first frame update
    private void Awake()
    {
        isThrowing = true;
    }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        SetStartOffset();
        if (owner.tag == "Monster")
        {
            myDetected = HitTarget.Detected.Monster;
        }
        else if (owner.tag == "Player")
        {
            myDetected = HitTarget.Detected.Player;
        }
        Move();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ThrowingStateChange();
        Move();
        DetectAndAttack();
    }
    public virtual void Move()
    {
        if (isThrowing)
        {
            angle = 180 - Mathf.Atan2(target.GetSetPos().y - startPosition.y, startPosition.x - target.GetSetPos().x) * Mathf.Rad2Deg;
            transform.position += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * speed, Mathf.Sin(angle * Mathf.Deg2Rad) * speed) * Time.deltaTime;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    public virtual void DetectAndAttack()
    {
        if (!isThrowing)
            return;

        var target = Detect();
        if (target.state == myDetected)
        {
            return;
        }
        if (target.state == HitTarget.Detected.None)
        {
            
            return;
        }else if (target.state == HitTarget.Detected.Player)
        {
            Attack(target);
            return;
        }else if (target.state == HitTarget.Detected.Floor)
        {
            EffectOn();
            return;
        }
        else if (target.state == HitTarget.Detected.Monster)
        {
            Attack(target);
            return;
        }
    }
    public virtual void ThrowingStateChange()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Effect") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75)
        {
            Destroy(gameObject);
        }
        anim.SetBool("Hit", !isThrowing);
    }
    public virtual void EffectOn()
    {
        isThrowing = false;
    }
    public virtual void Attack(HitTarget target)
    {
        UnitScript targetObj = target.obj.GetComponent<UnitScript>();
        if (targetObj.hp > 0)
        {
            targetObj.UnitHit(attackInfo);
            EffectOn();
        }
    }
    public virtual HitTarget Detect()
    {
        var hits= Physics2D.OverlapBoxAll(transform.position + attackPos, attackSize,0f,attackLayer);
        HitTarget target=new HitTarget();
        float minDistance=99999;
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.isTrigger == false && hit.tag!="Floor")
                {
                    continue;
                }
                if (hit.tag == "Player" || hit.tag=="Floor" || hit.tag=="Monster") { 
                    if (minDistance > Mathf.Abs(Vector2.Distance(hit.transform.position, transform.position)))
                    {
                        minDistance = Mathf.Abs(Vector2.Distance(hit.transform.position, transform.position));
                        target.obj = hit.GetComponent<UnitScript>();
                        target.SetState(target.TagToState());
                    }
                }
            }
        }
        if (target.obj == null)
        {
            target.state = HitTarget.Detected.None;
            return target;
        }
        return target;
    }
    public void SetStartOffset()
    {
        //반대편을 바라보면서 공격할때는 반대편으로 보내는게 아니라 그냥 보는 방향대로 보내게 하기 위해서 존재
        var distance = Mathf.Abs(target.GetSetPos().x - startPosition.x);
        if (dir == MyDir.right)
        {
            if (startPosition.x > target.GetSetPos().x)
            {
                target.ReSetPos(new Vector3(target.GetSetPos().x+(2*distance), target.GetSetPos().y));
            }
        }
        else
        {
            if (startPosition.x < target.GetSetPos().x)
            {
                target.ReSetPos(new Vector3(target.GetSetPos().x-(2*distance), target.GetSetPos().y));
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position+attackPos,attackSize);
    }
}

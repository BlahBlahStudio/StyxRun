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
    public GameObject obj;
    public Detected state;
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

    protected Animator anim;
    protected bool isThrowing;
    // Start is called before the first frame update
    private void Awake()
    {
        isThrowing = true;
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        SetStartOffset();
    }

    // Update is called once per frame
    void Update()
    {
        ThrowingStateChange();
        Move();
        DetectAndAttack();
    }
    public virtual void Move()
    {
        if (isThrowing == true)
        {
            angle = 180 - Mathf.Atan2(target.GetSetPos().y - startPosition.y, startPosition.x - target.GetSetPos().x) * Mathf.Rad2Deg;
            transform.position += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * speed, Mathf.Sin(angle * Mathf.Deg2Rad) * speed) * Time.deltaTime;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    public virtual void DetectAndAttack()
    {
        var target = Detect();
        if (target.state == HitTarget.Detected.None)
        {

            return;
        }else if (target.state == HitTarget.Detected.Player)
        {
            EffectOn();
            return;
        }else if (target.state == HitTarget.Detected.Floor)
        {
            EffectOn();
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
    public virtual void Attack()
    {

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
                if (hit.tag == "Player") { 
                    if (minDistance > Mathf.Abs(Vector2.Distance(hit.transform.position, transform.position)))
                    {
                        minDistance = Mathf.Abs(Vector2.Distance(hit.transform.position, transform.position));
                        target.obj = hit.gameObject;
                        target.state = HitTarget.Detected.Floor;
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

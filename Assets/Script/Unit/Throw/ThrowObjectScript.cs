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
public class ThrowObjectScript : MonoBehaviour
{
    public float speed;
    public float angle;
    public Target target;
    public Vector3 startPosition;
    public MyDir dir;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        SetStartOffset();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public virtual void Move()
    {
        angle = 180 - Mathf.Atan2(target.GetSetPos().y - startPosition.y, startPosition.x - target.GetSetPos().x) * Mathf.Rad2Deg;
        transform.position += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * speed, Mathf.Sin(angle * Mathf.Deg2Rad) * speed) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
}

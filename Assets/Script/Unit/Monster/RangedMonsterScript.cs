using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonsterScript : MonsterScript
{
    [Header("АјАн")]
    public GameObject throwObj;
    public float throwSpeed;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = new Color(1, 0, 0, 0.8f);
        if (dir == MyDir.right)
        {
            Gizmos.DrawWireSphere(transform.position + new Vector3(throwPoint.x,throwPoint.y), 0.1f);
        }
        else if(dir == MyDir.left)
        {
            Gizmos.DrawWireSphere(transform.position + new Vector3(-throwPoint.x, throwPoint.y), 0.1f);
        }
       
    }
}

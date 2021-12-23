using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowArrow : ThrowObjectScript
{
    // Start is called before the first frame update
    private float spawnTimer;
    public Vector2 startVec, middleVec, endVec;
  

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    public override void Move()
    {
        if (isThrowing)
        {
            Debug.Log("hi");
            MoveTest(startVec, middleVec, endVec);
            spawnTimer += Time.deltaTime;
        }
    }
    public void SetParabola(Vector2 startVec,Vector2 middleVec,Vector2 endVec)
    {
        this.startVec = startVec;
        this.middleVec = middleVec;
        this.endVec = endVec;
    }
    public void MoveTest(Vector2 startVec , Vector2 middleVec , Vector2 endVec)
    {
        float startPoint, middlePoint, endPoint;

        startPoint = (Pows(endVec.y, startVec.x, middleVec.x) + Pows(middleVec.y, endVec.x, startVec.x) + Pows(startVec.y, middleVec.x, endVec.x)) / (2 * (
            Minus(startVec.x, endVec.y, middleVec.y) + Minus(middleVec.x, startVec.y, endVec.y) + Minus(endVec.x, middleVec.y, startVec.y)
            ));

        middlePoint = (startVec.y - middleVec.y) / (Mathf.Pow(startVec.x, 2) - Mathf.Pow(middleVec.x, 2) + (2 * startPoint * (middleVec.x - startVec.x)));


        endPoint = middleVec.y - (middlePoint * Mathf.Pow((middleVec.x - startPoint), 2));
        //Æ÷ÀÎÆ®
        Debug.Log(startPoint + "/" + middlePoint + "/" + endPoint);
        transform.position = new Vector2(startVec.x + spawnTimer, (middlePoint * Mathf.Pow(((startVec.x + spawnTimer) - startPoint), 2)) + endPoint);

   
    }
    public float Pows(float first, float v1, float v2)
    {
        Debug.Log(first * (Mathf.Pow(v1, 2) - Mathf.Pow(v2, 2)) + ":::  " + first + "///" + v1 + "///" + v2);
        return first * (Mathf.Pow(v1, 2) - Mathf.Pow(v2, 2));
    }
    public float Minus(float v0, float v1, float v2)
    {
        Debug.Log(v0 * (v1 - v2) + "<>>>:::  " + v0 + "///" + v1 + "///" + v2);
        return v0 * (v1 - v2);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(startVec,endVec);
        var co = new Color(0, 1, 0, 0.5f);
        Gizmos.color = co;
        Gizmos.DrawSphere(middleVec, 0.2f);
    }
}

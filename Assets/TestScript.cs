using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform t;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawLine(t.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            Gizmos.DrawLine(t.position,t.position+Vector3.right);
        }
    }
}

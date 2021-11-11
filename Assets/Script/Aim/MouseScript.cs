using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
        //var angle = 180-Mathf.Atan2(mousePos.y - GameManager.Instance.player.transform.position.y, mousePos.x - GameManager.Instance.player.transform.position.x) * Mathf.Rad2Deg;
        //Debug.Log(angle);
    }
}

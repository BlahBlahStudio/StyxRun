using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedMonsterScript : MonsterScript
{
 
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        UnitUIScript ui = Instantiate(GameManager.Instance.monsterUI, transform, false).GetComponent<UnitUIScript>();
        ui.SetOwner(this);
        ui.transform.position = transform.position + uiPosition;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

}

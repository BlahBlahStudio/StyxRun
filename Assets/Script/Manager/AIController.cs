using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    public UnitScript obj;
    Coroutine state;
    public delegate bool Require();
    public enum AIBehaviors
    {
        Idle, MoveLeft, MoveRight, Attack,Find,FindAndAttack
    }

    // Update is called once per frame
    void Update()
    {
        if (state == null)
        {
            List<Require> req = new List<Require>();
            float time;
            int type = obj.InAttackRange();
            if (((MonsterScript)obj).fsmMachine.nowState == ((MonsterScript)obj).states[(int)MonsterScript.MyState.Idle])
            {
                if (type == 0)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            req.Add(obj.InFloorRangeLeft);
                            time = Random.Range(0.2f, 1.5f);
                            state = StartCoroutine(AIBehavior(time, AIBehaviors.MoveLeft, req));
                            break;
                        case 1:
                            req.Add(obj.InFloorRangeRight);
                            time = Random.Range(0.2f, 1.5f);
                            state = StartCoroutine(AIBehavior(Random.Range(1, 3), AIBehaviors.MoveRight, req));
                            break;
                        default:
                            req.Add(()=> { if (obj.InAttackRange() != 0) { return false; } else { return true; } });
                            time = Random.Range(1f, 2f);
                            state = StartCoroutine(AIBehavior(Random.Range(1, 3), AIBehaviors.Idle, req));
                            break;
                            //state = StartCoroutine(AIMove(Random.Range(1, 3), GetBehavior(Random.Range(0, 4))));
                    }
                }
                else if (type == 1 || type == -1)
                {                    
                    switch (Random.Range(0, 10))
                    {
                        case 0:
                            req.Add(obj.InFloorRangeLeft);
                            time = Random.Range(0.5f, 1f);
                            state = StartCoroutine(AIBehavior(time, AIBehaviors.MoveLeft, req));
                            break;
                        case 1:
                            req.Add(obj.InFloorRangeRight);
                            time = Random.Range(0.5f, 1f);
                            state = StartCoroutine(AIBehavior(Random.Range(1, 3), AIBehaviors.MoveRight, req));
                            break;
                        case 2:
                            time = Random.Range(0.2f, 0.7f);
                            state = StartCoroutine(AIBehavior(Random.Range(1, 3), AIBehaviors.Idle, req));
                            break;
                        case 3:
                            time = Random.Range(0.2f, 1f);
                            state = StartCoroutine(AIBehavior(Random.Range(1, 3), AIBehaviors.Find, req));
                            break;
                        case 4:
                            time = Random.Range(0.2f, 0.4f);
                            state = StartCoroutine(AIBehavior(Random.Range(1, 3), AIBehaviors.FindAndAttack, req));
                            break;
                        case 5:
                            time = Random.Range(0.2f, 0.4f);
                            state = StartCoroutine(AIBehavior(Random.Range(1, 3), AIBehaviors.FindAndAttack, req));
                            break;
                        default:
                            time = Random.Range(0.1f, 0.5f);
                            state = StartCoroutine(AIBehavior(Random.Range(1, 3), AIBehaviors.FindAndAttack, req));
                            break;
                            //state = StartCoroutine(AIMove(Random.Range(1, 3), GetBehavior(Random.Range(0, 4))));
                    }
                }
            }
        }
    }
    IEnumerator AIBehavior(float time, AIBehaviors behavior, List<Require> reqs)
    {
        while (time > 0)
        {
            bool requirCheck = true;
            foreach (var req in reqs)
            {
                if (!req()) { requirCheck = false; break; }
            }
            if (!requirCheck) break;
            obj.cmdList[behavior].Execute();
            time -= Time.deltaTime;
            yield return null;
        }
        obj.cmdList[behavior].unExecute();
        state = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    protected List<KeyCode> m_activeInputs = new List<KeyCode>();
    // Update is called once per frame
    public Dictionary<string, Command> keys = new Dictionary<string, Command>();
    private string selectedKey;
    List<string> key = new List<string>{
        "Q", "W", "E","R","T","Y", 
          "A", "S","D","F","G","H",
           "Z", "X","C","V","B","N",
        "Space","1","2","3","4","5"};
    private void Awake()
    {
        instance = this;
        foreach (string k in key)
        {
            keys.Add(k, new Command(k));
        }
    }
    void Update()
    {
        List<KeyCode> pressedInput = new List<KeyCode>();
        if (Input.anyKeyDown || Input.anyKey)
        {
            foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(code))
                {
                    //똑같은 키입력 여러개 생기는거 방지
                    m_activeInputs.Remove(code);
                    m_activeInputs.Add(code);
                    pressedInput.Add(code);
                    if (keys.ContainsKey(code.ToString()))
                    {
                        keys[code.ToString()].Execute();
                    }
                    //Debug.Log(code.ToString()+ "/"+code + " was pressed");
                }
            }
        }
        List<KeyCode> releasedInput = new List<KeyCode>();

        foreach (KeyCode code in m_activeInputs)
        {
            releasedInput.Add(code);

            if (!pressedInput.Contains(code))
            {
                releasedInput.Remove(code);
                if (keys.ContainsKey(code.ToString()))
                {
                    keys[code.ToString()].unExecute();
                }
            }
        }
        m_activeInputs = releasedInput;
    }
    public static void SetKey(string key,Command c)
    {
        instance.keys[key].unExecute();
        instance.keys[key] = c;
    }
}

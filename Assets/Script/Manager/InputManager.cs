using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    // Update is called once per frame
    public Dictionary<string, Command> keys = new Dictionary<string, Command>();
    private string selectedKey;
    List<string> key = new List<string>{
        "Q", "W", "E","R","T","Y", 
          "A", "S","D","F","G","H",
           "Z", "X","C","V","B","N",
        " ","1","2","3","4","5"};
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
        selectedKey = Input.inputString.ToUpper();
        if (keys.ContainsKey(selectedKey)) {
            keys[selectedKey].Execute();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            keys["A"].unExecute();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            keys["D"].unExecute();
        }
    }
    public static void SetKey(string key,Command c)
    {
        instance.keys[key].unExecute();
        instance.keys[key] = c;
    }
}

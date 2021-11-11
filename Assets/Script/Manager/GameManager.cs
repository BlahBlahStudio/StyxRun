using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Start is called before the first frame update
    public static GameManager Instance;
    public UnitScript player;
    Dictionary<string, string> data;
    public List<GameObject> effectList;
    public List<GameObject> throwObjectList;
    private void Awake()
    {
        Instance = this;
        if (DataManager.Instance == null)
        {
            Debug.Log("no");
            SceneManager.LoadScene("LobbyScene");
        }
    }
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {

    }
    public ItemWeapon GetItemWeapon(int index,UnitScript owner)
    {
        string name=DataManager.GetData(DBList.Item, index, "Name");
        float damage=float.Parse(DataManager.GetData(DBList.Item, index, "Damage"));
        //index로 공격 내용 조절////////////////
        var behaviour = DataManager.GetBehaviourList(int.Parse(DataManager.GetData(DBList.Item, index, "Behaviour")), owner);
        ItemWeapon.ItemDetailType detailType = ItemWeapon.ItemDetailType.Sword;
        ////////////////////////////////////////
        ItemWeapon item= new ItemWeapon(index,ItemType.Weapon,detailType, name, null, behaviour, damage);
        return item;
    }
}

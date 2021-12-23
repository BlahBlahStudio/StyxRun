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
    public GameObject monsterUI;
    [Header("에임정보")]
    public GameObject aim;

    [Header("현재 타겟팅 몬스터 정보")]
    public UnitUIScript targetUI;
    public UnitScript target;
    public float renderingTime;
   
    [Header("선택된 플레이어 정보")]
    public UnitUIScript myPlayerInfo;

    private void Awake()
    {
        Instance = this;
        if (DataManager.Instance == null)
        {
            SceneManager.LoadScene("LobbyScene");
        }
    }
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        PlayerUIRender();
        TargetUIRender();
    }
    public ItemWeapon GetItemWeapon(int index,UnitScript owner)
    {
        string name=DataManager.GetData(DBList.Item, index, "Name");
        float damage=float.Parse(DataManager.GetData(DBList.Item, index, "Damage"));
        //index로 공격 내용 조절////////////////
        var behaviour = DataManager.GetBehaviourList(int.Parse(DataManager.GetData(DBList.Item, index, "Behaviour")), owner);
        ////////////////////////////////////////

        ItemWeapon item = new ItemWeapon(index,owner);
        //ItemWeapon item= new ItemWeapon(index,ItemType.Weapon,detailType, name, null, behaviour, damage);
        return item;
    }
    public void OnTargetUI(UnitScript obj)
    {
        targetUI.SetOwner(obj);
        SetTarget(obj);
        renderingTime = Mathf.Clamp(renderingTime + 10, 0, 10);
    }
    public void PlayerUIRender()
    {
        if (myPlayerInfo.GetOwner() != null)
        {
            myPlayerInfo.gameObject.SetActive(true);
        }
    }
    public void TargetUIRender()
    {
        if (renderingTime > 0 && target != null && target.hp>0)
        {
            targetUI.gameObject.SetActive(true);
            renderingTime -= Time.deltaTime;
        }
        else
        {
            renderingTime = 0;
            targetUI.gameObject.SetActive(false);
        }
    }
    public void SetTarget(UnitScript obj)
    {
        target = obj;
    }
    public UnitScript GetTarget()
    {
        return target;
    }
}

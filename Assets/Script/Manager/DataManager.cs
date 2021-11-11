using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase;
public enum DBList
{
    Item,
    Monster,
    Behaviour,
    None
}
[System.Serializable]
public class test
{
    public float a;
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public static Dictionary<string, IDictionary[]> dataList;
    public bool dataLoadingSuccess;
    public Dictionary<string, bool> dataLoadCheckList;

    public void GetDB(DBList type)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
        .GetReference(type.ToString())
        .GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(type.ToString() + "연결 실패");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                int index = 0;
                DataSnapshot snapshot = task.Result;
                dataList[type.ToString()] = new IDictionary[snapshot.ChildrenCount];
                foreach (DataSnapshot source in snapshot.Children)
                {
                    //받은 데이터들을 하나씩 잘라 string 배열에 저장
                    dataList[type.ToString()][index] = (IDictionary)source.Value;
                    dataLoadCheckList[type.ToString()] = true;
                    //Debug.Log(noticeOption[index]["thema"]);
                    //Debug.Log(dataList[type.ToString()][index]["Name"] + ":" + dataList[type.ToString()][index]["Icon"]);
                    index++;
                }
            }
        });

    }
    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);//캐시
        FirebaseApp.CheckAndFixDependenciesAsync();
        dataList = new Dictionary<string, IDictionary[]>();
        dataLoadCheckList = new Dictionary<string, bool>();
    }
    public void Start()
    {
        for (DBList type = DBList.Item; type < DBList.None; type++)
        {
            dataList.Add(type.ToString(),null);
            dataLoadCheckList.Add(type.ToString(), false);
            GetDB(type);
        }
        StartCoroutine(CheckDataLoad());
    }
    IEnumerator CheckDataLoad()
    {
        dataLoadingSuccess = false;
        bool loading = false;
        while (loading == false)
        {
            loading = true;
            for (DBList type = DBList.Item; type < DBList.None; type++)
            {
                if (dataLoadCheckList.ContainsKey(type.ToString()) == false)
                {
                    Debug.Log(type.ToString());
                    loading = false;
                }
            }
            yield return null;
        }
        dataLoadingSuccess = true;
    }
    public static string GetData(DBList type,int index,string colume)
    {
        string data="null";
        if (dataList[type.ToString()] != null)
        {
            if (dataList[type.ToString()].Length > index)
            {
                if (dataList[type.ToString()][index].Contains(colume))
                {
                    data = dataList[type.ToString()][index][colume].ToString();
                }
            }
        }
        return data;
    }
    public static AttackScript GetBehaviourList(int index,UnitScript owner) {
        AttackScript data;
        switch (index) {
            case 0:
                data = new SwordAttackScript(owner);
                break;
            case 1:
                data = new RangedAttackScript(owner);
                break;
            default:
                data = null;
                break;
        }
        return data;
    }

    public void LoadGameScene()
    {
        if (dataLoadingSuccess)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            
        }
    }
}

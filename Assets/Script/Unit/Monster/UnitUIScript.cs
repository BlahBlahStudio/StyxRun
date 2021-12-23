using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitUIScript : MonoBehaviour
{
    private UnitScript own;
    public Text unitName;
    public Text hpText;
    public Image hpImg;
    public Image icon;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (own != null)
        {
            BarRendering(hpImg, own.mhp, own.hp);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void SetOwner(UnitScript unit)
    {
        gameObject.SetActive(true);
        own = unit;
    }
    public UnitScript GetOwner()
    {
        return own;
    }
    public void SetNameText()
    {
        //own index로 아이콘 찾기
        unitName.text = "혼령";
    }
    public void BarRendering(Image img,float max,float cur)
    {
        img.fillAmount = (cur / max );
        if (hpText != null)
        {
            hpText.text=(int)cur+"/"+ (int)max;
        }
        if (icon != null)
        {
            //own index로 아이콘 찾기
        }
    }
    
}

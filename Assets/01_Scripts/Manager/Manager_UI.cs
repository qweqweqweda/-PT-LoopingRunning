using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_UI : Singleton<Manager_UI>
{
    public Image level_img;
    public Text level_txt;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        Set_UI_Level();
    }

    public void Set_UI_Level()
    {
        int level = Manager_UD.Instance.level_ud.level + 1;
        float exp = Manager_UD.Instance.level_ud.exp;

        if (exp >= level * 10)
        {
            Manager_UD.Instance.level_ud.LevelUP(1, level * 10);

            level = Manager_UD.Instance.level_ud.level + 1;
            exp = Manager_UD.Instance.level_ud.exp;
        }

        float expMax = level * 10;

        level_img.fillAmount = exp / expMax;  // 추후 레벨별 경험치 수정
        level_txt.text = level.ToString();

        Debug.Log("exp : " + exp);
        Debug.Log("fill : " + exp / expMax);
    }
}

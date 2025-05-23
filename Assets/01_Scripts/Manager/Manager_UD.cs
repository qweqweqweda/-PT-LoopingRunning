using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_UD : Singleton<Manager_UD>
{
    List<UserData> userDatas = new List<UserData>();    // 사용하는 UD

    // 사용할 UD
    public UD_Level level_ud = new UD_Level();


    public void Init()
    {   // 사용하는 UD는 전부다 userDatas에 저장
        userDatas.Add(level_ud = new UD_Level());


        for (int i = 0; i < userDatas.Count; i++)
            userDatas[i].Initialize();  // 저장되어있는 UD 전부다 초기화
    }

    public void LoadData()
    {
        string mainID = "player";   // 임시로 저장된  MainID

        try
        {
            Init();

            for (int i = 0; i < userDatas.Count; i++)
            {
                userDatas[i].LoadData(mainID);  // 해당 MainID로 저장된 모든 UD의 데이터를 Load
            }
        }
        catch (Exception e)
        {
            Debug.Log("데이터 로드 에러 : " + e.Message);
            Init();
        }
    }

    public void SaveData()
    {
        for (int i = 0; i < userDatas.Count; i++)
            userDatas[i].SaveData();
        PlayerPrefs.Save();                     // 저장된 모든 UD의 데이터를 저장
    }
}

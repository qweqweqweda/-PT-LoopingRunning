using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public abstract class UserData
{
    public abstract Hashtable GetHashtable();               // 해당 UD의 데이터가 담긴 HashTable
    public abstract void SetHashtable(Hashtable loadJson);  // 현재 PlayerPrefs에 담긴 HashTable


    public string mainID;

    const string MainIDKey = "KEY_MI";
    const string SavedTimeKey = "KEY_ST";


    public virtual void Initialize() { }


    public virtual void SaveData()  // UD데이터를 PlayerPrefs에 저장
    {
        mainID = "player";
        Hashtable ht = GetHashtable();
        ht[MainIDKey] = mainID;

        string json = JsonConvert.SerializeObject(ht); // HashTable을 Json으로 변환

        PlayerPrefs.SetString(GetSaveKey(mainID), json);    // PlayerPrefs에 Key값 MainID + 현재 UD Value값 압축된 Json을 넣음
    }

    public virtual void LoadData(string tmpMainID = "player")   // PlayerPrefs에 저장된 데이터를 가져옴
    {
        string saveKey = GetSaveKey(tmpMainID);
        // Debug.Log(saveKey);
        try
        {
            string savedString = PlayerPrefs.GetString(saveKey);

            // JSON 문자열 → Dictionary<string, object>로 역직렬화
            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(savedString);

            Hashtable loadedTable = new Hashtable(dict);  // Json을 HashTable로 변환

            SetHashtable(loadedTable);  //  해당 UD에 저장된 데이터의 HashTable에 Key값을 통해 value를 UD에 저장하여 사용
        }
        catch (Exception e)
        {
            Debug.Log("데이터 로드 에러 : " + saveKey);
            throw e;
        }
    }


    public string GetTypeString()   // 어떤 UD인지
    {
        return GetType().ToString();
    }
    public string GetSaveKey(string tmpMainID)  // MainID + 어떤타입의 UD
    {
        return GetSaveKey(tmpMainID, GetTypeString());
    }
    public string GetSaveKey(string tmpMainID, string userDataType) //  MainID_어떤타입의UD 예) UD_Level 이면 MainID_UD_Level
    {
        return tmpMainID + "_" + userDataType;
    }
}
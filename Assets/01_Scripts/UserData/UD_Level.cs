using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UD_Level : UserData
{
    public int level;
    public int exp;

    public void LevelUP(int levelCount, int expCount) // 레벨업
    {
        level += levelCount;
        exp -= expCount;

        SaveData(); // 데이터 저장
    }


    public override void Initialize()
    {
        level = 0;
        exp = 0;
    }

    public override Hashtable GetHashtable()
    {
        Hashtable ht = new Hashtable();

        ht["L"] = level;    // L 은 level의 HashTable Key값
        ht["E"] = exp;

        return ht;
    }

    public override void SetHashtable(Hashtable loadJson)
    {
        if (loadJson["L"] != null)
            level = Convert.ToInt32(loadJson["L"]); // L의 value값을 level에 저장하여 사용
        if (loadJson["E"] != null)
            exp = Convert.ToInt32(loadJson["E"]);
    }
}

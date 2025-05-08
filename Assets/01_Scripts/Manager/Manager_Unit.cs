using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Unit : Singleton<Manager_Unit>
{
    public List<UnitBase> unitBases = new List<UnitBase>(); // 현재 spawn되어있는 모든 유닛

    public void Init()
    {
    }

    public void Clear()
    {
        for (int i = 0; i < unitBases.Count; i++)
        {
            if (unitBases[i].onUse)
                unitBases[i].Off();
        }
        unitBases.Clear();
    }

    public void Clear_WithoutHero()  // monster 유닛만 pooler에 넣기 + 유닛 리스트에서 삭제
    {
        for (int i = unitBases.Count - 1; i >= 0; i--)
        {
            if (unitBases[i].teamIndex != 0)
            {
                if (unitBases[i].onUse)
                {
                    unitBases[i].Off();
                    unitBases.RemoveAt(i);
                }
            }
        }
    }

    public UnitBase SpawnHero()
    {
        UnitStats unitStats = CV_UnitData.GetPlayerStats();

        SpawnPosData spawnPosData = new SpawnPosData();
        spawnPosData.xPos_Left = -100;
        spawnPosData.xPos_Right = -100;
        spawnPosData.yPos_Top = Mathf.Lerp(CV_Play.map_PosX_Top, CV_Play.map_PosY_Botton, 0.5f);    // 맵의 중간
        spawnPosData.yPos_Bottom = spawnPosData.yPos_Top;

        unitStats.spawnPosData = spawnPosData;

        return SpawnUnitBase(0, unitStats);
    }

    public void SpawnMonster()
    {
        float spawnPosX = 1000;

        UnitStats unitStats = CV_UnitData.GetMonsterStats();

        SpawnPosData spawnPosData = new SpawnPosData();
        spawnPosData.xPos_Left = spawnPosX - 50;
        spawnPosData.xPos_Right = spawnPosX + 50;
        spawnPosData.yPos_Top = CV_Play.map_PosX_Top;
        spawnPosData.yPos_Bottom = CV_Play.map_PosY_Botton;

        unitStats.spawnPosData = spawnPosData;
        SpawnUnitBase(1, unitStats);
    }

    UnitBase SpawnUnitBase(int teamIndex, UnitStats unitStats) // 유닛 스폰
    {
        UnitBase unitBase = Manager_Pooling.Instance.GetUnitBase(unitStats, unitStats.spawnPosData.GetRandomPos(), Quaternion.identity);

        unitBases.Add(unitBase);
        unitBase.Init(teamIndex, unitStats);
        return unitBase;
    }

    public UnitBase GetNearestUnitBase(UnitBase unitBase_My, float range)
    {
        UnitBase unitBase_target = null;
        float minDistance = 0;

        if (unitBases.Count > 0)
        {
            int teamIndex_My = unitBase_My.teamIndex;

            Vector3 pos_My = unitBase_My.transform.position;

            for (int i = 0; i < unitBases.Count; i++)
            {
                if (!unitBases[i].IsDead())
                {
                    if (unitBases[i].teamIndex != teamIndex_My && unitBases[i].teamIndex != -1)
                    {
                        float distance = CV_Play.GetDistance(pos_My, unitBases[i].transform.position);

                        if (range <= 0 || distance <= range)
                        {
                            if (unitBase_target == null || distance < minDistance)
                            {
                                unitBase_target = unitBases[i];
                                minDistance = distance;
                            }
                        }
                    }
                }
            }
        }

        return unitBase_target;
    }
}

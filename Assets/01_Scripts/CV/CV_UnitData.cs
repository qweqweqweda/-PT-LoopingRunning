using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CV_UnitData
{
    public static UnitDatas GetPlayerStats()
    {
        UnitDatas unitStats = new UnitDatas();

        unitStats.unitType = UnitType.Player;

        unitStats.attack = 1;     // 스탯 관련부분은 전부다 액셀로 정리 및 데이터 받아오기
        unitStats.attackRange = 10;
        unitStats.attackSpeed = 1;

        unitStats.maxHealth = 100;

        unitStats.moveSpeed = 300;

        unitStats.detectRange = 1000;

        unitStats.unitRadius = 10;

        unitStats.fileName = "player";
        unitStats.spriteName = "sprite_player";

        return unitStats;
    }

    public static UnitDatas GetMonsterStats()
    {
        UnitDatas unitStats = new UnitDatas();

        unitStats.unitType = UnitType.Monster;

        unitStats.attack = 1;
        unitStats.attackRange = 0;
        unitStats.attackSpeed = 2;
        unitStats.maxHealth = 50;
        unitStats.moveSpeed = 200;

        unitStats.detectRange = 300;

        unitStats.unitRadius = 10;

        unitStats.fileName = "monster";
        unitStats.spriteName = "sprite_monster";

        return unitStats;
    }
}
public class UnitDatas
{
    public UnitType unitType;
    public UnitAttackType unitAttackType;
    public SpawnPosData spawnPosData;

    public float attack;
    public float attackRange;
    public float attackSpeed;

    public float maxHealth;
    public float health;

    public float moveSpeed;

    public float unitRadius;

    public float detectRange;

    public string fileName;
    public string spriteName;
}
public class SpawnPosData
{
    public float xPos_Left;
    public float xPos_Right;

    public float yPos_Top;
    public float yPos_Bottom;

    public Vector3 GetRandomPos()
    {
        Vector3 pos = Vector3.zero;

        pos.x = Random.Range(xPos_Left, xPos_Right);
        pos.y = Random.Range(yPos_Top, yPos_Bottom);
        pos.z = CV_Play.GetConvertedPosZ(pos.y);

        return pos;
    }
}
public enum UnitType
{
    Player,
    Monster
}
public enum UnitAttackType
{
    Melee,
    Range
}
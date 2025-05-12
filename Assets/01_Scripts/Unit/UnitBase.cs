using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : PoolingObject
{
    public UnitDatas unitStats;
    public UnitBaseState unitBaseState;
    public SpriteRenderer unit_Sprite;

    public int teamIndex;

    public void Init(int _teamIndex, UnitDatas _unitStats)
    {
        teamIndex = _teamIndex;
        unitStats = _unitStats;
        unitStats.health = unitStats.maxHealth;

        if (unit_Sprite == null)
        {
            GameObject GO = new GameObject("sprite");
            SpriteRenderer spriteRenderer = GO.AddComponent<SpriteRenderer>();
            GO.transform.SetParent(transform);
            GO.transform.localPosition = Vector3.zero;
            GO.transform.localEulerAngles = Vector3.zero;
            GO.transform.localScale = new Vector3(100, 100, 100);
            spriteRenderer.sprite = Resources.Load(unitStats.spriteName, typeof(Sprite)) as Sprite;

            unit_Sprite = spriteRenderer;
        }
    }

    public void MovePosition(Vector3 pos)
    {
        Vector3 curPos = transform.position;

        curPos.z = 0;
        pos.z = 0;

        curPos = Vector3.MoveTowards(curPos, pos, GetMoveSpeed() * Time.deltaTime);
        SetPosition(curPos);
    }
    public void MoveDirection(Vector3 dir)
    {
        Vector3 TmpPos = transform.position;
        TmpPos += dir.normalized * GetMoveSpeed() * Time.deltaTime;
        SetPosition(TmpPos);
    }

    public void SetPosition(Vector3 pos)
    {
        pos.z = CV_Play.GetConvertedPosZ(pos.y);
        transform.position = pos;
    }

    float GetMoveSpeed()
    {
        return unitStats.moveSpeed;
    }

    public bool IsDead()
    {
        if (unitBaseState == UnitBaseState.Die)
            return true;
        else
            return false;
    }


    public void OnIdle()
    {
        if (unitBaseState != UnitBaseState.Idle)
            unitBaseState = UnitBaseState.Idle;
    }
    public void OnMove()
    {
        if (unitBaseState != UnitBaseState.Move)
            unitBaseState = UnitBaseState.Move;
    }
    public void OnAttackReady()
    {
        if (unitBaseState != UnitBaseState.AttackReady)
            unitBaseState = UnitBaseState.AttackReady;
    }
    public void OnAttack()
    {
        if (unitBaseState != UnitBaseState.Attack)
            unitBaseState = UnitBaseState.Attack;
    }
    public void OnDie()
    {
        if (unitBaseState != UnitBaseState.Die)
            unitBaseState = UnitBaseState.Die;
    }
}
public enum UnitBaseState
{
    Wait = 0,   // 게임 시작 전 단계
    Idle = 1,
    Move = 2,
    AttackReady = 3,
    Attack = 4,
    Die = 5,
}
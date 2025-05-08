using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : PoolingObject
{
    public UnitStats playerStats;
    public UnitBaseState unitBaseState;

    public int teamIndex;

    public void Init(int _teamIndex, UnitStats _unitStats)
    {
        teamIndex = _teamIndex;
        playerStats = _unitStats;
        playerStats.health = playerStats.maxHealth;
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
        return playerStats.moveSpeed;
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
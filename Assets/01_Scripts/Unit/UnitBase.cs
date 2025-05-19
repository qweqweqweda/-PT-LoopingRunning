using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : PoolingObject
{
    public bool oninvincibility;

    public UnitDatas unitDatas;
    public UnitBaseState unitBaseState;
    public SpriteRenderer unit_Sprite;

    public UnitBase unitBase_target;

    public int teamIndex; // 우리팀인지 체크 적팀이 여러팀으로 나누어져있을 경우를 대비

    public bool onBattle;
    public bool isWalk;
    public bool isControlled;

    public int stateAttack;

    public float deltaTime_AttackReady;
    public float deltaTime_Attack;
    public float hitTime_Attack;    // 데미지 넣는 시간 (원거리는 투사체 생성 시간)
    public float endTime_Attack;    // 공격 종료 시간간

    public int attackCount;

    public void Init(int _teamIndex, UnitDatas _unitStats)
    {
        teamIndex = _teamIndex;
        unitDatas = _unitStats;
        unitDatas.health = unitDatas.maxHealth;

        if (unit_Sprite == null)
        {
            GameObject GO = new GameObject("sprite");
            SpriteRenderer spriteRenderer = GO.AddComponent<SpriteRenderer>();
            GO.transform.SetParent(transform);
            GO.transform.localPosition = Vector3.zero;
            GO.transform.localEulerAngles = Vector3.zero;
            GO.transform.localScale = new Vector3(100, 100, 100);
            spriteRenderer.sprite = Resources.Load(unitDatas.spriteName, typeof(Sprite)) as Sprite;

            unit_Sprite = spriteRenderer;
        }

        unitBaseState = UnitBaseState.Idle;
    }

    public virtual void DoAction()
    {
        if (onBattle)
        {
            DoActionInBattle();
        }
        else
        {
            DoActionInNonBattle();
        }
    }
    protected virtual void DoActionInBattle() { }
    protected virtual void DoActionInNonBattle() { }

    public void MovePosition(Vector3 pos, bool onLookAtTarget)
    {
        Vector3 curPos = transform.position;

        curPos.z = 0;
        pos.z = 0;

        curPos = Vector3.MoveTowards(curPos, pos, GetMoveSpeed() * Time.deltaTime);
        SetPosition(curPos);

        if (onLookAtTarget)
            LookAtTarget(pos);
    }
    public void MoveDirection(Vector3 dir)
    {
        Vector3 TmpPos = transform.position;
        TmpPos += dir.normalized * GetMoveSpeed() * Time.deltaTime;
        SetPosition(TmpPos);

        if (dir.x > 0)
            SetRotation(true);
        else
            SetRotation(false);
    }

    public void SetPosition(Vector3 pos)
    {
        pos.z = CV_Play.GetConvertedPosZ(pos.y);
        transform.position = pos;
    }

    public void LookAtTarget(Vector3 posTarget)
    {
        if (posTarget.x >= transform.position.x)
            SetRotation(true);
        else
            SetRotation(false);
    }

    public void SetRotation(bool isRight)   // Hero랑 Monster랑 기본 스파인이 바라보는 방향이 다름
    {
        if (isRight)
            unit_Sprite.transform.localEulerAngles = new Vector3(0, 0, 0);
        else
            unit_Sprite.transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    float GetMoveSpeed()
    {
        return unitDatas.moveSpeed;
    }

    public bool IsInAttackRange(UnitBase unitBase_target)
    {
        float distance = CV_Play.GetDistance(transform.position, unitDatas.unitRadius,
                                        unitBase_target.transform.position, unitBase_target.unitDatas.unitRadius);

        if (distance <= unitDatas.attackRange)
            return true;

        else
            return false;
    }

    public bool IsDead()
    {
        if (unitBaseState == UnitBaseState.Die)
            return true;
        else
            return false;
    }

    public void OnDamaged(float damage, UnitBase unitBase_Target)
    {
        if (IsDead() || oninvincibility)
            return;

        unitDatas.health -= damage;
        OnDamagedListener(damage, unitBase_Target);

        if (unitDatas.health <= 0)
        {
            OnDie();
        }
    }

    public void OnIdle()
    {
        if (unitBaseState != UnitBaseState.Idle)
            unitBaseState = UnitBaseState.Idle;

        Debug.Log("idle");
    }
    public void OnMove()
    {
        if (unitBaseState != UnitBaseState.Move)
            unitBaseState = UnitBaseState.Move;

        Debug.Log("move");
    }
    public void OnAttackReady()
    {
        if (unitBaseState != UnitBaseState.AttackReady)
            unitBaseState = UnitBaseState.AttackReady;

        Debug.Log("attacReady");
    }
    public void OnAttack()
    {
        if (unitBaseState != UnitBaseState.Attack)
            unitBaseState = UnitBaseState.Attack;

        Debug.Log("attack");
    }
    public void OnDie()
    {
        if (unitBaseState != UnitBaseState.Die)
            unitBaseState = UnitBaseState.Die;
    }
    protected virtual void OnDamagedListener(float damage, UnitBase unitBase_Target) { }
    protected virtual void DieListener() { }
    protected virtual void AttackListener() { }

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase_Battle : UnitBase
{
    float deltaTime_SearchTarget;

    protected void AI()
    {
        if (IsDead())
            return;

        deltaTime_AttackReady += Time.deltaTime;

        if (isControlled)
            return;

        if (onBattle)
        {
            if (unitBase_target != null)
            {
                if (unitBase_target.IsDead()) // 공격 대상이 죽으면 null처리
                    unitBase_target = null;

                if (unitBase_target == null)  // 공격 대상이 없으면 대상 탐색
                    SearchTarget();
            }

            if (unitBaseState == UnitBaseState.Move || unitBaseState == UnitBaseState.AttackReady)
            {
                // 추격중이거나 공격준비중일때 공격 대상이 사라지면 NonBattle
                if (unitBase_target == null)
                {
                    BeNonBattleState();
                    return;
                }

                Vector3 pos_Target = unitBase_target.transform.position;
                LookAtTarget(pos_Target);   // 공격 대상 바라보기

                if (unitBaseState == UnitBaseState.Move) // 추적상태일때
                {
                    MovePosition(pos_Target, false);

                    if (IsInAttackRange(unitBase_target)) // 공격사거리 안에 대상이 있을때
                    {
                        OnAttackReady();
                    }
                }
                else if (unitBaseState == UnitBaseState.AttackReady)
                {
                    if (IsInAttackRange(unitBase_target))
                    {
                        if (deltaTime_AttackReady >= unitDatas.attackSpeed)
                        {
                            OnAttack();
                        }
                    }
                    else
                    {
                        OnMove();
                    }
                }
            }
            else if (unitBaseState == UnitBaseState.Attack)
            {
                deltaTime_Attack += Time.deltaTime; // Attack 상태일때부터 시간추가

                if (stateAttack == 0) // 공격 전
                {
                    if (deltaTime_Attack >= hitTime_Attack) // 데미지 넣는 시간
                    {
                        if (unitBase_target != null)
                        {
                            if (!unitBase_target.IsDead())
                            {
                                // 대상이 있고, 해당 대상이 살아있는지 확인하고, 대상이 공격 사거리 안에 있는지 체크
                                if (IsInAttackRange(unitBase_target))
                                {
                                    if (unitDatas.unitAttackType == UnitAttackType.Melee)
                                    {
                                        unitBase_target.OnDamaged(unitDatas.attack, this);
                                    }
                                    else if (unitDatas.unitAttackType == UnitAttackType.Range)
                                    {
                                        // 공격 행동
                                    }
                                    AttackListener();
                                }
                            }
                        }
                        stateAttack = 1;
                        deltaTime_Attack = 0;
                    }
                }
                else if (stateAttack == 1)
                {
                    if (deltaTime_Attack >= endTime_Attack)
                    {
                        if (unitBase_target == null)
                        {
                            SearchTarget();
                        }

                        if (unitBase_target != null)
                        {
                            if (IsInAttackRange(unitBase_target))
                            {
                                OnAttackReady();
                            }
                            else
                            {
                                OnMove();
                            }
                        }
                        else
                        {
                            BeNonBattleState();
                        }
                    }
                }
            }
        }
        else // 비전투 상태일때
        {
            AINonBattle();
        }
    }


    protected void SearchRepeat()
    {
        deltaTime_SearchTarget += Time.deltaTime;
        if (deltaTime_SearchTarget >= 0.5f)
        {
            deltaTime_SearchTarget = 0;
            SearchTarget();

            if (unitBase_target != null)
            {
                BeBattleState();
            }
        }
    }

    protected void SearchTarget()
    {
        unitBase_target = Manager_Unit.Instance.GetNearestUnitBase(this, unitDatas.detectRange);
    }

    protected virtual void AINonBattle() { }

    public void BeBattleState(UnitBase target = null)
    {
        // 공격 대상이 없으면 대상찾고, 대상이 있으면 거리를 체크하고 공격거리 안이면 공격, 밖이면 이동
        onBattle = true;

        unitBase_target = target;
        if (unitBase_target == null)
        {
            SearchTarget();
        }

        if (unitBase_target != null)
        {
            if (IsInAttackRange(unitBase_target))
            {
                OnAttackReady();
            }
            else
            {
                OnMove();
            }
        }
        else
        {
            BeNonBattleState();
        }
    }

    public void BeNonBattleState()
    {
        onBattle = false;
        DoActionInNonBattle();
    }


    protected virtual void Fever() { }

    protected override void OnDamagedListener(float damage, UnitBase unitBase_Target)
    {
        if (!onBattle)  // 때린 UnitBase 받아와서 Target에 추가
        {
            BeBattleState(unitBase_Target);
        }
    }
}

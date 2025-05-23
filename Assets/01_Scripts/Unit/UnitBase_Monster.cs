using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase_Monster : UnitBase_Battle
{
    float deltaTime_Idle;
    float idleTime;

    public Vector3 pos_Target_Idle;

    void Update()
    {
        AI();
    }
    protected override void AINonBattle()
    {
        if (unitBaseState == UnitBaseState.Idle)
        {
            deltaTime_Idle += Time.deltaTime;
            if (deltaTime_Idle >= idleTime) // Idle 일때 일정 시간 지나면 다음 행동 결정정
            {
                DoActionInNonBattle();
            }
        }
        else if (unitBaseState == UnitBaseState.Move)
        {
            MovePosition(pos_Target_Idle, true);   // 랜덤 위치를 향해 이동

            Vector3 curPos = transform.position;

            if (curPos.x == pos_Target_Idle.x && curPos.y == pos_Target_Idle.y) // 랜덤 위치에 도착하면 다음 행동 결정정
            {
                DoActionInNonBattle();
            }
        }
        SearchRepeat(); // 얼정 시간마다 적 탐색
    }

    protected override void DieListener()
    {
        Manager_UD.Instance.level_ud.exp += 1;  // 추후 경험치량 따로 계산해서 추가
        Manager_UI.Instance.Set_UI_Level();
    }
}

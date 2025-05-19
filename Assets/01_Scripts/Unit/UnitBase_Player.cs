using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase_Player : UnitBase_Battle
{
    void Update()
    {
        AI();
    }

    protected override void AINonBattle()
    {
        SearchRepeat();
    }

    protected override void DoActionInBattle()
    {
    }

    protected override void DoActionInNonBattle()
    {
        OnIdle();
    }
}

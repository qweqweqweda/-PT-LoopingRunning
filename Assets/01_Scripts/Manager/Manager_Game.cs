using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Game : Singleton<Manager_Game>
{
    public StageData stageData;
    public GameState gameState;


    public void Init()
    {
        GameStart();
    }

    public void GameStart()
    {
        gameState = GameState.Wait;

        SetStageData();

        Manager_Background.Instance.SetBackground(stageData.stage_Background_Name);
        SpawnPlayer();

        gameState = GameState.Play;
    }

    public void SetStageData()
    {
        StageData _stageData = new StageData();

        _stageData.stageLevel = 0;
        _stageData.stageType = StageType.Normal;
        _stageData.stage_Background_Name = "background";

        stageData = _stageData;
    }

    public void SpawnPlayer()
    {
        UnitBase player = Manager_Unit.Instance.SpawnHero();
        Manager_Controller.Instance.unit_Target = player;
    }
}
public enum GameState
{
    Wait,
    Play,
    End
}
public class StageData
{
    public int stageLevel;
    public StageType stageType;
    public string stage_Background_Name;
}

public enum StageType
{
    Normal,
    Boss
}
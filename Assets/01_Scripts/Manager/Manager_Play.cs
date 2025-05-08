using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Play : Singleton<Manager_Play>
{
    void Awake()
    {
        Manager_Background.Instance.Init();
        Manager_Game.Instance.Init();
        Manager_Controller.Instance.Init();
    }
}

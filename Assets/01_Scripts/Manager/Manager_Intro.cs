using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_Intro : Singleton<Manager_Intro>
{
    void Awake()
    {
        GoToPlay();

        Init();
    }


    void Init()
    {
        // Manager_UD.Instance.LoadData();
        // Manager_UD.Instance.SaveData();
    }

    public void GoToPlay()
    {
        StartCoroutine(GoToPlayInCoroutine());
    }

    IEnumerator GoToPlayInCoroutine()
    {
        AsyncOperation asyncOperation_UI = SceneManager.LoadSceneAsync("03_UI");

        asyncOperation_UI.allowSceneActivation = false;
        while (!asyncOperation_UI.isDone)
        {
            if (asyncOperation_UI.progress >= 0.9f)
                asyncOperation_UI.allowSceneActivation = true;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForFixedUpdate();

        // ****** Game Scene Load ******.
        AsyncOperation asyncOperation_Game = SceneManager.LoadSceneAsync("02_GAME", LoadSceneMode.Additive);
        while (!asyncOperation_Game.isDone)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}

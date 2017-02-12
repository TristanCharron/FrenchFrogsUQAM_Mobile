using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    private static bool isGameStarted = false;
    public static bool IsGameStarted { get { return isGameStarted; } }

    public delegate void GameplayEvent();
    public static event GameplayEvent OnBeginGame,OnEndGame,OnUpdateDuringGame;


    void Start()
    {
        BeginGame();
    }

    public static void BeginGame()
    {
        if(OnBeginGame != null)
        {
            OnBeginGame();
            isGameStarted = true;
            OnBeginGame = null;
        }
    }

    public static void EndGame()
    {
        if (OnEndGame != null)
        {
            OnEndGame();
            isGameStarted = false;
            OnEndGame = null;
        }
    }

    public void FixedUpdate()
    {
        if (OnUpdateDuringGame != null && isGameStarted)
            OnUpdateDuringGame();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject]
    private PieceController pieceController;

    [Inject]
    private PathManager pathManager;
    
    [Inject]
    private LevelManager levelManager;
    
    [Inject]
    private PlayerMovement playerMovement;
    
    [Inject]
    private AnimatorController animatorController;
    
    private bool isGame = true;

    private int clickCount;

    
    private void Update()
    {
        CheckInput();
    }

    private bool CanMove()
    {
        if (clickCount <= 0) return false;
        return true;
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanMove())
            {
                pieceController.OnClick();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartNewGame();

        }
    }

    public void StartNewGame()
    {
        pieceController.StartNewGame(pathManager.GetLastPlatform());

        levelManager.StartNewGame();
    
        playerMovement.StartNewGame();

        animatorController.StartNewGame();

    }

    public void GameFinished()
    {
        animatorController.GameFinished();
    }
    
    
    public void GameOver()
    {
        Debug.LogError("Game OVER");
        isGame = false;
    }

    public void SetClickCount(int finishCount)
    {
        clickCount = finishCount;
    }
    
    public void DecreaseClickCount()
    {
        clickCount--;
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
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

    [Inject]
    private CameraController cameraController;
    
    [Inject]
    private UIManager uıManager;
    
    
    [Inject]
    private CollectableManager collectableManager;
    
    
    private int clickCount;

    private bool isGameReadyForStart=false;

    private void Start()
    {
        SetTimeScale(1);
    }

    private void OnEnable()
    {
        LoadingManager.OnLoadingPanelClosed += StartTheGame;
    }

    private void OnDisable()
    {
        LoadingManager.OnLoadingPanelClosed -= StartTheGame;
    }

    private void StartTheGame()
    {
        SetIsGameReadyForStart(true);

        collectableManager.RandomCollectibles();

    }

    private bool GetIsGameReadyForStart()
    {
        return isGameReadyForStart;
    }

    private void SetIsGameReadyForStart(bool s)
    {
        isGameReadyForStart=s;
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanMove() && GetIsGameReadyForStart())
            {
                pieceController.OnClick();
            }
            
        }
    }
    private bool CanMove()
    {
        if (clickCount <= 0) return false;
        return true;
    }

    

    public void StartNewGame()
    {
        pieceController.StartNewGame(pathManager.GetLastPlatform());

        levelManager.StartNewGame();
    
        playerMovement.StartNewGame();

        animatorController.StartNewGame();
        
        cameraController.StartNewGame();

        uıManager.StartNewGame();
        
        collectableManager.RandomCollectibles();

    }

    public void GameFinished()
    {
        pieceController.GameFinished();
        
        pathManager.GameFinished();

        playerMovement.GameFinished();
        
        animatorController.GameFinished();

        cameraController.GameFinished();
        
        
        uıManager.GameFinished();

    }
    
    
    public void GameOver()
    {
        uıManager.GameOver();
        SetTimeScale(0);
        
        SetIsGameReadyForStart(false);
    }

    public void SetClickCount(int finishCount)
    {
        clickCount = finishCount;
    }
    
    public void DecreaseClickCount()
    {
        clickCount--;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void SetTimeScale(int i)
    {
        Time.timeScale = i;
    }
    
}

using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTextUI;

    public GameObject gameFinishedPanel;
    public GameObject gameOverPanel;

    public GameObject gamePanel;


    private void OnEnable()
    {
        LoadingManager.OnLoadingPanelClosed += EnableGamePanel;
    }

    private void OnDisable()
    {
        LoadingManager.OnLoadingPanelClosed -= EnableGamePanel;
    }
    private void EnableGamePanel()
    {
        gamePanel.SetActive(true);
    }
    

    public void StartNewGame()
    {
        gameFinishedPanel.SetActive(false);
    }
    
    public void GameFinished()
    {
        gameFinishedPanel.SetActive(true);
        gameOverPanel.SetActive(false); 
    }
    
    
    public void GameOver()
    {
        gameFinishedPanel.SetActive(false);
        gameOverPanel.SetActive(true); 
    }
    

    public void UpdateScoreText(int score)
    {
        scoreTextUI.text = $"Score: {score}";
    }
}
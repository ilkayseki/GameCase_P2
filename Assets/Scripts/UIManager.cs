using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameFinishedPanel;
    public GameObject gameOverPanel;

    
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
    
    
}
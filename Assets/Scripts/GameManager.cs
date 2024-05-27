using UnityEngine;
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
    
    private int clickCount;

    private void Start()
    {
        SetTimeScale(1);
    }

    private void Update()
    {
        CheckInput();
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

    private void SetTimeScale(int i)
    {
        Time.timeScale = i;
    }
    
}

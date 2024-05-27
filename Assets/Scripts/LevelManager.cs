using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject]
    private PieceController pieceController;
    
    [Inject]
    private ObjectPoolManager objectPoolManager;
    
    [Inject]
    private GameManager gameManager;
    
    private int levelCount;
    
    private int finishCount;
    
    private Transform pieceControllerTransform;

    private Transform finishPrefab;
    
    public LevelData[] levels;
    
    void Awake()
    {
        LoadAllLevelData();
        CreateLevel();
        SetCounts();
    }

    private void CreateLevel()
    {
        pieceControllerTransform = pieceController.GetPieceControllerTransform();
        
        finishPrefab = objectPoolManager.GetFinishPlatform();
        
        float pieceControllerSizeZ = pieceControllerTransform.localScale.z;
        
        Vector3 finishBounds = finishPrefab.GetComponent<MeshRenderer>().bounds.size;
        float finishDepth = finishBounds.z;

        Vector3 newPosition = new Vector3(
            pieceControllerTransform.position.x,
            pieceControllerTransform.position.y + (finishBounds.y / 2),
            pieceControllerTransform.position.z +
            (pieceControllerSizeZ * (finishCount - 1)) + // finishCount kadar ileride
            (pieceControllerSizeZ / 2) + // küpün yarısı kadar daha
            (finishDepth / 2) // finishPrefab'in yarısı kadar daha
        );

        finishPrefab.transform.position = newPosition;
    }

    void LoadAllLevelData()
    {
        levels = Resources.LoadAll<LevelData>("Scriptable");

        levelCount = 0;

        if (levels.Length > 0)
        {
            finishCount = levels[levelCount].finishCount;
        }
        else
        {
            Debug.LogError("No level data found in Resources/Scriptable.");
        }
    }

    private void SetCounts()
    {
        gameManager.SetClickCount(finishCount);
        pieceController.SetMoveCount(finishCount);
    }

    public void StartNewGame()
    {
        IncreaseLevelCount();
        LoadLevelData();
        CreateLevel();
        SetCounts();
    }

    private void IncreaseLevelCount()
    {
        levelCount++;
        if (levelCount >= levels.Length)
        {
            levelCount = 0;
        }
    }

    void LoadLevelData()
    {
        levels = Resources.LoadAll<LevelData>("Scriptable");

        if (levels.Length > 0)
        {
            if (levelCount >= levels.Length) levelCount = 0;
            finishCount = levels[levelCount].finishCount;
        }
        else
        {
            Debug.LogError("No level data found in Resources/Scriptable.");
        }
    }
}

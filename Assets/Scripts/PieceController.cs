using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PieceController : MonoBehaviour
{
    [Inject] private ColorController colorController;
    [Inject] private ObjectPoolManager objectPoolManager;
    [Inject] private PathManager pathManager;
    [Inject] private GameManager gameManager;

    [SerializeField] private Transform reference;
    [SerializeField] private Transform last;
    [SerializeField] [Range(1, 5)] private float speed;
    [SerializeField] [Range(1, 2)] private float limit;
    [SerializeField] [Range(0, 0.8f)] private float endTolerance;
    [SerializeField] [Range(0, 0.8f)] private float tolerance;

    

    #region Variables

    private Vector3 position;
    private int direction;
    private float move;

    private Vector3 distance;
    private Transform stand;
    private float previousXPosition;
    private Vector3 newPosition;
    private bool isFirstFalling;

    private Transform falling;

    private Vector3 fallingSize;
    private Vector3 standSize;

    private Vector3 fallingPosition;
    private int fallingMultiply;

    private Vector3 standPosition;
    private int standMultiply;

    private Vector3 extents;
    private float originLocalScaleX;
    private float distanceAbsX;
    
    private bool _isForward = true;
    private bool _isStop;
    
    private int moveCount;
    
    #endregion

    private void FixedUpdate()
    {
        if (_isStop || !canMove()) return;
        MovePiece();
    }

    public void StartNewGame(GameObject finishPrefab)
    {
        IsStopHandler(true);

        float Depth = last.transform.localScale.z;

        Vector3 finishBounds = finishPrefab.GetComponent<MeshRenderer>().bounds.size;
        float finishDepth = finishBounds.z;

        Vector3 newPosition = new Vector3(
            last.transform.position.x,
            last.transform.position.y,
            last.transform.position.z + (Depth * 1) + (finishDepth)
        );

        transform.position = newPosition;
        last = finishPrefab.transform;

        Debug.LogError("StartNewGame");
        
        IsStopHandler(false);
        
    }

    private void MovePiece()
    {
        position = transform.position;
        
        direction = _isForward ? 1 : -1;
        move = speed * Time.deltaTime * direction;
        position = UpdatePosition(position, move);
        
        float minXLimit = last.position.x - limit;
        float maxXLimit = last.position.x + limit;
    
        if (position.x < minXLimit)
        {
            position.x = minXLimit;
            _isForward = true;
        }
        else if (position.x > maxXLimit)
        {
            position.x = maxXLimit;
            _isForward = false;
        }
    
        transform.position = position;
    }

    private Vector3 UpdatePosition(Vector3 position, float move)
    {
        position.x += move;
        return position;
    }

    public void OnClick()
    {
        IsStopHandler(true);
        
        IncreaseMoveCount();

        if (IsFail())
        {
            Debug.LogError("Yandın");
            return;
        }
        
        SplitCube();
        
        ChangeColors();

        AddPlatformToObjectPool();
        
        SetNewPlatform();

        IsStopHandler(false);
    }

    private void IncreaseMoveCount()
    {
        moveCount--;
    }

    private void AddPlatformToObjectPool()
    {
        pathManager.AddPlatform(stand.transform);
    }

    private void SetNewPlatform()
    {
        if (canMove())
        {
            MoveToNextPosition();
            colorController.SetNewColor();
        }
        else
        {
            MoveToLastPosition();
        }

        gameManager.DecreaseClickCount();
    }

    void SplitCube()
    {
        Vector3 purpleCubePosition = last.transform.position;
        Vector3 purpleCubeSize = last.transform.localScale;

        Vector3 yellowCubePosition = transform.position;
        Vector3 yellowCubeSize = transform.localScale;

        float yellowCubeRightEdge = yellowCubePosition.x + (yellowCubeSize.x / 2);
        float yellowCubeLeftEdge = yellowCubePosition.x - (yellowCubeSize.x / 2);
        float purpleCubeRightEdge = purpleCubePosition.x + (purpleCubeSize.x / 2);
        float purpleCubeLeftEdge = purpleCubePosition.x - (purpleCubeSize.x / 2);

        if (IsWithinTolerance(yellowCubeRightEdge, yellowCubeLeftEdge, purpleCubeRightEdge, purpleCubeLeftEdge))
        {
            Debug.LogError("IsWithinTolerance");
            HandleWithinTolerance(yellowCubeSize, yellowCubePosition, purpleCubePosition.x);
        }
        else if (IsCoveringCompletely(yellowCubeRightEdge, yellowCubeLeftEdge, purpleCubeRightEdge, purpleCubeLeftEdge))
        {
            Debug.LogError("IsCoveringCompletely");

            HandleCompleteCover(yellowCubeSize, purpleCubeSize, purpleCubePosition, yellowCubePosition);
        }
        else if (yellowCubeRightEdge > purpleCubeRightEdge)
        {
            Debug.LogError("yellowCubeRightEdge > purpleCubeRightEdge");

            HandleRightOverlap(yellowCubeRightEdge, yellowCubeLeftEdge, purpleCubeRightEdge, yellowCubeSize, yellowCubePosition);
        }
        else if (yellowCubeLeftEdge < purpleCubeLeftEdge)
        {
            Debug.LogError("yellowCubeLeftEdge < purpleCubeLeftEdge");

            HandleLeftOverlap(yellowCubeLeftEdge, yellowCubeRightEdge, purpleCubeLeftEdge, yellowCubeSize, yellowCubePosition);
        }
        else
        {
            Debug.LogError("Else");

            HandlePartialOverlap(yellowCubeSize, yellowCubePosition);
        }
    }

    bool IsWithinTolerance(float yellowRight, float yellowLeft, float purpleRight, float purpleLeft)
    {
        return Mathf.Abs(yellowRight - purpleRight) <= tolerance || Mathf.Abs(yellowLeft - purpleLeft) <= tolerance;
    }

    bool IsCoveringCompletely(float yellowRight, float yellowLeft, float purpleRight, float purpleLeft)
    {
        return yellowRight > purpleRight && yellowLeft < purpleLeft;
    }

    void HandleWithinTolerance(Vector3 yellowSize, Vector3 yellowPosition, float purpleX)
    {
        CreateStand(yellowSize.x, yellowSize, new Vector3(purpleX,yellowPosition.y,yellowPosition.z));
    }

    void HandleCompleteCover(Vector3 yellowSize, Vector3 purpleSize, Vector3 purplePosition, Vector3 yellowPosition)
    {
        float standSizeX = purpleSize.x;
        float fallingSizeX = yellowSize.x - purpleSize.x;

        Vector3 standPosition = purplePosition;
        Vector3 fallingPosition = yellowPosition.x > purplePosition.x
            ? new Vector3(yellowPosition.x + (fallingSizeX / 2), yellowPosition.y, yellowPosition.z)
            : new Vector3(yellowPosition.x - (fallingSizeX / 2), yellowPosition.y, yellowPosition.z);

        CreateStand(standSizeX, yellowSize, standPosition);
        CreateFalling(fallingSizeX, yellowSize, fallingPosition);
    }

    void HandleRightOverlap(float yellowRight, float yellowLeft, float purpleRight, Vector3 yellowSize, Vector3 yellowPosition)
    {
        float standSizeX = purpleRight - yellowLeft;
        float fallingSizeX = yellowSize.x - standSizeX;

        Vector3 standPosition = new Vector3(yellowLeft + (standSizeX / 2), yellowPosition.y, yellowPosition.z);
        Vector3 fallingPosition = new Vector3(standPosition.x + (standSizeX / 2) + (fallingSizeX / 2), yellowPosition.y, yellowPosition.z);

        CreateStand(standSizeX, yellowSize, standPosition);
        CreateFalling(fallingSizeX, yellowSize, fallingPosition);
    }

    void HandleLeftOverlap(float yellowLeft, float yellowRight, float purpleLeft, Vector3 yellowSize, Vector3 yellowPosition)
    {
        float standSizeX = yellowRight - purpleLeft;
        float fallingSizeX = yellowSize.x - standSizeX;

        Vector3 standPosition = new Vector3(yellowRight - (standSizeX / 2), yellowPosition.y, yellowPosition.z);
        Vector3 fallingPosition = new Vector3(standPosition.x - (standSizeX / 2) - (fallingSizeX / 2), yellowPosition.y, yellowPosition.z);

        CreateStand(standSizeX, yellowSize, standPosition);
        CreateFalling(fallingSizeX, yellowSize, fallingPosition);
    }

    private void ChangeColors()
    {
        if(stand!=null)
        colorController.UpdateColor(stand);
        if(falling!=null)
        colorController.UpdateColor(falling);
    }
    

    void HandlePartialOverlap(Vector3 yellowSize, Vector3 yellowPosition)
    {
        CreateStand(yellowSize.x, yellowSize, yellowPosition);
    }

    void CreateStand(float sizeX, Vector3 originalSize, Vector3 position)
    {
        if (sizeX > 0)
        {
            stand = objectPoolManager.GetStandPiece();
            stand.position = position;
            stand.transform.localScale = new Vector3(sizeX, originalSize.y, originalSize.z);
            last = stand.transform;
        }
    }

    void CreateFalling(float sizeX, Vector3 originalSize, Vector3 position)
    {
        if (sizeX > 0)
        {
            falling = objectPoolManager.GetFallingPiece();
            falling.position = position;
            falling.transform.localScale = new Vector3(sizeX, originalSize.y, originalSize.z);
        }
    }
    
    private Vector3 CalculateDistance()
    {
        distance =  transform.position-last.position ;
        return distance;
    }

    private void IsStopHandler(bool status)
    {
        _isStop = status;
    }

    private void MoveToNextPosition()
    {
        newPosition = new Vector3(last.position.x + (Random.value > 0.5f ? limit : -limit), transform.position.y, transform.position.z);
        newPosition.z += reference.localScale.z;
        transform.position = newPosition;
        transform.localScale = last.localScale;
    }
    
    private void MoveToLastPosition()
    {
        transform.position = stand.position;
        transform.localScale = stand.localScale;
    }
    
    private bool IsFail()
    {

        if (transform.localScale.x < 0.01)
            return false;
        
        // Mor küpün bounds değerlerini al
        Bounds lastBounds = last.GetComponent<Renderer>().bounds;
        // Diğer küpün bounds değerlerini al
        Bounds currentBounds = GetComponent<Renderer>().bounds;

        // Küplerin X eksenindeki minimum ve maksimum değerlerini al
        float lastMinX = lastBounds.min.x;
        float lastMaxX = lastBounds.max.x;
        float currentMinX = currentBounds.min.x;
        float currentMaxX = currentBounds.max.x;

        Debug.LogError("lastMinX: " + lastMinX);
        Debug.LogError("lastMaxX: " + lastMaxX);
        Debug.LogError("currentMinX: " + currentMinX);
        Debug.LogError("currentMaxX: " + currentMaxX);

        // Küplerin çakışıp çakışmadığını kontrol et
        bool isOverlapping = (currentMaxX >= lastMinX && currentMinX <= lastMaxX);

        Debug.LogError("isOverlapping: " + isOverlapping);
    
        // Çakışma yoksa fail dön
        return !isOverlapping;
    }



    public Transform GetPieceControllerTransform()
    {
        return transform;
    }
    
    private bool canMove()
    {
        return moveCount > 0;
    }

    public void SetMoveCount(int move)
    {
        moveCount = move;
    }
}

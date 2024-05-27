using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

public class PieceController : MonoBehaviour
{
    [Inject] private ColorManager _colorManager;
    [Inject] private ObjectPoolManager objectPoolManager;
    [Inject] private PathManager pathManager;
    [Inject] private GameManager gameManager;
    [Inject] private MusicManager _musicManager;


    [SerializeField] private Transform reference;
    [SerializeField] private Transform last;
    [SerializeField] [Range(1, 5)] private float speed;
    [SerializeField] [Range(1, 2)] private float limitX;
    [SerializeField] [Range(0, 0.2f)] private float tolerance;

    
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
    private float Depth;
    private Vector3 finishBounds;
    private float finishDepth;
    private float minXLimit;
    private float maxXLimit;
    private Vector3 firstPosition;
    private Vector3 firstSize;
    private Vector3 secondPosition;
    private Vector3 secondSize;
    private float secondRightEdge;
    private float secondLeftEdge;
    private float firstRightEdge;
    private float firstLeftEdge;
    private float standSizeX;
    private float fallingSizeX;
    private Bounds lastBounds;
    private Bounds currentBounds;
    private bool isOverlapping;
    private float lastMinX;
    private float lastMaxX;
    private float currentMinX;
    private float currentMaxX;

    #endregion

    private void FixedUpdate()
    {
        if (GetIsStop() || !canMove()) return;
        MovePiece();
    }

    public void StartNewGame(GameObject finishPrefab)
    {
        IsStopHandler(true);

        Depth = last.transform.localScale.z;

        finishBounds = finishPrefab.GetComponent<MeshRenderer>().bounds.size;
        finishDepth = finishBounds.z;

        newPosition = new Vector3(
            last.transform.position.x,
            last.transform.position.y,
            last.transform.position.z + (Depth * 1) + (finishDepth)
        );

        transform.position = newPosition;
        last = finishPrefab.transform;

        // Debug.LogError("StartNewGame");
        
        IsStopHandler(false);
        
    }
    

    private void MovePiece()
    {
        position = transform.position;
        
        direction = _isForward ? 1 : -1;
        move = speed * Time.deltaTime * direction;
        position = UpdatePosition(position, move);
        
         minXLimit = last.position.x - limitX;
         maxXLimit = last.position.x + limitX;
    
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
        
        DecreaseClickCount();
        
        if (IsFail())
        {
            return;
        }
        
        SplitCube();
        
        ChangeColors();
        
        AddPlatformToObjectPool();
        
        SetNewPlatform();

        IsStopHandler(false);
    }

    private void DecreaseClickCount()
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
            _colorManager.SetNewColor();
        }
        else
        {
            MoveToLastPosition();
        }

        gameManager.DecreaseClickCount();
    }

    void SplitCube()
    {
        firstPosition = last.transform.position;
        firstSize = last.transform.localScale;

        secondPosition = transform.position;
        secondSize = transform.localScale;

        secondRightEdge = secondPosition.x + (secondSize.x / 2);
        secondLeftEdge = secondPosition.x - (secondSize.x / 2);
        firstRightEdge = firstPosition.x + (firstSize.x / 2);
        firstLeftEdge = firstPosition.x - (firstSize.x / 2);

        if (IsWithinTolerance(secondRightEdge, secondLeftEdge, firstRightEdge, firstLeftEdge))
        {
            HandleWithinTolerance(secondSize, secondPosition, firstPosition.x);

            _musicManager.PlayHighPitchMusic();

        }
        else if (IsCoveringCompletely(secondRightEdge, secondLeftEdge, firstRightEdge, firstLeftEdge))
        {
            // Debug.LogError("IsCoveringCompletely");
            _musicManager.PlayNormalMusic();
            HandleCompleteCover(secondSize, firstSize, firstPosition, secondPosition);
        }
        else if (secondRightEdge > firstRightEdge)
        {
            // Debug.LogError("yellowCubeRightEdge > purpleCubeRightEdge");
            _musicManager.PlayNormalMusic();

            HandleRightOverlap(secondRightEdge, secondLeftEdge, firstRightEdge, secondSize, secondPosition);
        }
        else if (secondLeftEdge < firstLeftEdge)
        {
            // Debug.LogError("yellowCubeLeftEdge < purpleCubeLeftEdge");
            _musicManager.PlayNormalMusic();

            HandleLeftOverlap(secondLeftEdge, secondRightEdge, firstLeftEdge, secondSize, secondPosition);
        }
        else
        {
            // Debug.LogError("Else");
            _musicManager.PlayNormalMusic();

            HandlePartialOverlap(secondSize, secondPosition);
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
         standSizeX = purpleSize.x;
         fallingSizeX = yellowSize.x - purpleSize.x;

        standPosition = purplePosition;
        fallingPosition = yellowPosition.x > purplePosition.x
            ? new Vector3(yellowPosition.x + (fallingSizeX / 2), yellowPosition.y, yellowPosition.z)
            : new Vector3(yellowPosition.x - (fallingSizeX / 2), yellowPosition.y, yellowPosition.z);

        CreateStand(standSizeX, yellowSize, standPosition);
        CreateFalling(fallingSizeX, yellowSize, fallingPosition);
    }

    void HandleRightOverlap(float yellowRight, float yellowLeft, float purpleRight, Vector3 yellowSize, Vector3 yellowPosition)
    {
        standSizeX = purpleRight - yellowLeft;
        fallingSizeX = yellowSize.x - standSizeX;

         standPosition = new Vector3(yellowLeft + (standSizeX / 2), yellowPosition.y, yellowPosition.z);
         fallingPosition = new Vector3(standPosition.x + (standSizeX / 2) + (fallingSizeX / 2), yellowPosition.y, yellowPosition.z);

        CreateStand(standSizeX, yellowSize, standPosition);
        CreateFalling(fallingSizeX, yellowSize, fallingPosition);
    }

    void HandleLeftOverlap(float yellowLeft, float yellowRight, float purpleLeft, Vector3 yellowSize, Vector3 yellowPosition)
    {
         standSizeX = yellowRight - purpleLeft;
         fallingSizeX = yellowSize.x - standSizeX;

        standPosition = new Vector3(yellowRight - (standSizeX / 2), yellowPosition.y, yellowPosition.z);
        fallingPosition = new Vector3(standPosition.x - (standSizeX / 2) - (fallingSizeX / 2), yellowPosition.y, yellowPosition.z);

        CreateStand(standSizeX, yellowSize, standPosition);
        CreateFalling(fallingSizeX, yellowSize, fallingPosition);
    }

    private void ChangeColors()
    {
        if(stand!=null)
        _colorManager.UpdateColor(stand);
        if(falling!=null)
        _colorManager.UpdateColor(falling);
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

    private void IsStopHandler(bool status)
    {
        _isStop = status;
    }

    private bool GetIsStop()
    {
        return _isStop;
    }

    private void MoveToNextPosition()
    {
        newPosition = new Vector3(last.position.x + (Random.value > 0.5f ? limitX : -limitX), transform.position.y, transform.position.z);
        newPosition.z += reference.localScale.z;
        transform.position = newPosition;
        transform.localScale = last.localScale;
    }
    
    private void MoveToLastPosition()
    {
        transform.position = stand.position;
        transform.localScale = stand.localScale;
    }
    
    public void GameFinished()
    {
        
        DecreaseClickCount();

        gameManager.DecreaseClickCount();

        last = transform;

    }
    
    private bool IsFail()
    {

        if (transform.localScale.x < 0.01)
            return false;
        
        
        lastBounds = last.GetComponent<Renderer>().bounds;
        
        currentBounds = GetComponent<Renderer>().bounds;

        
        lastMinX = lastBounds.min.x;
        lastMaxX = lastBounds.max.x;
        currentMinX = currentBounds.min.x;
        currentMaxX = currentBounds.max.x;

       
        isOverlapping = (currentMaxX >= lastMinX && currentMinX <= lastMaxX);

        
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

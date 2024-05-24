using System;
using UnityEngine;
using Zenject;

public enum Direction
{
    Left,
    Right,
    Front,
    Back
}

public class PieceController : MonoBehaviour
{
    [Inject]
    private GameManager gameManager;
    [Inject]
    private ColorController colorController;
    
    
    
    [SerializeField] private Transform reference;
    [SerializeField] private MeshRenderer referenceMesh;
    [SerializeField] private GameObject fallingPrefab;
    [SerializeField] private GameObject standPrefab;
    [SerializeField] private Transform last;
    [SerializeField] [Range(1, 5)] private float speed;
    [SerializeField] [Range(1, 2)] private float limit;
    [SerializeField] [Range(0, 1)] private float musicTolerance;
    [SerializeField] [Range(0, 1)] private float endTolerance;

    private bool _isForward = true;
    private bool _isAxisX = true;
    private bool _isStop;

    #region GarbageCollecttor

    private Vector3 position;
    private int direction;
    private float move;


    private Vector3 distance;
    private Transform stand;
    private float previousXPosition;
    private Vector3 newPosition;
    private bool isFirstFalling;
    
    private Transform falling;
    
    private Direction minDirection;
    private Direction maxDirection;
    
    
    private Vector3 fallingSize;
    private Vector3 standSize;

    private Vector3 fallingPosition;
    private int fallingMultiply;
    
    private Vector3 standPosition;
    private int standMultiply;
    
    private Vector3 extents;
    private float origin;
    private float current;


    #endregion
    
    
    private void LateUpdate()
    {
        if (_isStop) return;
        MovePiece();
    }

    private void MovePiece()
    {
        position = transform.position;
        direction = _isForward ? 1 : -1;
        move = speed * Time.deltaTime * direction;
        position = UpdatePosition(position, move);

        if (_isAxisX && (position.x < -limit || position.x > limit))
        {
            position.x = Mathf.Clamp(position.x, -limit, limit);
            _isForward = !_isForward;
        }

        transform.position = position;
    }

    private Vector3 UpdatePosition(Vector3 position, float move)
    {
        if (_isAxisX) position.x += move;
        return position;
    }

    
    public void OnClick()
    {
        SetIsStop(true);
        distance = last.position - transform.position;
        if (IsFail(distance))
        {
            gameManager.GameOver();
            return;
        }

        if (Math.Abs(last.position.x - transform.position.x) < musicTolerance)
        {
            HandlePerfectPlacement();
        }
        else
        {
            DivideObject(_isAxisX, _isAxisX ? distance.x : distance.z);
        }

        MoveToNextPosition();
        SetIsStop(false);
        gameManager.SetScore();
    }

    private void SetIsStop(bool statue)
    {
        _isStop = statue;
    }
    
    private void HandlePerfectPlacement()
    {
       
        stand = Instantiate(standPrefab).transform;
        stand.localScale = last.localScale;
        stand.position = new Vector3(last.transform.position.x, last.transform.position.y, last.transform.position.z + last.localScale.z);

        colorController.UpdateColor(stand);

        last = stand;
    }

    private void MoveToNextPosition()
    {
        previousXPosition = last.position.x;
        newPosition = new Vector3(previousXPosition, transform.position.y, transform.position.z);
        newPosition.z += reference.localScale.z;
        transform.position = newPosition;
        transform.localScale = last.localScale;
    }


 
    
    private void DivideObject(bool isAxisX, float value)
    {
        isFirstFalling = value > 0;
        falling = Instantiate(fallingPrefab).transform;
        stand = Instantiate(standPrefab).transform;

        UpdateSizes(isAxisX, value, falling, stand);

         minDirection = isAxisX ? Direction.Left : Direction.Back;
         maxDirection = isAxisX ? Direction.Right : Direction.Front;

        UpdatePositions(isAxisX, value, isFirstFalling, falling, stand, minDirection, maxDirection);
        
        colorController.UpdateColor(falling);
        colorController.UpdateColor(stand);

        last = stand;
    }

    private void UpdateSizes(bool isAxisX, float value, Transform falling, Transform stand)
    {
        fallingSize = reference.localScale;
        if (isAxisX) fallingSize.x = Math.Abs(value);
        else fallingSize.z = Math.Abs(value);
        falling.localScale = fallingSize;

        standSize = reference.localScale;
        if (isAxisX) standSize.x = reference.localScale.x - Math.Abs(value);
        else standSize.z = reference.localScale.z - Math.Abs(value);
        stand.localScale = standSize;
    }

    private void UpdatePositions(bool isAxisX, float value, bool isFirstFalling, Transform falling, Transform stand, Direction minDirection, Direction maxDirection)
    {
        fallingPosition = GetPositionEdge(referenceMesh, isFirstFalling ? minDirection : maxDirection);
        fallingMultiply = isFirstFalling ? 1 : -1;
        if (isAxisX) fallingPosition.x += (falling.localScale.x / 2) * fallingMultiply;
        else fallingPosition.z += (falling.localScale.z / 2) * fallingMultiply;
        falling.position = fallingPosition;

        standPosition = GetPositionEdge(referenceMesh, !isFirstFalling ? minDirection : maxDirection);
        standMultiply = !isFirstFalling ? 1 : -1;
        if (isAxisX) standPosition.x += (stand.localScale.x / 2) * standMultiply;
        else standPosition.z += (stand.localScale.z / 2) * standMultiply;
        stand.position = new Vector3(last.position.x, standPosition.y, standPosition.z);
    }
    
    private Vector3 GetPositionEdge(MeshRenderer mesh, Direction direction)
    {
        extents = mesh.bounds.extents;
        position = mesh.transform.position;

        switch (direction)
        {
            case Direction.Left:
                position.x += -extents.x;
                break;
            case Direction.Right:
                position.x += extents.x;
                break;
            case Direction.Front:
                position.z += extents.z;
                break;
            case Direction.Back:
                position.z += -extents.z;
                break;
        }

        return position;
    }

    private bool IsFail(Vector3 distance)
    {
        origin = _isAxisX ? transform.localScale.x : transform.localScale.z;
        current = _isAxisX ? Mathf.Abs(distance.x) : Mathf.Abs(distance.z);
        return current + endTolerance >= origin;
    }
}

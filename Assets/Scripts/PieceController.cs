using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PieceController : MonoBehaviour
{
    [Inject] private ColorController colorController;
    [Inject] private ObjectPoolManager objectPoolManager;
    [Inject] private PathManager pathManager;

    [SerializeField] private Transform reference;
    [SerializeField] private MeshRenderer referenceMesh;
    [SerializeField] private Transform last;
    [SerializeField] [Range(1, 5)] private float speed;
    [SerializeField] [Range(1, 2)] private float limit;
    
    [Tooltip("Müzik toleransı: Objelerin müzikle mükemmel bir şekilde yerleştirildiği kabul edilen tolerans seviyesi [Önerilen 0.059]")]
    [SerializeField] [Range(0, 0.8f)] private float musicTolerance;
    [Tooltip("Bitiş toleransı: Oyun objelerinin tamamen yerleştirildiği kabul edilen tolerans seviyesi [Önerilen 0.213 Bu değer müzik töleransından küçük olmamalıdır]")]
    [SerializeField] [Range(0, 0.8f)] private float endTolerance;

   
    
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
    
    private bool _isForward = true;
    private bool _isStop;


    #endregion

    private void FixedUpdate()
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
        
        // Başlangıç pozisyonu ve limitler arasında kontrol yap
        float minXLimit = last.position.x - limit;
        float maxXLimit = last.position.x + limit;
    
        if (position.x < minXLimit)
        {
            position.x = minXLimit;
            _isForward = true; // Sınıra ulaşıldığında ileriye doğru hareket et
        }
        else if (position.x > maxXLimit)
        {
            position.x = maxXLimit;
            _isForward = false; // Sınıra ulaşıldığında geriye doğru hareket et
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
        CalculateDistance();
        
        if (IsFail(CalculateDistance()))
        {
            //gameManager.GameOver();
            return;
        }

        if (Math.Abs(last.position.x - transform.position.x) < musicTolerance)
        {
            HandlePerfectPlacement();
        }
        else
        {
            DivideObject(CalculateDistance().x);
        }

        MoveToNextPosition();
        
        colorController.SetNewColor();

        pathManager.AddPlatform(stand.transform);
        
        IsStopHandler(false);
    }

    private Vector3 CalculateDistance()
    {
        return distance = last.position - transform.position;
    }

    private void IsStopHandler(bool status)
    {
        _isStop = status;
    }

    private void HandlePerfectPlacement()
    {
        stand = objectPoolManager.GetStandPiece();
        stand.localScale = last.localScale;
        stand.position = new Vector3(transform.position.x, last.transform.position.y, last.transform.position.z + last.localScale.z);

        colorController.UpdateColor(stand);

        last = stand;
    }

    private void MoveToNextPosition()
    {
        previousXPosition = last.position.x;
        newPosition = new Vector3(previousXPosition + (Random.value > 0.5f ? limit : -limit), transform.position.y, transform.position.z);
        newPosition.z += reference.localScale.z;
        transform.position = newPosition;
        transform.localScale = last.localScale;
    }

    public GameObject fallingPrefab;
    public GameObject standPrefab;
    private void DivideObject(float value)
    {
        isFirstFalling = value > 0;
    
        //falling = objectPoolManager.GetFallingPiece();
        //stand = objectPoolManager.GetStandPiece();

         falling = Instantiate(fallingPrefab).transform;
         stand = Instantiate(standPrefab).transform;
        
        UpdateSizes(value, falling, stand); // Boyutları güncelle

        minDirection = Direction.Left;
        maxDirection = Direction.Right;

        UpdatePositions( isFirstFalling, falling, stand, minDirection, maxDirection); // Pozisyonları güncelle

        colorController.UpdateColor(falling);
        colorController.UpdateColor(stand);

        last = stand;
    }



    private void UpdateSizes(float value, Transform falling, Transform stand)
    {
        fallingSize = transform.localScale;
        fallingSize.x = Math.Abs(value);
        falling.localScale = fallingSize;

        standSize = transform.localScale;
        standSize.x = reference.localScale.x - Math.Abs(value);
        stand.localScale = standSize;
    }

    private void UpdatePositions(bool isFirstFalling, Transform falling, Transform stand, Direction minDirection, Direction maxDirection)
    {
        fallingPosition = GetPositionEdge(referenceMesh, isFirstFalling ? minDirection : maxDirection);
        fallingMultiply = isFirstFalling ? 1 : -1;
        fallingPosition.x += (falling.localScale.x / 2) * fallingMultiply;
        falling.position = fallingPosition;

        standPosition = GetPositionEdge(referenceMesh, !isFirstFalling ? minDirection : maxDirection);
        standMultiply = !isFirstFalling ? 1 : -1;
        standPosition.x += (stand.localScale.x / 2) * standMultiply;
        stand.position = new Vector3(transform.position.x, standPosition.y, standPosition.z);
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
        origin = transform.localScale.x ;
        current = Mathf.Abs(distance.x) ;
        return current + endTolerance >= origin;
    }
}

public enum Direction
{
    Left,
    Right,
    Front,
    Back
}

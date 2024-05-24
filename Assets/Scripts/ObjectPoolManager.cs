using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [SerializeField] private GameObject fallingPrefab;
    [SerializeField] private GameObject standPrefab;
    [SerializeField] private int initialPoolSize = 10;

    private ObjectPool<Transform> fallingPool;
    private ObjectPool<Transform> standPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        fallingPool = new ObjectPool<Transform>(fallingPrefab.transform, initialPoolSize, transform);
        standPool = new ObjectPool<Transform>(standPrefab.transform, initialPoolSize, transform);
    }

    public Transform GetFallingPiece()
    {
        var fallingPiece = fallingPool.Get();

        ResetVelocity(fallingPiece);

        return fallingPiece;
    }

    private void ResetVelocity(Transform fallingPiece)
    {
        fallingPiece.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public Transform GetStandPiece()
    {
        return standPool.Get();
    }
    
}
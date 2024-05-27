using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{

    [SerializeField] private GameObject fallingPrefab;
    [SerializeField] private GameObject standPrefab;
    [SerializeField] private GameObject finishPrefab;

    [SerializeField] private int initialPoolSize = 10;

    private ObjectPool<Transform> fallingPool;
    private ObjectPool<Transform> standPool;
    private ObjectPool<Transform> finishPool;


    private void Awake()
    {
        InitializePools();

    }

    private void InitializePools()
    {
        fallingPool = new ObjectPool<Transform>(fallingPrefab.transform, initialPoolSize, transform);
        standPool = new ObjectPool<Transform>(standPrefab.transform, initialPoolSize, transform);
        finishPool = new ObjectPool<Transform>(finishPrefab.transform, 3, transform);

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
    
    public Transform GetFinishPlatform()
    {
        return finishPool.Get();
    }
    
}
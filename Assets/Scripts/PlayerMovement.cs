using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public PathManager pathManager;
    private Transform currentTarget;

    void Start()
    {
        if (pathManager.platforms.Count > 0)
        {
            SetNextTarget();
            StartCoroutine(MoveTowardsPlatform());
        }
    }

    void SetNextTarget()
    {
        currentTarget = pathManager.GetNextPlatform();
        if (currentTarget != null)
        {
            pathManager.RemoveFirstPlatform();
        }
    }

    IEnumerator MoveTowardsPlatform()
    {
        while (currentTarget != null)
        {
            Vector3 targetPosition = new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z);
            
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, forwardSpeed * Time.deltaTime);
                yield return null;
            }

            SetNextTarget();
        }

        Debug.Log("Hata: Kuyrukta başka platform yok.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // İsteğe bağlı: Platformla çarpışma sırasında başka bir işlem yapmak isterseniz buraya ekleyebilirsiniz
        }
    }
}
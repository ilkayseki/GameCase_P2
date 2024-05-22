using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;
    private GameObject currentPlatform;
    private GameObject previousPlatform;
    private Vector3 spawnPosition;

    private void Start()
    {
        spawnPosition = new Vector3(0, 0, 0); // Başlangıç pozisyonu
        CreatePlatform();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentPlatform != null)
            {
                StopPlatform();
                TrimPlatform();
                spawnPosition = currentPlatform.transform.position;
                spawnPosition.z += currentPlatform.transform.localScale.z; // Bir sonraki platformun spawn pozisyonu
                CreatePlatform();
            }
        }
    }

    private void CreatePlatform()
    {
        previousPlatform = currentPlatform;
        currentPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
    }

    private void StopPlatform()
    {
        if (currentPlatform != null)
        {
            var platformMovement = currentPlatform.GetComponent<CharacterController>();
            if (platformMovement != null)
            {
                platformMovement.Stop();
            }
        }
    }

    private void TrimPlatform()
    {
        if (previousPlatform == null || currentPlatform == null)
        {
            return;
        }

        float overlap = previousPlatform.transform.position.x - currentPlatform.transform.position.x;
        float cutAmount = Mathf.Abs(overlap);

        if (cutAmount >= currentPlatform.transform.localScale.x)
        {
            Debug.Log("Game Over");
            Destroy(currentPlatform);
            // Oyun bittiği zaman yapılacak işlemler
            return;
        }

        float newXScale = currentPlatform.transform.localScale.x - cutAmount;
        float newXPosition = currentPlatform.transform.position.x + (overlap / 2);

        currentPlatform.transform.localScale = new Vector3(newXScale, currentPlatform.transform.localScale.y, currentPlatform.transform.localScale.z);
        currentPlatform.transform.position = new Vector3(newXPosition, currentPlatform.transform.position.y, currentPlatform.transform.position.z);

        // Kesilen kısmı yok et
        Vector3 cutPosition;
        if (overlap > 0)
        {
            cutPosition = new Vector3(currentPlatform.transform.position.x + newXScale / 2, currentPlatform.transform.position.y, currentPlatform.transform.position.z);
        }
        else
        {
            cutPosition = new Vector3(currentPlatform.transform.position.x - newXScale / 2, currentPlatform.transform.position.y, currentPlatform.transform.position.z);
        }

        GameObject cutPiece = Instantiate(platformPrefab, cutPosition, Quaternion.identity);
        Destroy(cutPiece, 0.5f);
    }
}

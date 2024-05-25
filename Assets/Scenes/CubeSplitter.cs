using UnityEngine;

public class CubeSplitter : MonoBehaviour
{
    public GameObject purpleCube;
    public GameObject standPrefab;
    public GameObject fallingPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SplitCube();
        }
    }

    void SplitCube()
    {
        Vector3 purpleCubePosition = purpleCube.transform.position;
        Vector3 purpleCubeSize = purpleCube.transform.localScale;

        Vector3 yellowCubePosition = transform.position;
        Vector3 yellowCubeSize = transform.localScale;

        float yellowCubeRightEdge = yellowCubePosition.x + (yellowCubeSize.x / 2);
        float yellowCubeLeftEdge = yellowCubePosition.x - (yellowCubeSize.x / 2);
        float purpleCubeRightEdge = purpleCubePosition.x + (purpleCubeSize.x / 2);
        float purpleCubeLeftEdge = purpleCubePosition.x - (purpleCubeSize.x / 2);

        if (IsCoveringCompletely(yellowCubeRightEdge, yellowCubeLeftEdge, purpleCubeRightEdge, purpleCubeLeftEdge))
        {
            HandleCompleteCover(yellowCubeSize, purpleCubeSize, purpleCubePosition, yellowCubePosition);
        }
        else if (yellowCubeRightEdge > purpleCubeRightEdge)
        {
            HandleRightOverlap(yellowCubeRightEdge, yellowCubeLeftEdge, purpleCubeRightEdge, yellowCubeSize, yellowCubePosition);
        }
        else if (yellowCubeLeftEdge < purpleCubeLeftEdge)
        {
            HandleLeftOverlap(yellowCubeLeftEdge, yellowCubeRightEdge, purpleCubeLeftEdge, yellowCubeSize, yellowCubePosition);
        }
        else
        {
            HandlePartialOverlap(yellowCubeSize, yellowCubePosition);
        }

        Destroy(gameObject);
    }

    bool IsCoveringCompletely(float yellowRight, float yellowLeft, float purpleRight, float purpleLeft)
    {
        return yellowRight > purpleRight && yellowLeft < purpleLeft;
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

    void HandlePartialOverlap(Vector3 yellowSize, Vector3 yellowPosition)
    {
        float standSizeX = yellowSize.x;
        CreateStand(standSizeX, yellowSize, yellowPosition);
    }

    void CreateStand(float sizeX, Vector3 originalSize, Vector3 position)
    {
        if (sizeX > 0)
        {
            GameObject stand = Instantiate(standPrefab, position, transform.rotation);
            stand.transform.localScale = new Vector3(sizeX, originalSize.y, originalSize.z);
        }
    }

    void CreateFalling(float sizeX, Vector3 originalSize, Vector3 position)
    {
        if (sizeX > 0)
        {
            GameObject falling = Instantiate(fallingPrefab, position, transform.rotation);
            falling.transform.localScale = new Vector3(sizeX, originalSize.y, originalSize.z);
        }
    }
}

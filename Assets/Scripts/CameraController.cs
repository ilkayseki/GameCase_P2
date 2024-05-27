using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform target; 
    public float rotationSpeed = 20f; 
    public float transitionSpeed = 2f;

    private Vector3 initialOffset;
    private bool isRotatingAround = false;
    private bool isTransitioningBack = false;

    void Start()
    {
        // Başlangıç ofsetini kaydet
        initialOffset = virtualCamera.transform.position - target.position;
    }

    public void GameFinished()
    {
        isRotatingAround = true;
        isTransitioningBack = false;
    }

    public void StartNewGame()
    {
        isRotatingAround = false;
        isTransitioningBack = true;
    }

    void LateUpdate()
    {
        if (isRotatingAround)
        {
            virtualCamera.transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (isTransitioningBack)
        {
            Vector3 desiredPosition = target.position + initialOffset;
            virtualCamera.transform.position = Vector3.Lerp(virtualCamera.transform.position, desiredPosition, transitionSpeed * Time.deltaTime);
            virtualCamera.transform.LookAt(target);

            if (Vector3.Distance(virtualCamera.transform.position, desiredPosition) < 0.01f)
            {
                isTransitioningBack = false;
            }
        }
        else
        {
            virtualCamera.transform.LookAt(target);
        }
    }
}

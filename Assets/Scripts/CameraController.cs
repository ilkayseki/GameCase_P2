using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform target; // Takip edilecek karakter
    public Vector3 offset; // Kamera ile karakter arasındaki ofset
    public float rotationSpeed = 20f; // Dönme hızı
    public float transitionSpeed = 2f; // Geçiş hızı

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
            // Kamera karakterin etrafında dönsün
            virtualCamera.transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (isTransitioningBack)
        {
            // Kamerayı karakterin güncel pozisyonuna göre eski ofset pozisyonuna yavaşça geri döndür
            Vector3 desiredPosition = target.position + initialOffset;
            virtualCamera.transform.position = Vector3.Lerp(virtualCamera.transform.position, desiredPosition, transitionSpeed * Time.deltaTime);
            virtualCamera.transform.LookAt(target);

            // Eğer kamera hedef pozisyona yeterince yaklaştıysa geçişi durdur
            if (Vector3.Distance(virtualCamera.transform.position, desiredPosition) < 0.01f)
            {
                isTransitioningBack = false;
            }
        }
        else
        {
            // Kamera hedefe bakmaya devam etsin
            virtualCamera.transform.LookAt(target);
        }
    }
}

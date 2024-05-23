using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool isMoving = true;

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void Stop()
    {
        isMoving = false;
    }
}

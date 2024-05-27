using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class ETFXRotation : MonoBehaviour
    {
        [Header("Rotate axes by degrees per second")]
        public Vector3 rotateVector = Vector3.zero;

        public enum SpaceEnum { Local, World };
        public SpaceEnum rotateSpace = SpaceEnum.Local;

        public float duration = 5f; // Rotation duration in seconds

        // Use this for initialization
        void Start()
        {
            if (rotateVector != Vector3.zero)
            {
                StartCoroutine(RotateObject());
            }
        }

        private IEnumerator RotateObject()
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float deltaTimeRotation = Time.deltaTime * rotateVector.magnitude;
                if (rotateSpace == SpaceEnum.Local)
                    transform.Rotate(rotateVector * deltaTimeRotation);
                else
                    transform.Rotate(rotateVector * deltaTimeRotation, Space.World);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class ETFXLightFade : MonoBehaviour
    {
        [Header("Seconds to dim the light")]
        public float life = 0.2f;
        public bool killAfterLife = true;

        private Light li;
        private float initIntensity;

        // Use this for initialization
        void Start()
        {
            li = gameObject.GetComponent<Light>();
            if (li != null)
            {
                initIntensity = li.intensity;
                StartCoroutine(FadeLight());
            }
            else
            {
                Debug.LogWarning("No light object found on " + gameObject.name);
            }
        }

        private IEnumerator FadeLight()
        {
            float elapsedTime = 0f;

            while (elapsedTime < life)
            {
                li.intensity = Mathf.Lerp(initIntensity, 0, elapsedTime / life);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            li.intensity = 0;

            if (killAfterLife)
            {
                Destroy(li);
            }
        }
    }
}
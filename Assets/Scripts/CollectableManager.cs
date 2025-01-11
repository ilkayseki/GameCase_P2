using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] List<Collectable> _coinPool;
    [SerializeField] List<Collectable> _starPool;
    [SerializeField] List<Collectable> _gemPool;
    [SerializeField] float _startYPosition = 10f;
    [SerializeField] float _finishYPosition = 10f;

    [SerializeField] float _duration = 1f;

    [Inject] private PieceController pieceController;

    [SerializeField]float minDistanceForEveryCollectible = 2f; // Minimum mesafe (örneğin 1 birim)
    public void RandomCollectibles()
    {
        HandOutAllCollectables((int)pieceController.transform.position.z,(int)pieceController.transform.position.z+12);
    }
    
    public void HandOutAllCollectables(int minZ, int maxZ)
    {
        var usedZPositions = new List<float>(); // Kullanılmış Z pozisyonlarını saklıyoruz

        foreach (var collectablePool in new List<List<Collectable>> { _coinPool, _starPool, _gemPool })
        {
            foreach (var collectable in collectablePool)
            {
                float randZ;
                int attempts = 0;
                const int maxAttempts = 100; // Sonsuz döngüye girmemek için bir sınır koyuyoruz

                do
                {
                    randZ = Random.Range(minZ, maxZ);
                    attempts++;

                    // Eğer deneme sayısı sınırı aşarsa döngüden çık (pozisyon bulamazsa).
                    if (attempts > maxAttempts)
                    {
                        Debug.LogWarning("Could not find a suitable Z position for a collectable.");
                        break;
                    }

                } while (usedZPositions.Exists(z => Mathf.Abs(z - randZ) < minDistanceForEveryCollectible));

                // Eğer uygun pozisyon bulunamazsa bu nesneyi atla.
                if (attempts > maxAttempts) continue;

                usedZPositions.Add(randZ);

                collectable.transform.position = new Vector3(0, _startYPosition, randZ);
                collectable.gameObject.SetActive(true);
                collectable.transform.DOMoveY(_finishYPosition, _duration).SetEase(Ease.OutCirc);
            }
        }
    }
}
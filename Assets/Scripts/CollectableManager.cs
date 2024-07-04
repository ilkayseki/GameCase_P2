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

    public void RandomCollectibles()
    {
        HandOutAllCollectables((int)pieceController.transform.position.z,(int)pieceController.transform.position.z+10);
    }
    
    public void HandOutAllCollectables(int minZ, int maxZ)
    {
        var usedZPositions = new HashSet<float>();

        foreach (var collectablePool in new List<List<Collectable>> { _coinPool, _starPool, _gemPool })
        {
            foreach (var collectable in collectablePool)
            {
                float randZ;
                do
                {
                    randZ = Random.Range(minZ, maxZ);
                } while (usedZPositions.Contains(randZ));

                usedZPositions.Add(randZ);

                collectable.transform.position = new Vector3(0, _startYPosition, randZ);
                collectable.gameObject.SetActive(true);
                collectable.transform.DOMoveY(_finishYPosition, _duration).SetEase(Ease.OutBack);
            }
        }
    }
}
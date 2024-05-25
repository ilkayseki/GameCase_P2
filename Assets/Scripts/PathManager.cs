using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public Queue<Transform> platforms = new Queue<Transform>();

    public List<Transform> platforms2 = new List<Transform>();

    void Start()
    {
        foreach (Transform platform in platforms2)
        {
            platforms.Enqueue(platform);
        }
    }
    
    public void AddPlatform(Transform platform)
    {
        platforms.Enqueue(platform);
    }

    public void RemoveFirstPlatform()
    {
        if (platforms.Count > 0)
        {
            Transform firstPlatform = platforms.Dequeue();
            //Destroy(firstPlatform.gameObject);
        }
    }

    public Transform GetNextPlatform()
    {
        if (platforms.Count > 0)
        {
            return platforms.Peek();
        }
        else
        {
            Debug.Log("Hata: Kuyrukta başka platform yok.");
            return null;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PathManager : MonoBehaviour
{
    public Queue<Transform> platforms = new Queue<Transform>();

    public List<Transform> pivotPlatforms = new List<Transform>();

    public GameObject LastPlatformFinish;
    
    void Awake()
    {
        foreach (Transform platform in pivotPlatforms)
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
            //Debug.Log("Hata: Kuyrukta başka platform yok.");
            return null;
        }
    }

    public void SetLastPlatform(GameObject last)
    {
        LastPlatformFinish = last;
    }
    public GameObject GetLastPlatform()
    {
        return LastPlatformFinish;
    }
    
}
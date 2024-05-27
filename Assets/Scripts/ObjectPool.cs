using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly List<T> objects = new List<T>();
    private readonly Transform parent;
    private int index = 0;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        AddObjects(initialSize);
    }

    public T Get()
    {
        if (objects.Count == 0)
        {
            Debug.LogWarning("Object pool is empty. Refilling the pool with existing objects.");
            RefillPool();
        }

        var obj = objects[index];
        obj.gameObject.SetActive(true);
        index = (index + 1) % objects.Count; // Rotate index to next object
        return obj;
    }

    private void RefillPool()
    {
        var objectsOfTypeT = Object.FindObjectsOfType<T>();
        Array.Sort(objectsOfTypeT, (x, y) => x.transform.position.z.CompareTo(y.transform.position.z));

        objects.AddRange(objectsOfTypeT);
        Debug.Log("Object pool has been refilled.");
    }

    private void AddObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            objects.Add(obj);
        }
    }
}
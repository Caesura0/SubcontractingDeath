using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField] GameObject afterImagePrefab;

    private Queue<GameObject> avaliableObjects = new Queue<GameObject>();

    public static PlayerAfterImagePool Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < 10; i++)
        {
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instanceToAdd)
    {
        instanceToAdd.SetActive(false);
        avaliableObjects.Enqueue(instanceToAdd);
    }

    public GameObject GetFromPool()
    {
        if (avaliableObjects.Count == 0)
        {
            GrowPool();
        }
        var instance = avaliableObjects.Dequeue();
        Debug.Log("getfrompool");
        instance.SetActive(true);
        return instance;
    }
}

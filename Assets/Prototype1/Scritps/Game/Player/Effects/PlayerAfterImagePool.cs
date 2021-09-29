using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField] private GameObject prefabAfterImage;

    private Queue<GameObject> aviableObject = new Queue<GameObject>();

    public static PlayerAfterImagePool Instance { get; private set; }

    public int poolSize;

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var instanceToAdd = Instantiate(prefabAfterImage);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        aviableObject.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if (aviableObject.Count == 0)
            GrowPool();

        var instance = aviableObject.Dequeue();
        
        if(instance != null)
            instance.SetActive(true);

        return instance;
    }
}

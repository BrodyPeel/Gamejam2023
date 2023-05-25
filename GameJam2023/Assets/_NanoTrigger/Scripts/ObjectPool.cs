using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> pooledSmallProjectile;
    //public List<GameObject> pooledLargeProjectile;
    public GameObject smallProjectile;
    //public GameObject largeProjectile;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledSmallProjectile = new List<GameObject>();
        //pooledLargeProjectile = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(smallProjectile);
            tmp.SetActive(false);
            pooledSmallProjectile.Add(tmp);
        }
        /*for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(largeProjectile);
            tmp.SetActive(false);
            pooledLargeProjectile.Add(tmp);
        }*/
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledSmallProjectile[i].activeInHierarchy)
            {
                return pooledSmallProjectile[i];
            }
        }
        return null;
    }
    /*public GameObject GetPooledLargeObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledLargeProjectile[i].activeInHierarchy)
            {
                return pooledLargeProjectile[i];
            }
        }
        return null;
    }*/
}

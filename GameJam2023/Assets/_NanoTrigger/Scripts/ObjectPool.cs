using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> pooledSmallProjectile;
    public List<GameObject> pooledLargeProjectile;
    public List<GameObject> pooledEnemyProjectileOne;
    public List<GameObject> pooledEnemyProjectileTwo;
    public GameObject smallProjectile;
    public GameObject largeProjectile;
    public GameObject enemyProjectileOne;
    public GameObject enemyProjectileTwo;
    public int amountToPool;
    public int amountToPoolEnemy;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledSmallProjectile = new List<GameObject>();
        pooledLargeProjectile = new List<GameObject>();
        pooledEnemyProjectileOne = new List<GameObject>();
        pooledEnemyProjectileTwo = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(smallProjectile);
            tmp.SetActive(false);
            pooledSmallProjectile.Add(tmp);
        }
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(largeProjectile);
            tmp.SetActive(false);
            pooledLargeProjectile.Add(tmp);
        }
        for (int i = 0; i < amountToPoolEnemy; i++)
        {
            tmp = Instantiate(enemyProjectileOne);
            tmp.SetActive(false);
            pooledEnemyProjectileOne.Add(tmp);
        }
        for (int i = 0; i < amountToPoolEnemy; i++)
        {
            tmp = Instantiate(enemyProjectileTwo);
            tmp.SetActive(false);
            pooledEnemyProjectileTwo.Add(tmp);
        }

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
    public GameObject GetPooledLargeObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledLargeProjectile[i].activeInHierarchy)
            {
                return pooledLargeProjectile[i];
            }
        }
        return null;
    }

    public GameObject GetPooledEnemyProjectileOne()
    {
        for (int i = 0; i< amountToPoolEnemy; i++)
        {
            if(!pooledEnemyProjectileOne[i].activeInHierarchy)
            {
                return pooledEnemyProjectileOne[i];
            }
        }
        return null;
    }

    public GameObject GetPooledEnemyProjectileTwo()
    {
        for (int i = 0; i < amountToPoolEnemy; i++)
        {
            if (!pooledEnemyProjectileTwo[i].activeInHierarchy)
            {
                return pooledEnemyProjectileTwo[i];
            }
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Enemy
{

    public GameObject Spawn;
    public int spawnMax;
    public float spawnInterval;

    private float nextSpawn;
    private int spawned;

    // Start is called before the first frame update
    void Start()
    {
        health = 10.0f;
        moveSpeed = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Spawn != null)
        {
            if (spawned < spawnMax && Time.time >= nextSpawn)
            {
                //TODO update spawn position?
                SpawnObject();
                
            }
        }
        else
        {
            Debug.Log("There is no Spawn Object");
        }

        if (health <= 0.0f)
        {
            Death();
            Destroy(this);
        }
    }    

    public void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(Spawn, transform.position, transform.rotation);
        spawnedObject.GetComponent<Spawned>().OnDeath += HandleSpawnedDeath;
        spawned++;        
    }
    private void HandleSpawnedDeath()
    {
        // Handle the death event of the spawned object
        spawned--;
    }

}

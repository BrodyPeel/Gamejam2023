using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Enemy
{
    public GameObject spawnPrefab;
    public int spawnMax;
    public float spawnInterval;
    public Transform spawnPosition;

    private float nextSpawn;
    private int spawned;

    private bool spawning;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        moveSpeed = 0.0f;
        nextSpawn = Time.time + spawnInterval;
        spawning = true;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (dead) return;

        if (GameManager.Instance.state.isState("PlayState"))
        {
            if (spawnPrefab != null)
            {
                if (spawned < spawnMax && Time.time >= nextSpawn && spawning)
                {
                    //TODO update spawn position?
                    SpawnObject();
                    nextSpawn = Time.time + spawnInterval;

                }
            }
            else
            {
                Debug.Log("There is no Spawn Object");
            }

            if (health <= 0.0f)
            {
                spawning = false;
                Death();
            }
        }
    }    

    public void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(spawnPrefab, spawnPosition.position, spawnPosition.rotation);
        spawnedObject.GetComponent<Spawned>().OnDeath += HandleSpawnedDeath;
        AudioController.Instance.PlaySFX(SFX.EnemySpawn2);
        spawned++;        
    }
    private void HandleSpawnedDeath()
    {
        // Handle the death event of the spawned object
        spawned--;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        AudioController.Instance.PlaySFX(SFX.EnemySpawn1);
    }

}

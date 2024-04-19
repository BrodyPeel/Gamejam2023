using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Enemy
{
    public GameObject spawnPrefab;
    public int spawnMax;
    public float spawnInterval;
    public Transform spawnPosition;
    public float spawnDistance = 10f; // Distance within which the player needs to be for the spawner to be active

    private float nextSpawn;
    private int spawned;

    private bool spawning;

    // Reference to the player's transform
    private Transform playerTransform;

    public override void Start()
    {
        base.Start();
        moveSpeed = 0.0f;
        nextSpawn = Time.time + spawnInterval;
        spawning = true;

        // Assuming you have a player object tagged with "Player" in your scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (dead || playerTransform == null) return;

        if (GameManager.Instance.state.isState("PlayState"))
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= spawnDistance)
            {
                if (spawnPrefab != null && spawned < spawnMax && Time.time >= nextSpawn && spawning)
                {
                    SpawnObject();
                    nextSpawn = Time.time + spawnInterval;
                }
            }
            else
            {
                return;
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

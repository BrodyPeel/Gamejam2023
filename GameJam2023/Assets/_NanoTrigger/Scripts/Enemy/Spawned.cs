using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawned : Enemy
{
    public delegate void SpawnedDeathEventHandler();
    public event SpawnedDeathEventHandler OnDeath;

    // Start is called before the first frame update
    void Start()
    {
        health = 15.0f;
        moveSpeed = 2.0f;
        sightRadius = 20.0f;
        attackRadius = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0.0f)
        {
            HandleDeath();
        }
    }
    private void HandleDeath()
    {
        // Perform death logic

        // Invoke the event
        OnDeath?.Invoke();
    }

    private void Move()
    {

    }

    private void Attack()
    {

    }
}

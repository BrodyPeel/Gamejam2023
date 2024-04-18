using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    Rigidbody2D rb;

    public override void Start()
    {
        base.Start();

        health = 15.0f;
        moveSpeed = 2.0f;
        sightRadius = 10.0f;
        attackRadius = 7.5f;
        rotationSpeed = 5.0f;

        rb = this.GetComponent<Rigidbody2D>();

        GameObject playerShip = GameObject.FindGameObjectWithTag("Player");
        if (playerShip != null)
        {
            PlayerPosition = playerShip.transform;
        }
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (dead) return;
        playerShipTransform = PlayerPosition.transform.position;
        enemyPosition = this.transform.position;

        if (health <= 0.0f)
        {
            currentState = EnemyState.Death;
        }
        if (GameManager.Instance.state.isState("PlayState"))
        {
            //if idle move about freely
            if (currentState == EnemyState.Idle)
            {
                if (canMove)
                {
                    Move();
                }
            }
            else if (currentState == EnemyState.Chase)
            {
                if (canMove)
                {
                    Chase();
                }
            }
            else if (currentState == EnemyState.Attack)
            {
                if (canMove)
                {
                    Chase();
                }
                Attack();
            }
            else if (currentState == EnemyState.Death)
            {
                HandleDeath();
            }

            //check to see where player is. Change state accordingly
            if (Vector2.Distance(playerShipTransform, enemyPosition) <= sightRadius &&
                Vector2.Distance(playerShipTransform, enemyPosition) > attackRadius)
            {
                currentState = EnemyState.Chase;
            }
            else if (Vector2.Distance(playerShipTransform, enemyPosition) <= attackRadius)
            {
                currentState = EnemyState.Attack;
            }
            else if (Vector2.Distance(playerShipTransform, enemyPosition) > sightRadius)
            {
                currentState = EnemyState.Idle;
            }
            else if (health <= 0.0f)
            {
                currentState = EnemyState.Death;
            }
        }
    }

    private void HandleDeath()
    {
        // Perform death animation
        //stop movement        
        // Invoke the event        
        Death();
        Destroy(this.gameObject);
    }

    private void Move()
    {
        //pick random direction to move
        Vector2 wanderDirection = Random.insideUnitCircle.normalized;

        // Rotate the enemy towards the wander direction using slerp
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, wanderDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Apply force to move the enemy in the wander direction
        rb.AddForce(wanderDirection * moveSpeed);
    }

    private void Chase()
    {
        //find direction of playership
        Vector2 playerDirection = (playerShipTransform - enemyPosition).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //addforce to move in direction of playership
        rb.AddForce(playerDirection * moveSpeed);
    }

    private void Attack()
    {
        //instantiate projectiles? 
        //fire towards player
        Debug.Log("ATTACK");
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}

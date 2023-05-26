using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, 
    Chase,
    Attack,
    Death
}

public class Enemy : MonoBehaviour
{
    protected delegate void enemyDeathHandler();
    protected event enemyDeathHandler OnEnemyDeath;

    protected EnemyState currentState;
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float sightRadius;
    [SerializeField]
    protected float attackRadius;
    [SerializeField]
    protected float rotationSpeed;

    protected Vector2 enemyPosition;
    protected Transform PlayerPosition;
    protected Vector2 playerShipTransform;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Idle;

        
    }

    // Update is called once per frame
    void Update()
    {
       
        switch (currentState)
        {
            case EnemyState.Idle:
                // Handle idle state behavior
                //if see the nanobot currentState = EnemyState.Chase
                //if nanobot within attackradius currentState = EnemyState.Attack.
                break;
            case EnemyState.Chase:
                // Handle chase state behavior
                break;
            case EnemyState.Attack:
                // Handle attack state behavior
                break;
            case EnemyState.Death:
                //handle death behavior
                break;
        }

    }

    public void TakeDamage(float damage)
    {
        //animation for damage?
        health -= damage;
    }

    public void Death()
    {
        OnEnemyDeath?.Invoke();
    }
}
